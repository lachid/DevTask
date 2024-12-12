using DevTask.CreditProcessor.Application.Credits.StatusReport;
using DevTask.CreditProcessor.Domain.Abstractions;
using DevTask.CreditProcessor.Domain.Models;

using FakeItEasy;

using FluentAssertions;
using System.Diagnostics.CodeAnalysis;

namespace DevTask.CreditProcessor.Application.Tests.Credits;

public class GetStatusReportHandlerTests
{
    private readonly GetStatusReportHandler _sut;
    private readonly ICreditRepository _creditRepository = A.Fake<ICreditRepository>();

    public GetStatusReportHandlerTests() => _sut = new GetStatusReportHandler(_creditRepository);

    [Theory]
    [InlineData(null, 100)]
    [InlineData(0, 100)]
    public async Task Handle_NoPaidCredits_ReturnReportWithPaid0AndTotalIsAwaitingPaymentAsync(int? paidAmountParam, int awaitingPaymentAmountParam)
    {
        // Arrange
        var paidAmount = (decimal?)paidAmountParam;
        var awaitingPaymentAmount = (decimal)awaitingPaymentAmountParam;

        IEnumerable<StatusTotalAmount> amountsByStatus =
        [
            new StatusTotalAmount { Status = CreditStatus.AwaitingPayment, TotalAmount = awaitingPaymentAmount }
        ];

        if (paidAmount.HasValue)
            amountsByStatus.Append(new StatusTotalAmount { Status = CreditStatus.Paid, TotalAmount = paidAmount.Value });

        A.CallTo(() => _creditRepository.GetTotalAmountsByStatusAsync(
                A<IEnumerable<CreditStatus>>.That.Matches(
                    s => s.Count() == 2 &&
                    s.Contains(CreditStatus.Paid)
                    && s.Contains(CreditStatus.AwaitingPayment)),
                A<CancellationToken>._))
            .Returns(amountsByStatus);

        // Act
        var result = await _sut.Handle(new GetStatusReportQuery(), CancellationToken.None);


        // Assert
        result.Paid.Amount.Should().Be(0);
        result.Paid.Percent.Should().Be(0);

        result.AwatingPayment.Amount.Should().Be(awaitingPaymentAmount);
        result.AwatingPayment.Percent.Should().Be(100);
    }

    [Theory]
    [InlineData(100, null)]
    [InlineData(100, 0)]
    public async Task Handle_NoAwaitingPaymentCredits_ReturnReportWithAwaitingPayment0AndTotalIsPaidAsync(int paidAmountParam, int? awaitingPaymentAmountParam)
    {
        // Arrange
        var paidAmount = (decimal)paidAmountParam;
        var awaitingPaymentAmount = (decimal?)awaitingPaymentAmountParam;

        IEnumerable<StatusTotalAmount> amountsByStatus =
        [
            new StatusTotalAmount { Status = CreditStatus.Paid, TotalAmount = paidAmount }
        ];

        if (awaitingPaymentAmount.HasValue)
            amountsByStatus.Append(new StatusTotalAmount { Status = CreditStatus.AwaitingPayment, TotalAmount = awaitingPaymentAmount.Value });

        A.CallTo(() => _creditRepository.GetTotalAmountsByStatusAsync(
                A<IEnumerable<CreditStatus>>.That.Matches(
                    s => s.Count() == 2 &&
                    s.Contains(CreditStatus.Paid)
                    && s.Contains(CreditStatus.AwaitingPayment)),
                A<CancellationToken>._))
            .Returns(amountsByStatus);

        // Act
        var result = await _sut.Handle(new GetStatusReportQuery(), CancellationToken.None);


        // Assert
        result.AwatingPayment.Amount.Should().Be(0);
        result.AwatingPayment.Percent.Should().Be(0);

        result.Paid.Amount.Should().Be(paidAmount);
        result.Paid.Percent.Should().Be(100);
    }

    [Theory]
    [InlineData(50, 50, 50, 50)]
    [InlineData(60, 40, 60, 40)]
    [InlineData(50, 150, 25, 75)]
    public async Task Handle_AwaitingPaymentAndPaidCreditsExist_ReturnReportWithCalculatedTotalsAsync(
        int paidAmountParam, int awaitingPaymentAmountParam, int expectedPaidAmountPercentParam, int expectedAwaitingPaymentAmountPercentParam)
    {
        // Arrange
        var paidAmount = (decimal)paidAmountParam;
        var awaitingPaymentAmount = (decimal)awaitingPaymentAmountParam;
        var expectedPaidAmountPercent = (decimal)expectedPaidAmountPercentParam;
        var expectedAwaitingPaymentAmountPercent = (decimal) expectedAwaitingPaymentAmountPercentParam;

        IEnumerable<StatusTotalAmount> amountsByStatus =
        [
            new StatusTotalAmount { Status = CreditStatus.Paid, TotalAmount = paidAmount },
            new StatusTotalAmount { Status = CreditStatus.AwaitingPayment, TotalAmount = awaitingPaymentAmount }
        ];

        A.CallTo(() => _creditRepository.GetTotalAmountsByStatusAsync(
                A<IEnumerable<CreditStatus>>.That.Matches(
                    s => s.Count() == 2 &&
                    s.Contains(CreditStatus.Paid)
                    && s.Contains(CreditStatus.AwaitingPayment)),
                A<CancellationToken>._))
            .Returns(amountsByStatus);

        // Act
        var result = await _sut.Handle(new GetStatusReportQuery(), CancellationToken.None);


        // Assert
        result.Paid.Amount.Should().Be(paidAmount);
        result.Paid.Percent.Should().Be(expectedPaidAmountPercent);

        result.AwatingPayment.Amount.Should().Be(awaitingPaymentAmount);
        result.AwatingPayment.Percent.Should().Be(expectedAwaitingPaymentAmountPercent);
    }
}
