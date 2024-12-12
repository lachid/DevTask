namespace DevTask.CreditProcessor.Domain.Models;

public class Invoice
{
    public int Id { get; set; }

    public required string Number { get; set; }

    public required decimal Amount { get; set; }
}
