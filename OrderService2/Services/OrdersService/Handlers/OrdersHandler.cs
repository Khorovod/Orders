using OrderService2.Contracts;
using OrderService2.Repositories;

namespace OrderService2.Services.OrdersService.Handlers;

public class OrdersHandler : IOrdersHandler
{
    private readonly IQueueRepository _repository;
    private readonly HttpClient _httpClient;

    public OrdersHandler(IQueueRepository repository, HttpClient httpClient)
    {
        _repository = repository;
        _httpClient = httpClient;
    }

    public async Task CreateOrder(QueueItemDto dto)
    {
        try
        {
            _repository.Enqueue(dto);
        
            await _httpClient.PostAsync("/add",
                JsonContent.Create(dto));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}