namespace TA_API.Endpoints;

public static class HealthEndpoints
{
    public static IEndpointRouteBuilder MapHealthEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/", () => "Technical Assessment API");
        routes.MapGet("/lbhealth", () => "Technical Assessment API");

        return routes;
    }
}
