// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Eventing;
using cCoder.Security;
using Packaging.Web.Models;

namespace Packaging.Web;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddPackagingWeb(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<PackagingWebConfiguration> configure = null)
    {
        PackagingWebConfiguration packagingWebConfiguration = new();
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