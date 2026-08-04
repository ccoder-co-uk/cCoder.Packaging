// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Eventing;
using cCoder.Eventing.Models;
using cCoder.Packaging.Models;
using cCoder.Security;
using cCoder.Security.Models;
using Packaging.Web.Models;

namespace Packaging.Web;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddPackagingWeb(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<PackagingWebConfiguration> configure = null)
    {
        PackagingWebConfiguration packagingWebConfiguration = new()
        {
            Eventing = new EventingConfiguration(),
            Packaging = new PackagingConfiguration(),
            Security = new SecurityConfiguration()
        };
        configuration.Bind(instance: packagingWebConfiguration);
        configure?.Invoke(obj: packagingWebConfiguration);

        services.AddEventingWeb(
            configuration: packagingWebConfiguration.Eventing);
        services.AddSecurityWeb(
            configuration: packagingWebConfiguration.Security);
        cCoder.Packaging.IServiceCollectionExtensions.AddPackagingWeb(
            services: services,
            configuration: packagingWebConfiguration.Packaging);

        return services;
    }
}