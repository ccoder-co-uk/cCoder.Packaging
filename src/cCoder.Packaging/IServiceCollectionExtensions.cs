using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Api.OData;
using cCoder.Packaging.Brokers;
using cCoder.Packaging.Brokers.Events;
using cCoder.Packaging.Brokers.Storages;
using cCoder.Packaging.Services;
using cCoder.Packaging.Services.Foundations.Events;
using cCoder.Packaging.Services.Foundations;
using cCoder.Packaging.Services.Foundations.Storages;
using cCoder.Packaging.Services.Orchestrations;
using cCoder.Packaging.Services.Processings;
using EventLibrary;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OData.ModelBuilder;


namespace cCoder.Packaging;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddPackaging(
        this IServiceCollection services,
        bool includeRouteContributor = true
    )
    {
        if (includeRouteContributor)
        {
            services.AddSingleton<Action<ODataConventionModelBuilder>>(
                builder => new PackagingModelBuilder(builder).Configure());
        }

        services.AddEventingForType<Package>();
        services.AddEventingForType<PackageItem>();
        services.AddEventingForType<(int, Package)>();
        services.TryAddTransient<IAuthorizationBroker, AuthorizationBroker>();
        services.TryAddTransient<IPackageEventBroker, PackageEventBroker>();
        services.TryAddTransient<IPackageItemEventBroker, PackageItemEventBroker>();
        services.TryAddTransient<IPackageBroker, PackageBroker>();
        services.TryAddTransient<IPackageItemBroker, PackageItemBroker>();
        services.TryAddTransient<IPackageEventService, PackageEventService>();
        services.TryAddTransient<IPackageItemEventService, PackageItemEventService>();
        services.TryAddTransient<IPackagingMetadataTypeService, PackagingMetadataTypeService>();
        services.TryAddTransient<IPackageService, PackageService>();
        services.TryAddTransient<IPackageItemService, PackageItemService>();
        services.TryAddTransient<IAppSecurityPackageService, AppSecurityPackageService>();
        services.TryAddTransient<IContentManagementPackageService, ContentManagementPackageService>();
        services.TryAddTransient<IDocumentManagementPackageService, DocumentManagementPackageService>();
        services.TryAddTransient<ISchedulingPackageService, SchedulingPackageService>();
        services.TryAddTransient<IWorkflowPackageService, WorkflowPackageService>();
        services.TryAddTransient<IPackageEventProcessingService, PackageEventProcessingService>();
        services.TryAddTransient<IPackageItemEventProcessingService, PackageItemEventProcessingService>();
        services.TryAddTransient<IPackageItemProcessingService, PackageItemProcessingService>();
        services.TryAddTransient<IPackageProcessingService, PackageProcessingService>();
        services.TryAddTransient<IPackageItemOrchestrationService, PackageItemOrchestrationService>();
        services.TryAddTransient<IPackageOrchestrationService, PackageOrchestrationService>();
        services.TryAddTransient<IPackageManagerOrchestrationService, PackageManagerOrchestrationService>();

        return services;
    }
}


