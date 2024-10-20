using OrderService1.Contracts;

namespace OrderService1.Services.ProductsService.Handlers;

public interface IProductsHandler
{
    Task MakeBooking(Product[] products, CancellationToken cancellationToken);
}