namespace DevTask.CreditProcessor.Application.Credits.GetAll;

public record InvoiceDto
{
    public required string Number { get; init; }

    public required decimal Amount { get; init; }
}
