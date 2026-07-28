// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace cCoder.Packaging.Testing;

internal sealed class AcceptanceTestConfiguration
{
    private AcceptanceTestConfiguration(
        string packagingConnectionString,
        string securityConnectionString,
        string securityDecryptionKey)
    {
        PackagingConnectionString = packagingConnectionString;
        SecurityConnectionString = securityConnectionString;
        SecurityDecryptionKey = securityDecryptionKey;
    }

    internal string PackagingConnectionString { get; }
    internal string SecurityConnectionString { get; }
    internal string SecurityDecryptionKey { get; }

    internal static AcceptanceTestConfiguration Load()
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(basePath: AppContext.BaseDirectory)
            .AddJsonFile(
                path: "appsettings.testing.json",
                optional: true)
            .AddEnvironmentVariables()
            .Build();

        string suffix = $"-acceptance-{Guid.NewGuid():N}";

        return new AcceptanceTestConfiguration(
            packagingConnectionString: AddDatabaseSuffix(
                connectionString: GetConfigurationValue(
                    configuration: configuration,
                    key: "Packaging:ConnectionString"),
                suffix: suffix),
            securityConnectionString: AddDatabaseSuffix(
                connectionString: GetConfigurationValue(
                    configuration: configuration,
                    key: "Security:ConnectionString"),
                suffix: suffix),
            securityDecryptionKey: GetConfigurationValue(
                configuration: configuration,
                key: "Security:DecryptionKey"));
    }

    private static string GetConfigurationValue(
        IConfiguration configuration,
        string key)
    {
        string environmentVariableName =
            key.Replace(oldValue: ":", newValue: "__");

        string value = configuration[key];

        if (!string.IsNullOrWhiteSpace(value: value))
        {
            return value;
        }

        return Environment.GetEnvironmentVariable(
                variable: environmentVariableName,
                target: EnvironmentVariableTarget.User)
            ?? Environment.GetEnvironmentVariable(
                variable: environmentVariableName,
                target: EnvironmentVariableTarget.Machine)
            ?? string.Empty;
    }

    private static string AddDatabaseSuffix(
        string connectionString,
        string suffix)
    {
        if (string.IsNullOrWhiteSpace(value: connectionString))
        {
            return string.Empty;
        }

        SqlConnectionStringBuilder builder =
            new(connectionString: connectionString)
            {
                Encrypt = true,
                TrustServerCertificate = true
            };

        if (!string.IsNullOrWhiteSpace(value: builder.InitialCatalog))
        {
            builder.InitialCatalog = $"{builder.InitialCatalog}{suffix}";
        }

        return builder.ConnectionString;
    }
}