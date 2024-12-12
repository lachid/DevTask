namespace DevTask.CreditProcessor.WebApi.Endpoints.Credits;

public static class CreditEndpoints
{
    public static void MapCreditEndpoints(this IEndpointRouteBuilder app)
    {
        var creditGroup = app.MapGroup("/credits");

        creditGroup.MapGet("", GetAll.Handle);
        creditGroup.MapGet("/status-report", StatusReport.Handle);
    }
}
