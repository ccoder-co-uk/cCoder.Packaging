// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Testing;
using cCoder.Packaging.Brokers;
using cCoder.Data.Models.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
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