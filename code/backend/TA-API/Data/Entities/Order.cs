namespace TA_API.Data.Entities;

public class Order
{
    public int OrderId { get; set; }
    public int CustomerId { get; set; }
    public List<Item> Items { get; set; } = new();
    public double Total { get; set; }
}
