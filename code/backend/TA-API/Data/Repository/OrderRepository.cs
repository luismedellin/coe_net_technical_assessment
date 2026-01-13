using Microsoft.EntityFrameworkCore;
using TA_API.Data.Entities;

namespace TA_API.Data.Repository;

public class OrderRepository : IOrderRepository
{
    private readonly AssessmentDbContext _context;

    public OrderRepository(AssessmentDbContext context)
    {
        _context = context;
    }

    public async Task<Order?> GetOrderById(int id)
    {
        try
        {
            var order = _context.Orders
                .Include(o => o.Items)
                .AsNoTracking()
                .FirstOrDefault(o => o.OrderId == id);

            return order;
        }
        catch (Exception ex)
        {
            throw;
        }
    }


    public async Task Save(Order order)
    {
        //TODO: avoid SaveChangesAsync here, move to service layer, use unit of work pattern to validate products
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
    }
}
