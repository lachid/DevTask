using MediatR;

namespace DevTask.CreditProcessor.Application.Credits.GetAll;

public record GetAllCreditsQuery : IRequest<IEnumerable<CreditDto>>;
