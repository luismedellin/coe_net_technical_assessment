using TA_API.Core.Utils;

namespace TA_API.Endpoints;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/products", () =>
        {
            // TODO: create product migration, repository and service
            return Results.Ok(DataUtils.Products);
        });
        return routes;
    }
}
