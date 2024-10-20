using OrderService1.Contracts;

namespace OrderService1;

internal static class ExternalApi
{
    private static readonly Product[] Products =
    {
        new() {Name = "Укроп"},
        new() {Name = "Кошка"},
        new() {Name = "Ведро"},
        new() {Name = "Вода"},
        new() {Name = "Дрова"}
    };

    public static async Task<Product[]> GetAvailableProducts()
    {
        var rnd = new Random();
        var chance  = rnd.Next(1, 5);

        if (chance != 1) return await Task.Run(Array.Empty<Product>);
        
        var item = rnd.Next(0, Products.Length-1);
        return await Task.FromResult(new [] { Products[item] });
    }

    public static async Task BookProduct(Product product)
    {
        if (product == null) throw new ArgumentNullException(nameof(product));
    }
}