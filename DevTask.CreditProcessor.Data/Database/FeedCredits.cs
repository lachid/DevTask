using Dapper;

using System.Data;

namespace DevTask.CreditProcessor.Data.Database;

internal static class FeedCredits
{
    internal static void FeedCreditsTable(this IDbConnection dbConnection)
    {
        var credits = CreditBuilder.New().Build(10);

        using var transaction = dbConnection.BeginTransaction();

        try
        {
            foreach (var credit in credits)
            {
                var creditRecord = $"('{credit.Number}', '{credit.ClientName}', {credit.RequestedAmount}, '{credit.RequestDate.ToString("yyyy-MM-dd HH:mm:ss.fff")}', {(int)credit.Status})";

                var insertCreditQuery = @$"
                    INSERT INTO Credits (Number, ClientName, RequestedAmount, RequestDate, Status)
                    VALUES {creditRecord};

                    SELECT last_insert_rowid();
                ";

                var creditId = dbConnection.ExecuteScalar<int>(insertCreditQuery, transaction: transaction);

                foreach (var invoice in credit.Invoices)
                {
                    var invoiceRecord = $"('{invoice.Number}', '{invoice.Amount}', {creditId})";

                    var insertInvoiceQuery = @$"
                        INSERT INTO Invoices (Number, Amount, CreditId)
                        VALUES {invoiceRecord};

                    ";

                    dbConnection.Execute(insertInvoiceQuery, transaction: transaction);
                }
            }

            transaction.Commit();
        }
        catch (Exception)
        {
            transaction.Rollback();
        }
    }
}
