using Orchestrator.Contracts;
using Orchestrator.Infrastructure;
using Orchestrator.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var section = builder.Configuration.GetSection("OrderServicesConfig");
builder.Services.Configure<OrderServicesConfig>(section);
builder.Services.AddSingleton<ISyncQueueService, SyncQueueService>();


var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();


app.MapPost("/complete", async (QueueItemDto item, ISyncQueueService service) =>
{
    try
    {
        await service.MoveToEnd(item);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        throw;
    }
    
    Notification.Notify(item.Client.Id, item.Product.Name);
});

app.MapPost("/add", async (QueueItemDto item, ISyncQueueService service) =>
{
    await service.AddToQueue(item);
});

app.MapGet("/queue", (ISyncQueueService service) => service.GetQueue());

app.Run();


internal abstract class Notification
{
    public static void Notify(int client, string product) => Console.WriteLine($"Для клиента {client} был куплен {product}");
}
