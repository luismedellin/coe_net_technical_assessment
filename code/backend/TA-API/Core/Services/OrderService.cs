using TA_API.Core.Dtos;
using TA_API.Core.Utils;
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

        double total = 0d;
        var items = orderDto.Items.Select(i =>
        {
            var product = DataUtils.Products.FirstOrDefault(p => p.Id == i.Id);
            var productPrice = product?.Price ?? 0m;
            // TODO: validate products exist in product catalog
            total += Convert.ToDouble(i.Quantity * productPrice);

            return new Item
            {
                Name = product?.Name ?? string.Empty,
                Quantity = i.Quantity,
                Price = productPrice
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
