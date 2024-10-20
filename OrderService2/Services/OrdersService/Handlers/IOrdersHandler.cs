using OrderService2.Contracts;

namespace OrderService2.Services.OrdersService.Handlers;

public interface IOrdersHandler
{
    Task CreateOrder(QueueItemDto dto);
}