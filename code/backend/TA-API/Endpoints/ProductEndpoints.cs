namespace TA_API.Endpoints;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/products", () =>
        {
            // TODO: create product migration, repository and service
            var products = new[]
            {
                new { Id = 1, Name = "Product 1", Price = 10.0 },
                new { Id = 2, Name = "Product 2", Price = 20.0 },
                new { Id = 3, Name = "Product 3", Price = 30.0 },
                new { Id = 4, Name = "Product 4", Price = 30.0 },
                new { Id = 5, Name = "Product 5", Price = 30.0 },
                new { Id = 6, Name = "Product 6", Price = 30.0 },
            };

            return Results.Ok(products);
        });
        return routes;
    }
}
