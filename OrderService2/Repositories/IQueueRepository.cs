using OrderService2.Contracts;

namespace OrderService2.Repositories;

public interface IQueueRepository
{
    void Enqueue(QueueItemDto queueItemDto);
    void Enqueue(QueueItemDto[] queueItems);
    void Dequeue(QueueItemDto queueItemDto);
    List<QueueItemDto> GetQueue();
    void ResetQueue(QueueItemDto[] newQueue);
}