using DevTask.CreditProcessor.Application.Utils;
using DevTask.CreditProcessor.Domain.Models;

namespace DevTask.CreditProcessor.Application.Credits.GetAll;

internal static class Mapper
{
    internal static IEnumerable<CreditDto> Map(IEnumerable<Credit> credits) => credits.Select(Map);

    internal static CreditDto Map(Credit credit) => new CreditDto
    {
        Id = credit.Id,
        Number = credit.Number,
        ClientName = credit.ClientName,
        RequestedAmount = credit.RequestedAmount.ToPrecision2(),
        RequestDate = credit.RequestDate,
        Status = credit.Status.ToString(),
        Invoices = credit.Invoices.Select(Map).ToList()
    };

    internal static InvoiceDto Map(Invoice invoice) => new InvoiceDto
    {
        Number = invoice.Number,
        Amount = invoice.Amount.ToPrecision2()
    };
}
