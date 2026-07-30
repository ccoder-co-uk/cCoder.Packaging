// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data;
using cCoder.Data.Models.Packaging;
using cCoder.Eventing;
using cCoder.Packaging.Brokers;
using cCoder.Packaging.Brokers.Events;
using cCoder.Packaging.Brokers.Metadata;
using cCoder.Packaging.Brokers.OData;
using cCoder.Packaging.Brokers.Storages;
using cCoder.Packaging.Exposures;
using cCoder.Packaging.Exposures.PackageManagers;
using cCoder.Packaging.Models;
using cCoder.Packaging.Services.Aggregations;
using cCoder.Packaging.Services.Foundations;
using cCoder.Packaging.Services.Foundations.Events;
using cCoder.Packaging.Services.Foundations.Metadata;
using cCoder.Packaging.Services.Foundations.PackageExports;
using cCoder.Packaging.Services.Foundations.PackageManagers;
using cCoder.Packaging.Services.Foundations.Storages;
using cCoder.Packaging.Services.Orchestrations;
using cCoder.Packaging.Services.Processings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Batch;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi;

namespace cCoder.Packaging;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddPackagingWeb(
        this IServiceCollection services,
        Action<PackagingConfiguration> configure)
    {
        PackagingConfiguration configuration = new();
        configure?.Invoke(obj: configuration);

        return services.AddPackagingWeb(configuration: configuration);
    }

    public static IServiceCollection AddPackagingWeb(
        this IServiceCollection services,
        PackagingConfiguration configuration)
    {
        services.AddConfiguration(configuration: configuration);
        services.AddBrokers(includePackageManagerServices: false);
        services.AddFoundations(includePackageManagerServices: false);
        services.AddProcessings();
        services.AddOrchestrations();
        services.AddAggregations(includePackageManagerServices: false);
        services.AddExposures(includeRouteContributor: false);
        services.AddWebExposures(configuration: configuration);

        return services;
    }

    public static IServiceCollection AddPackaging(
        this IServiceCollection services,
        Action<PackagingConfiguration> configure)
    {
        PackagingConfiguration configuration = new();
        configure?.Invoke(obj: configuration);

        return services.AddPackaging(configuration: configuration);
    }

    public static IServiceCollection AddPackaging(
        this IServiceCollection services,
        PackagingConfiguration configuration)
    {
        services.AddConfiguration(configuration: configuration);
        services.AddBrokers(includePackageManagerServices: true);
        services.AddFoundations(includePackageManagerServices: true);
        services.AddProcessings();
        services.AddOrchestrations();
        services.AddAggregations(includePackageManagerServices: true);
        services.AddExposures(includeRouteContributor: true);

        return services;
    }

    private static void AddConfiguration(
        this IServiceCollection services,
        PackagingConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(argument: configuration);
        services.TryAddSingleton(instance: configuration);

        services.AddData(
            configuration: new cCoder.Data.Models.DataConfiguration
            {
                ConnectionString = configuration.ConnectionString
            });
    }

    private static void AddBrokers(
        this IServiceCollection services,
        bool includePackageManagerServices)
    {
        services.TryAddTransient<IAuthorizationBroker, AuthorizationBroker>();
        services.TryAddTransient<IAuthInfoBroker, AuthInfoBroker>();
        services.TryAddTransient<IPackageEventBroker, PackageEventBroker>();
        services.TryAddTransient<IPackageItemEventBroker, PackageItemEventBroker>();
        services.TryAddTransient<IPackageBroker, PackageBroker>();
        services.TryAddTransient<IPackageItemBroker, PackageItemBroker>();
        services.TryAddTransient<IMetadataBroker, MetadataBroker>();

        if (includePackageManagerServices)
        {
            services.TryAddTransient<IPackageLoggerBroker, PackageLoggerBroker>();
        }
    }

    private static void AddFoundations(
        this IServiceCollection services,
        bool includePackageManagerServices)
    {
        services.TryAddTransient<IPackageEventService, PackageEventService>();
        services.TryAddTransient<IPackageItemEventService, PackageItemEventService>();
        services.TryAddTransient<IPackagingMetadataTypeService, PackagingMetadataTypeService>();
        services.TryAddTransient<IPackageService, PackageService>();
        services.TryAddTransient<IPackageItemService, PackageItemService>();
        services.TryAddTransient<IPackageExportService, PackageExportService>();
        services.TryAddTransient<IMetadataService, MetadataService>();

        if (includePackageManagerServices)
        {
            services.TryAddTransient<IAppSecurityPackageService, AppSecurityPackageService>();
            services.TryAddTransient<IContentManagementPackageService, ContentManagementPackageService>();
            services.TryAddTransient<IDocumentManagementPackageService, DocumentManagementPackageService>();
            services.TryAddTransient<ISchedulingPackageService, SchedulingPackageService>();
            services.TryAddTransient<IWorkflowPackageService, WorkflowPackageService>();
            services.TryAddTransient<IPackageManagerTelemetryService, PackageManagerTelemetryService>();
        }
    }

    private static void AddProcessings(
        this IServiceCollection services)
    {
        services.TryAddTransient<IPackageEventProcessingService, PackageEventProcessingService>();
        services.TryAddTransient<IPackageItemEventProcessingService, PackageItemEventProcessingService>();
        services.TryAddTransient<IPackageItemProcessingService, PackageItemProcessingService>();
        services.TryAddTransient<IPackageProcessingService, PackageProcessingService>();
        services.TryAddTransient<IPackageExportProcessingService, PackageExportProcessingService>();
    }

    private static void AddOrchestrations(
        this IServiceCollection services) =>
        services.TryAddTransient<
            IPackageItemOrchestrationService,
            PackageItemOrchestrationService>();

    private static void AddAggregations(
        this IServiceCollection services,
        bool includePackageManagerServices)
    {
        services.TryAddTransient<IPackageAggregationService, PackageAggregationService>();

        if (includePackageManagerServices)
        {
            services.TryAddTransient<
                IPackageManagerAggregationService,
                PackageManagerAggregationService>();
        }
    }

    private static void AddExposures(
        this IServiceCollection services,
        bool includeRouteContributor)
    {
        services.AddEventingForType<Package>();
        services.AddEventingForType<PackageItem>();
        services.AddEventingForType<(int, Package)>();
        services.TryAddTransient<IPackageManager, PackageManager>();
        services.TryAddTransient<
            IPackageTransferManager,
            PackageTransferManager>();
        services.TryAddTransient<IPackageItemManager, PackageItemManager>();
        services.TryAddTransient<
            IPackageMetadataManager,
            PackageMetadataManager>();

        if (includeRouteContributor)
        {
            services.AddSingleton<Action<ODataConventionModelBuilder>>(
                implementationInstance: builder =>
                    new ODataModelBroker().ConfigureODataModel(
                        builder: builder));
        }
    }

    private static void AddWebExposures(
        this IServiceCollection services,
        PackagingConfiguration configuration)
    {
        services.TryAddTransient<IAppDomainManager, AppDomainManager>();
        services.AddAspNet();
        services.AddApiDocumentation(configuration: configuration);

        ODataConventionModelBuilder modelBuilder = new();
        new ODataModelBroker().ConfigureODataModel(builder: modelBuilder);
        IEdmModel routeModel = modelBuilder.GetEdmModel();
        DefaultODataBatchHandler batchHandler = new();
        IMvcBuilder mvcBuilder = services.AddControllers();

        mvcBuilder.AddOData(setupAction: options =>
        {
            options.RouteOptions.EnableQualifiedOperationCall = false;
            options.EnableAttributeRouting = true;
            options.RouteOptions.EnableKeyAsSegment = false;

            options.Expand()
                .Count()
                .Filter()
                .Select()
                .OrderBy()
                .SetMaxTop(maxTopValue: 1000)
                .AddRouteComponents(
                    routePrefix: configuration.RootPath,
                    model: routeModel,
                    batchHandler: batchHandler);
        });
    }

    private static void AddApiDocumentation(
        this IServiceCollection services,
        PackagingConfiguration configuration) =>
        services.AddSwaggerGen(setupAction: options =>
        {
            options.ResolveConflictingActions(
                resolver: apiDescriptions => apiDescriptions.First());

            options.SwaggerDoc(name: "Packaging", info: new OpenApiInfo
            {
                Title = "Packaging API definition",
                Version = "Packaging"
            });

            options.DocInclusionPredicate(
                predicate: (documentName, apiDescription) =>
                {
                    if (string.IsNullOrWhiteSpace(
                        value: apiDescription.RelativePath))
                    {
                        return false;
                    }

                    string path =
                        apiDescription.RelativePath.StartsWith(value: '/')
                            ? apiDescription.RelativePath
                            : $"/{apiDescription.RelativePath}";

                    string rootPath =
                        configuration.RootPath.StartsWith(value: '/')
                            ? configuration.RootPath
                            : $"/{configuration.RootPath}";

                    return string.Equals(
                            a: documentName,
                            b: "Packaging",
                            comparisonType:
                                StringComparison.OrdinalIgnoreCase)
                        && (path.Equals(
                            value: rootPath,
                            comparisonType: StringComparison.OrdinalIgnoreCase)
                        || path.StartsWith(
                            value: $"{rootPath}/",
                            comparisonType: StringComparison.OrdinalIgnoreCase));
                });
        });

    private static void AddAspNet(
        this IServiceCollection services)
    {
        services.AddRouting();
        services.AddResponseCompression();
        services.AddHttpClient();
        services.AddHttpContextAccessor();

        services.AddScoped(
            serviceType: typeof(HttpContext),
            implementationFactory: serviceProvider =>
                serviceProvider.GetService<IHttpContextAccessor>()?.HttpContext
                ?? new DefaultHttpContext());

        services.AddScoped(
            serviceType: typeof(HttpRequest),
            implementationFactory: serviceProvider =>
                serviceProvider.GetRequiredService<HttpContext>().Request);

        services.AddSession();

        services.AddHsts(configureOptions: options =>
        {
            options.Preload = true;
            options.IncludeSubDomains = true;
            options.MaxAge = TimeSpan.FromMinutes(minutes: 60);
        });

        services.AddMvc(
            setupAction: options => options.EnableEndpointRouting = false);
        services.AddRazorPages();
        services.Configure<KestrelServerOptions>(configureOptions: options =>
        {
            options.Limits.MaxRequestBodySize = int.MaxValue;
        });
        services.AddEndpointsApiExplorer();
        services.AddSignalR();
    }
}