using Microsoft.EntityFrameworkCore;
using TA_API.Data.Entities;

namespace TA_API.Data;

public class AssessmentDbContext : DbContext
{
    public AssessmentDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; }
    public DbSet<Item> OrderItems { get; set; }
}
