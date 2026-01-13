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

    // Swagger
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

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

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.MapGet("/", () => "Technical Assessment API");
    app.MapGet("/lbhealth", () => "Technical Assessment API");

    // Orders
    app.MapGet("/orders/{id}", async (int id, IOrderService service) =>
    {
        var order = await service.GetOrderById(id);
        return order is null ? Results.NotFound("Order not found") : Results.Ok(order);
    });

    app.MapPost("/orders/", async (OrderDto orderDto,
        IValidator<OrderDto> validator,
        IOrderService service,
        ILogger<Program> logger) =>
    {
        try
        {
            var validationResult = await validator.ValidateAsync(orderDto);
            if (!validationResult.IsValid)
            {
                logger.LogWarning("Validation failed for CustomerId {CustomerId}", orderDto.CustomerId);
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var order = await service.Save(orderDto);
            return Results.Ok(order);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Unhandled error saving order for CustomerId {orderDto.CustomerId}");
            return Results.Problem("An error occurred processing your request");
        }
    });
}
app.Run();
