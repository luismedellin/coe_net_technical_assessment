using TA_API.Data.Entities;

namespace TA_API.Data.Repository;

public interface IOrderRepository
{
    Task<Order?> GetOrderById(int id);
    Task Save(Order order);
}