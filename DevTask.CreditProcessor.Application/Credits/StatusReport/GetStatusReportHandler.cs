using DevTask.CreditProcessor.Application.Utils;
using DevTask.CreditProcessor.Domain.Abstractions;
using DevTask.CreditProcessor.Domain.Models;

using MediatR;

namespace DevTask.CreditProcessor.Application.Credits.StatusReport;
public record GetStatusReportHandler(ICreditRepository _creditRepository) : IRequestHandler<GetStatusReportQuery, StatusReportDto>
{
    public async Task<StatusReportDto> Handle(GetStatusReportQuery _, CancellationToken cancellationToken)
    {
        var totalAmounts = await _creditRepository.GetTotalAmountsByStatusAsync([CreditStatus.Paid, CreditStatus.AwaitingPayment], cancellationToken);

        var paidAmount = totalAmounts.SingleOrDefault(x => x.Status == CreditStatus.Paid)?.TotalAmount.ToPrecision2() ?? 0;
        var awaitingAmount = totalAmounts.SingleOrDefault(x => x.Status == CreditStatus.AwaitingPayment)?.TotalAmount.ToPrecision2() ?? 0;
        var totalAmount = paidAmount + awaitingAmount;

        return ToDto(paidAmount, awaitingAmount, totalAmount);
    }

    private StatusReportDto ToDto(decimal paidAmount, decimal awaitingAmount, decimal totalAmount) =>
        new StatusReportDto(
            Paid: new ReportItem(paidAmount, paidAmount.PercentOf(totalAmount)),
            AwatingPayment: new ReportItem(awaitingAmount, awaitingAmount.PercentOf(totalAmount)));
}
