using OrderService2.Contracts;

namespace OrderService2.Services.ProductsService.Handlers;

public interface IProductsHandler
{
    Task MakeBooking(Product[] products, CancellationToken cancellationToken);
}