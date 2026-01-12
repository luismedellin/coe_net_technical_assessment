using TA_API.Data.Entities;
using TA_API.Data.Repository;

namespace TA_API.Core.Services;

public class OrderService : IOrderService
{
    readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Order?> GetOrderById(int id)
    {
        var order = await _orderRepository.GetOrderById(id);

        return order;
    }
}
