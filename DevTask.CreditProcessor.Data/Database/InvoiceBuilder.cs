using Bogus;

using DevTask.CreditProcessor.Domain.Models;

namespace DevTask.CreditProcessor.Data.Database;

internal class InvoiceBuilder
{
    private readonly Faker<Invoice> _faker;

    private InvoiceBuilder()
    {
        _faker = new Faker<Invoice>()
            .RuleFor(p => p.Number, (f, _) => f.UniqueIndex.ToString())
            .RuleFor(p => p.Amount, (f, _) => f.Random.Decimal(1, 5000));
    }

    public static InvoiceBuilder New() => new InvoiceBuilder();

    public Invoice Build() => _faker.Generate();

    public List<Invoice> Build(int count) => _faker.Generate(count);
}
