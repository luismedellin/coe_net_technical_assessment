using FluentValidation;
using TA_API.Core.Dtos;
using TA_API.Core.Services;

namespace TA_API.Endpoints;

public static class OrdersEndpoints
{
    public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/orders", () => "Orders endpoint is working!");

        routes.MapGet("/orders/{id}", async (int id, IOrderService service) =>
        {
            var order = await service.GetOrderById(id);
            return order is null ? Results.NotFound("Order not found") : Results.Ok(order);
        });

        routes.MapPost("/orders/", async (OrderDto orderDto,
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

        return routes;
    }
}
