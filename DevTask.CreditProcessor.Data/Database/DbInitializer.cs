using Dapper;

using DevTask.CreditProcessor.Data.Database;
using DevTask.CreditProcessor.Domain.Models;

using Microsoft.Extensions.DependencyInjection;

using System.Data;

namespace Microsoft.AspNetCore.Builder;

public static class DbInitializer
{
    public static void InitializeDatabase(this WebApplication app, bool feed)
    {
        // Keep this scope and connection alive during the app lifecycle to have access to the in memmory DB
        var scope = app.Services.CreateScope();

        var dbConnection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
        dbConnection.Open();

        dbConnection.CreateTables();

        if (feed)
        {
            dbConnection.FeedCreditsTable();
        }

        #region Clean up
        app.Lifetime.ApplicationStopping.Register(() =>
        {
            dbConnection.Close();
            scope.Dispose();
        });
        #endregion
    }

    private static void CreateTables(this IDbConnection dbConnection)
    {
        var statuses = Enum.GetValues<CreditStatus>()
            .Select(status => $"({(int)status}, '{status.ToString()}')");

        var values = string.Join(",", statuses);

        var createTablesQuery = @$"
            CREATE TABLE IF NOT EXISTS CreditStatusLookup (
                Id INTEGER PRIMARY KEY,
                Name TEXT NOT NULL
            );

            INSERT INTO CreditStatusLookup (Id, Name) VALUES {values};

            CREATE TABLE IF NOT EXISTS Credits (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Number TEXT NOT NULL,
                ClientName TEXT NOT NULL,
                RequestedAmount DECIMAL(10,5) NOT NULL,
                RequestDate DATE NOT NULL,
                Status INTEGER NOT NULL,

                FOREIGN KEY (Status) REFERENCES CreditStatusLookup(Id)
            );

            CREATE TABLE IF NOT EXISTS Invoices (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Number TEXT NOT NULL,
                Amount DECIMAL(10,5) NOT NULL,
                CreditId INTEGER NOT NULL,

                FOREIGN KEY (CreditId) REFERENCES Credits(Id)
            );
        ";

        dbConnection.Execute(createTablesQuery);
    }
}
