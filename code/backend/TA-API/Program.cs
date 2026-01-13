using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;
using TA_API.Core.Dtos;
using TA_API.Core.Services;
using TA_API.Core.Validators;
using TA_API.Data;
using TA_API.Data.Repository;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console());
    builder.Services.AddDbContext<AssessmentDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("AssessmentDB")));

    // Validators
    builder.Services.AddValidatorsFromAssemblyContaining<OrderDtoValidator>();

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

    app.MapPost("/orders/", async (OrderDto orderDto, IValidator<OrderDto> validator, IOrderService service) =>
    {
        var validationResult = await validator.ValidateAsync(orderDto);

        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var order = await service.Save(orderDto);

        return Results.Ok(order);
    });
}
app.Run();
