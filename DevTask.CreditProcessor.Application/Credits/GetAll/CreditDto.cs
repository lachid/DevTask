namespace DevTask.CreditProcessor.Application.Credits.GetAll;

public record CreditDto
{
    public int Id { get; init; }

    public required string Number { get; init; }

    public required string ClientName { get; init; }

    public required decimal RequestedAmount { get; init; }

    public required DateTime RequestDate { get; init; }

    public required string Status { get; init; }

    public required List<InvoiceDto> Invoices { get; init; } = [];
}
