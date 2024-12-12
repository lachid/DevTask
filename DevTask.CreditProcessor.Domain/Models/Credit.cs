namespace DevTask.CreditProcessor.Domain.Models;

public class Credit
{
    public int Id { get; set; }

    public required string Number { get; set; }

    public required string ClientName { get; set; }

    public required decimal RequestedAmount { get; set; }

    public required DateTime RequestDate { get; set; }

    public required CreditStatus Status { get; set; }

    public required List<Invoice> Invoices { get; set; } = [];
}
