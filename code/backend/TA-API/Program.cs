using Microsoft.EntityFrameworkCore;
using Serilog;
using TA_API.Core.Services;
using TA_API.Data;
using TA_API.Data.Repository;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console());
    builder.Services.AddDbContext<AssessmentDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("AssessmentDB")));

    // Services
    builder.Services.AddScoped<IOrderService, OrderService>();

    // Repositories
    builder.Services.AddScoped<IOrderRepository, OrderRepository>();
}

var app = builder.Build();
{
    app.UseSerilogRequestLogging();
    app.MapGet("/", () => "Technical Assessment API");
    app.MapGet("/lbhealth", () => "Technical Assessment API");

    // Orders
    app.MapGet("/orders/{id}", async (int id, IOrderService service) =>
    {
        var order = await service.GetOrderById(id);
        return order is null ? Results.NotFound("Order not found") : Results.Ok(order);
    });
}
app.Run();
