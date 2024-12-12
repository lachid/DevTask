using Bogus;

using DevTask.CreditProcessor.Domain.Models;

using System.Text.RegularExpressions;

namespace DevTask.CreditProcessor.Data.Database;

internal class CreditBuilder
{
    private const string NonAlfanumericChars = @"[^a-zA-Z0-9\s]";

    private readonly Faker<Credit> _faker;

    private CreditBuilder()
    {
        _faker = new Faker<Credit>()
            .RuleFor(p => p.Number, (f, _) => f.UniqueIndex.ToString())
            .RuleFor(p => p.ClientName, (f, _) => Sanitize(f.Name.FullName()))
            .RuleFor(p => p.RequestedAmount, (f, _) => f.Random.Decimal(1, 5000))
            .RuleFor(p => p.RequestDate, (f, _) => f.Date.Between(new DateTime(2024, 12, 1), DateTime.Now))
            .RuleFor(p => p.Status, (f, _) => f.Random.Enum<CreditStatus>())
            .RuleFor(p => p.Invoices, (f, _) => BuildInvoices(count: f.Random.Int(1, 3)));
    }

    public static CreditBuilder New() => new CreditBuilder();

    public CreditBuilder WithInvoicesCount(int count)
    {
        _faker.RuleFor(p => p.Invoices, (f, _) => BuildInvoices(count));

        return this;
    }

    public Credit Build() => _faker.Generate();

    public List<Credit> Build(int count) => _faker.Generate(count);

    private List<Invoice> BuildInvoices(int count) => InvoiceBuilder.New().Build(count);

    private static string Sanitize(string input) => Regex.Replace(input, NonAlfanumericChars, string.Empty);
}
