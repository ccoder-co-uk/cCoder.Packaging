// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Testing;
using cCoder.Packaging.Brokers;
using cCoder.Data.Models.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Packaging.Web.AcceptanceTests.Infrastructure;

internal sealed class WebAcceptanceFactory : WebApplicationFactory<Program>
{
    private readonly AcceptanceTestConfiguration configuration =
        AcceptanceTestConfiguration.Load();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment(environment: "Acceptance");

        builder.ConfigureAppConfiguration(configureDelegate: (_, appConfiguration) =>
        {
            appConfiguration.AddInMemoryCollection(initialData:
            [
                new(
                    key: "Eventing:ProviderType",
                    value: "InProcess"),
                new(
                    key: "CoreData:ConnectionString",
                    value: configuration.PackagingConnectionString),
                new(
                    key: "SecurityData:ConnectionString",
                    value: configuration.SecurityConnectionString),
                new(
                    key: "Security:DecryptionKey",
                    value: configuration.SecurityDecryptionKey)
            ]);
        });

        builder.ConfigureTestServices(servicesConfiguration: services =>
        {
            services.RemoveAll<IAuthorizationBroker>();
            services.AddSingleton<IAuthorizationBroker, AcceptanceAuthorizationBroker>();
        });
    }

    protected override void Dispose(bool disposing)
    {
        try
        {
            base.Dispose(disposing: disposing);
        }
        finally
        {
            if (disposing)
            {
                DropDatabase(
                    connectionString: configuration.PackagingConnectionString);

                DropDatabase(
                    connectionString: configuration.SecurityConnectionString);
            }
        }
    }

    private static void DropDatabase(string connectionString)
    {
        SqlConnectionStringBuilder builder = new(
            connectionString: connectionString);

        string databaseName = builder.InitialCatalog;

        if (!databaseName.Contains(
                value: "-acceptance-",
                comparisonType: StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                $"Refusing to drop non-acceptance database '{databaseName}'.");
        }

        builder.InitialCatalog = "master";

        using SqlConnection connection = new(
            connectionString: builder.ConnectionString);

        connection.Open();

        using SqlCommand command = connection.CreateCommand();

        command.CommandText = @"
IF DB_ID(@databaseName) IS NOT NULL
BEGIN
    DECLARE @sql nvarchar(max) =
        N'ALTER DATABASE [' + REPLACE(@databaseName, ']', ']]') + N'] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;'
        + N'DROP DATABASE [' + REPLACE(@databaseName, ']', ']]') + N']';
    EXEC(@sql);
END";

        _ = command.Parameters.AddWithValue(
            parameterName: "@databaseName",
            value: databaseName);

        command.ExecuteNonQuery();
    }

    private sealed class AcceptanceAuthorizationBroker : IAuthorizationBroker
    {
        public User GetCurrentUser() =>
            null;

        public bool IsAdminOfApp(int? appId) =>
            true;

        public bool IsAdmin(int appId, string userName) =>
            true;

        public void Authorize(int? appId, string privilege)
        { }
    }
}