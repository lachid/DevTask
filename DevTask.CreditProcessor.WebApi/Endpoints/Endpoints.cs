using DevTask.CreditProcessor.WebApi.Endpoints.Credits;

namespace DevTask.CreditProcessor.WebApi.Endpoints;

public static class Endpoints
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGroup("/api")
            .MapCreditEndpoints();
    }
}
