// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Eventing;
using cCoder.Packaging;
using cCoder.Security;
using cCoder.Security.Data.EF;

namespace Packaging.Web;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddPackagingWebApplication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string coreConnection = configuration.GetConnectionString(name: "Core")
            ?? throw new InvalidOperationException("ConnectionStrings:Core is required.");

        string ssoConnection = configuration.GetConnectionString(name: "SSO")
            ?? throw new InvalidOperationException("ConnectionStrings:SSO is required.");

        cCoder.Data.Config config = new();
        configuration.Bind(instance: config);
        services.AddSingleton(implementationInstance: config);
        services.AddEventing();

        services.AddSecurityApi(configAction: (securityServices, securityConfig) =>
        {
            securityConfig.AddMSSQLModelProvider(
                services: securityServices,
                connectionString: ssoConnection);

            securityConfig.UseAESHMMACPasswordEncryption(
                services: securityServices,
                decryptionKey: configuration.GetSection(key: "Settings")["DecryptionKey"]);
        });

        cCoder.Data.IServiceCollectionExtensions.AddCoreData(
            services: services,
            connectionString: coreConnection);

        services.AddPackagingWeb();

        return services;
    }
}