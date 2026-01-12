using TA_API.Data.Entities;

namespace TA_API.Core.Services
{
    public interface IOrderService
    {
        Task<Order?> GetOrderById(int id);
    }
}