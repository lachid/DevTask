using Dapper;

using DevTask.CreditProcessor.Domain.Abstractions;
using DevTask.CreditProcessor.Domain.Models;

using System.Data;

namespace DevTask.CreditProcessor.Data.Repositories;

public class CreditRepository(IDbConnection _connection) : ICreditRepository
{
    public async Task<IEnumerable<Credit>> GetAllAsync(CancellationToken cancellationToken)
    {
        var credits = await _connection.QueryAsync(
            new CommandDefinition(GetAllQuery, cancellationToken: cancellationToken),
            map: CreditWithInvoicesMapper);

        return credits.Combine();
    }

    public async Task<IEnumerable<StatusTotalAmount>> GetTotalAmountsByStatusAsync(IEnumerable<CreditStatus> statuses, CancellationToken cancellationToken)
    {
        return await _connection.QueryAsync<StatusTotalAmount>(
            new CommandDefinition(TotalAmountsByStatus, parameters: new { Statuses = statuses }, cancellationToken: cancellationToken));
    }

    #region Queries

    private const string GetAllQuery = @"
        Select * From Credits c
        Join Invoices i On i.CreditId = c.Id
    ";

    private const string TotalAmountsByStatus = @"
        Select c.Status, Sum(RequestedAmount) As TotalAmount From Credits c
        Where c.Status In @Statuses
        Group By c.Status
    ";

    #endregion

    #region Mappers

    Func<Credit, Invoice, Credit> CreditWithInvoicesMapper = (credit, invoice) =>
    {
        credit.Invoices.Add(invoice);
        return credit;
    };

    #endregion
}


#region Extensions

internal static class Extensions
{
    public static IEnumerable<Credit> Combine(this IEnumerable<Credit> credits) =>
        credits.GroupBy(c => c.Id)
            .Select(group =>
            {
                var credit = group.First();
                credit.Invoices = group.SelectMany(g => g.Invoices).ToList();

                return credit;
            });
}

#endregion
