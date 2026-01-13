namespace TA_API.Core.Dtos;

public class OrderDto
{
    public int CustomerId { get; set; }
    public List<ItemDto> Items { get; set; } = new();
}


public class ItemDto
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}