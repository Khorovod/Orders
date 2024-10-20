using OrderService2.Contracts;
using OrderService2.Repositories;
using OrderService2.Services.OrdersService.Handlers;
using OrderService2.Services.ProductsService;
using OrderService2.Services.ProductsService.Handlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var orchestratorUrl = builder.Configuration.GetSection("OrchestratorUrl").Value;


builder.Services.AddSingleton<IQueueRepository, QueueRepository>();
builder.Services.AddSingleton<IProductsHandler, ProductsHandler>();
builder.Services.AddSingleton<IOrdersHandler, OrdersHandler>();
builder.Services.AddHttpClient<IProductsHandler, ProductsHandler>(client => client.BaseAddress = new Uri(orchestratorUrl));
builder.Services.AddHttpClient<IOrdersHandler, OrdersHandler>(client => client.BaseAddress = new Uri(orchestratorUrl));

builder.Services.AddHostedService<ProductsBookingService>();

var app = builder.Build();

app.UseRouting();
app.UseSwagger();
app.UseSwaggerUI();

// Пользователь делает новый заказ
app.MapPost("/create", (QueueItemDto dto, IOrdersHandler handler) =>
{
    handler.CreateOrder(dto);
});

app.MapGet("/order", (int clientId, IQueueRepository repository) =>
{
    return repository.GetQueue().Where(i => i.Client.Id == clientId);
});

// Получить новую очередь для мониторинга
app.MapPost("/sync", (QueueItemDto[] queue, IQueueRepository repository) =>
{
    repository.ResetQueue(queue);
});

app.Run();