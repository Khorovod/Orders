namespace OrderService1.Contracts;

public class QueueItemDto
{
    public Guid Id { get; set; }
    public Product Product { get; set; }
    public Client Client { get; set; }
}