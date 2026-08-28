// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data;
using cCoder.Eventing;
using cCoder.Security;
using cCoder.Security.Data.EF;
using Packaging.Web.Models;

namespace Packaging.Web;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddWeb(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<AppConfiguration> configure = null)
    {
        AppConfiguration appConfiguration = new();
        configuration.Bind(instance: appConfiguration);
        configure?.Invoke(obj: appConfiguration);

        services.AddData(configuration: appConfiguration.CoreData);
        services.AddEventingWeb(
            configuration: appConfiguration.Eventing);
        services.AddSecurityData(configuration: appConfiguration.SecurityData);
        services.AddSecurityWeb(
            configuration: appConfiguration.Security);
        cCoder.Packaging.IServiceCollectionExtensions.AddPackagingWeb(
            services: services,
            configuration: appConfiguration.Packaging);

        return services;
    }
}