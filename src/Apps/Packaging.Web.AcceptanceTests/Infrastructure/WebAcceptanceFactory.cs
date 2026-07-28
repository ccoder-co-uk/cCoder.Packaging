// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

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
                    key: "Packaging:ConnectionString",
                    value: configuration.PackagingConnectionString),
                new(
                    key: "Security:ConnectionString",
                    value: configuration.SecurityConnectionString),
                new(
                    key: "Security:DecryptionKey",
                    value: configuration.SecurityDecryptionKey)
            ]);
        });
    }
}