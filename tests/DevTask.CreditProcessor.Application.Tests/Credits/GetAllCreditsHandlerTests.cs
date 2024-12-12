using DevTask.CreditProcessor.Application.Credits.GetAll;
using DevTask.CreditProcessor.Application.Utils;
using DevTask.CreditProcessor.Data.Database;
using DevTask.CreditProcessor.Domain.Abstractions;

using FakeItEasy;

using FluentAssertions;

namespace DevTask.CreditProcessor.Application.Tests.Credits;

public class GetAllCreditsHandlerTests
{
    private readonly GetAllCreditsHandler _sut;
    private readonly ICreditRepository _creditRepository = A.Fake<ICreditRepository>();

    public GetAllCreditsHandlerTests() => _sut = new GetAllCreditsHandler(_creditRepository);

    [Fact]
    public async Task Handle_Always_ReturnsCreditsWithInvoicesAsync()
    {
        // Arrange
        var credit = CreditBuilder.New()
            .WithInvoicesCount(1)
            .Build();

        A.CallTo(() => _creditRepository.GetAllAsync(A<CancellationToken>._))
            .Returns([credit]);

        // Act
        var result = await _sut.Handle(new GetAllCreditsQuery(), CancellationToken.None);

        // Assert
        result.Should().HaveCount(1);

        var creditDto = result.First();
        creditDto.Number.Should().Be(credit.Number);
        creditDto.ClientName.Should().Be(credit.ClientName);
        creditDto.RequestedAmount.Should().Be(credit.RequestedAmount.ToPrecision2());
        creditDto.RequestDate.Should().Be(credit.RequestDate);
        creditDto.Status.Should().Be(credit.Status.ToString());

        creditDto.Invoices.Should().HaveCount(1);
        var invoice = creditDto.Invoices.First();
        invoice.Number.Should().Be(credit.Invoices[0].Number);
        invoice.Amount.Should().Be(credit.Invoices[0].Amount.ToPrecision2());
    }
}
