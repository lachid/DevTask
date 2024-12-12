using DevTask.CreditProcessor.Domain.Models;

namespace DevTask.CreditProcessor.Domain.Abstractions;

public interface ICreditRepository
{
    Task<IEnumerable<Credit>> GetAllAsync(CancellationToken cancellationToken);

    Task<IEnumerable<StatusTotalAmount>> GetTotalAmountsByStatusAsync(IEnumerable<CreditStatus> statuses, CancellationToken cancellationToken);
}
