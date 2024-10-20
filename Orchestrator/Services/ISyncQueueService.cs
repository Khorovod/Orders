using Orchestrator.Contracts;

namespace Orchestrator.Services;

public interface ISyncQueueService
{
    Task MoveToEnd(QueueItemDto item);
    QueueItemDto[] GetQueue();
    Task AddToQueue(QueueItemDto item);
}