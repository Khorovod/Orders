using OrderService1.Contracts;

namespace OrderService1.Services.OrdersService.Handlers;

public interface IOrdersHandler
{
    Task CreateOrder(QueueItemDto dto);
}