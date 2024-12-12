using DevTask.CreditProcessor.Application.Credits.GetAll;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace DevTask.CreditProcessor.WebApi.Endpoints.Credits;

public static class GetAll
{
    public static async Task<IResult> Handle(
        [FromServices] ISender dispatcher,
        CancellationToken cancellationToken) => Results.Ok(await dispatcher.Send(new GetAllCreditsQuery(), cancellationToken));
}
