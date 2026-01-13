using TA_API.Core.Dtos;
using TA_API.Data.Entities;
using TA_API.Data.Repository;

namespace TA_API.Core.Services;

public class OrderService : IOrderService
{
    readonly IOrderRepository _orderRepository;
    readonly ILogger<OrderService> _logger;

    public OrderService(IOrderRepository orderRepository, ILogger<OrderService> logger)
    {
        _orderRepository = orderRepository;
        _logger = logger;
    }

    public async Task<Order?> GetOrderById(int id)
    {
        var order = await _orderRepository.GetOrderById(id);

        if (order == null)
        {
            _logger.LogWarning("Order with ID {OrderId} not found.", id);
            return null;
        }

        return order;
    }

    public async Task<Order> Save(OrderDto orderDto)
    {
        _logger.LogInformation($"Starting to save order for CustomerId {orderDto.CustomerId} with {orderDto.Items.Count()} items");

        // TODO: validate products exist in product catalog

        double total = 0d;
        var items = orderDto.Items.Select(i =>
        {
            total += Convert.ToDouble(i.Quantity * i.Price);

            return new Item
            {
                Name = $"Product {i.Id}",
                Quantity = i.Quantity,
                Price = i.Price
            };
        }).ToList();

        var order = new Order
        {
            CustomerId = orderDto.CustomerId,
            Total = total,
            Items = items
        };
        await _orderRepository.Save(order);

        _logger.LogInformation($"Order {order.OrderId} saved successfully. Total: {order.Total}");

        return order;
    }
}
