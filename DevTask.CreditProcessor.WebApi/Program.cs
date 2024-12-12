using DevTask.CreditProcessor.WebApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("SQLite") ?? throw new ArgumentException("No connection string found.");

builder.Services.AddDataAccess(connectionString);
builder.Services.AddApplicationServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapEndpoints();

var feedDb = app.Environment.IsDevelopment();
app.InitializeDatabase(feedDb);

try
{
    app.Run();
}
catch (Exception ex)
{
    app.Logger.LogCritical(ex, "Application startup error.");
    Environment.Exit(-1);
}