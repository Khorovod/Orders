using OrderService1.Services.ProductsService.Handlers;

namespace OrderService1.Services.ProductsService;

public class ProductsBookingService : BackgroundService
{
    private readonly IProductsHandler _handler;

    public ProductsBookingService(IProductsHandler handler)
    {
        _handler = handler;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var items = await ExternalApi.GetAvailableProducts();

            if (items.Any())
            {
                await _handler.MakeBooking(items, stoppingToken);
            }

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }
    
}