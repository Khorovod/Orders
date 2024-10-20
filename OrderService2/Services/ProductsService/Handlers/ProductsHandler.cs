using OrderService2.Contracts;
using OrderService2.Repositories;

namespace OrderService2.Services.ProductsService.Handlers;

public class ProductsHandler : IProductsHandler
{
    private readonly HttpClient _httpClient;
    private readonly IQueueRepository _repository;

    public ProductsHandler(HttpClient httpClient, IQueueRepository repository)
    {
        _httpClient = httpClient;
        _repository = repository;
    }

    public async Task MakeBooking(Product[] products, CancellationToken cancellationToken)
    {
        var itemsToBuy = _repository
            .GetQueue()
            .Where(i => products
                .Select(p => p.Name)
                .Contains(i.Product.Name))
            .ToList();
        
        try
        {
            foreach (var itemToBuy in itemsToBuy)
            {
                await ExternalApi.BookProduct(itemToBuy.Product);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        await UpdateQueue(itemsToBuy, cancellationToken);
        
        itemsToBuy.ForEach(i => _repository.GetQueue().Remove(i));
    }

    private async Task UpdateQueue(List<QueueItemDto> items, CancellationToken cancellationToken)
    {
        try
        {
            foreach (var item in items)
            {
                await _httpClient.PostAsync("/complete",
                    JsonContent.Create(item),
                    cancellationToken);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}