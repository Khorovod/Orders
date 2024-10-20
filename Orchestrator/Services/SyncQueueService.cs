using Microsoft.Extensions.Options;
using Orchestrator.Contracts;
using Orchestrator.Infrastructure;

namespace Orchestrator.Services;

public class SyncQueueService : ISyncQueueService
{
    private readonly IOptions<OrderServicesConfig> _config;
    private readonly List<QueueItemDto> _queue = new();

    public SyncQueueService(IOptions<OrderServicesConfig> config)
    {
        _config = config;
    }

    public QueueItemDto[] GetQueue()
    {
        return _queue.ToArray();
    }
    
    public async Task MoveToEnd(QueueItemDto item)
    {
        var toRemove = _queue.First(i => i.Client.Id == item.Client.Id 
                                         && i.Product.Name == item.Product.Name);
        _queue.Remove(toRemove);
        _queue.Add(item);

        await SyncQueue();
    }
    
    public async Task AddToQueue(QueueItemDto item)
    {
        _queue.Add(item);
        
        await SyncQueue();
    }

    /// <summary>
    /// О каждом изменении в очереди сообщаем сервисам.
    /// </summary>
    private async Task SyncQueue()
    {
        using var client = new HttpClient();
        
        var addresses = _config.Value.Resources;
        
        foreach (var address in addresses)
        {
            client.BaseAddress = new Uri(address);
            
            await client.PostAsync("/sync", JsonContent.Create(_queue.ToArray()));
        }
    }
}