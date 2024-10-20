using OrderService1.Contracts;

namespace OrderService1.Repositories;

public class QueueRepository : IQueueRepository
{
    //private Queue<QueueItemDto> _queue = new();
    private List<QueueItemDto> _queue = new();
    
    public void Enqueue(QueueItemDto queueItemDto)
    {
        _queue.Add(queueItemDto);
    }
    
    public void Dequeue(QueueItemDto queueItemDto)
    {
        var toRemove = _queue.First(i => i.Client.Id == queueItemDto.Client.Id 
                                         && i.Product.Name == queueItemDto.Product.Name);
        _queue.Remove(queueItemDto);
        _queue.Add(queueItemDto);
    }
    
    public void Enqueue(QueueItemDto[] queueItems)
    {
        _queue.AddRange(queueItems);
    }

    public List<QueueItemDto> GetQueue()
    {
        return _queue;
    }

    public void ResetQueue(QueueItemDto[] newQueue)
    {
        var notInMainQueue = _queue
            .Where(i => newQueue.Any(n => n.Client.Id != i.Client.Id 
                                                            && n.Product.Name != i.Product.Name))
            .ToList();
        
        if (notInMainQueue.Count == 0)
        {
            _queue.Clear();
            _queue.AddRange(newQueue);
            return;
        }

        var localNewQueue = newQueue.ToList();
        localNewQueue.AddRange(notInMainQueue);
        
        _queue.Clear();
        _queue.AddRange(localNewQueue);
    }
}