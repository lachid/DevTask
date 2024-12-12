using DevTask.CreditProcessor.Application.Credits.StatusReport;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace DevTask.CreditProcessor.WebApi.Endpoints.Credits;

public static class StatusReport
{
    public static async Task<IResult> Handle(
        [FromServices] ISender dispatcher,
        CancellationToken cancellationToken) => Results.Ok(await dispatcher.Send(new GetStatusReportQuery(), cancellationToken));
}
