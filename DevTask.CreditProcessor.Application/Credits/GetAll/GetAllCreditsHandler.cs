using DevTask.CreditProcessor.Domain.Abstractions;

using MediatR;

using static DevTask.CreditProcessor.Application.Credits.GetAll.Mapper;

namespace DevTask.CreditProcessor.Application.Credits.GetAll;

public class GetAllCreditsHandler(ICreditRepository _creditRepository) : IRequestHandler<GetAllCreditsQuery, IEnumerable<CreditDto>>
{
    public async Task<IEnumerable<CreditDto>> Handle(GetAllCreditsQuery _, CancellationToken cancellationToken)=>
        Map(await _creditRepository.GetAllAsync(cancellationToken));
}
