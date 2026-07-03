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
using cCoder.Eventing;
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
    public static IServiceCollection AddPackagingWeb(this IServiceCollection services)
    {
        AddPackaging(services, includePackageManagerServices: false);
        services.TryAddTransient<IAppDomainProvider, AppDomainProvider>();
        AddAspNet(services);
        AddApiDocumentation(services);

        IEdmModel routeModel = BuildRouteModel();
        DefaultODataBatchHandler batchHandler = new();

        services.AddControllers().AddOData(options =>
        {
            options.RouteOptions.EnableQualifiedOperationCall = false;
            options.EnableAttributeRouting = true;
            options.RouteOptions.EnableKeyAsSegment = false;
            options.Expand()
                .Count()
                .Filter()
                .Select()
                .OrderBy()
                .SetMaxTop(1000)
                .AddRouteComponents("Api/Packaging", routeModel, batchHandler)
                .AddRouteComponents("Api/Core", routeModel, batchHandler);
        });

        return services;
    }

    public static IServiceCollection AddPackaging(
        this IServiceCollection services,
        bool includeRouteContributor = true,
        bool includePackageManagerServices = true
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
        services.TryAddTransient<IPackageEventProcessingService, PackageEventProcessingService>();
        services.TryAddTransient<IPackageItemEventProcessingService, PackageItemEventProcessingService>();
        services.TryAddTransient<IPackageItemProcessingService, PackageItemProcessingService>();
        services.TryAddTransient<IPackageProcessingService, PackageProcessingService>();
        services.TryAddTransient<IPackageItemOrchestrationService, PackageItemOrchestrationService>();
        services.TryAddTransient<IPackageOrchestrationService, PackageOrchestrationService>();

        if (includePackageManagerServices)
        {
            services.TryAddTransient<IAppSecurityPackageService, AppSecurityPackageService>();
            services.TryAddTransient<IContentManagementPackageService, ContentManagementPackageService>();
            services.TryAddTransient<IDocumentManagementPackageService, DocumentManagementPackageService>();
            services.TryAddTransient<ISchedulingPackageService, SchedulingPackageService>();
            services.TryAddTransient<IWorkflowPackageService, WorkflowPackageService>();
            services.TryAddTransient<IPackageManagerOrchestrationService, PackageManagerOrchestrationService>();
        }

        return services;
    }

    private static void AddApiDocumentation(IServiceCollection services) =>
        services.AddSwaggerGen(options =>
        {
            options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            options.SwaggerDoc("Packaging", new OpenApiInfo
            {
                Title = "Packaging API definition",
                Version = "Packaging",
            });
            options.SwaggerDoc("Core", new OpenApiInfo
            {
                Title = "Core API definition",
                Version = "Core",
            });
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Core API definition",
                Version = "v1",
            });
            options.DocInclusionPredicate((documentName, apiDescription) =>
            {
                if (string.IsNullOrWhiteSpace(apiDescription.RelativePath))
                    return false;

                string normalizedDocument = string.Equals(documentName, "v1", StringComparison.OrdinalIgnoreCase)
                    ? "Core"
                    : documentName;
                string path = apiDescription.RelativePath.StartsWith('/')
                    ? apiDescription.RelativePath
                    : $"/{apiDescription.RelativePath}";

                return string.Equals(normalizedDocument, "Core", StringComparison.OrdinalIgnoreCase)
                    ? MatchesContextRoute(path, "Api/Core")
                    : MatchesContextRoute(path, "Api/Packaging");
            });
        });

    private static bool MatchesContextRoute(string path, string rootPath)
    {
        string normalizedRootPath = rootPath.StartsWith('/')
            ? rootPath
            : $"/{rootPath}";

        return path.Equals(normalizedRootPath, StringComparison.OrdinalIgnoreCase)
            || path.StartsWith($"{normalizedRootPath}/", StringComparison.OrdinalIgnoreCase);
    }

    private static IEdmModel BuildRouteModel()
    {
        ODataConventionModelBuilder builder = new();
        new PackagingModelBuilder(builder).Configure();

        return builder.GetEdmModel();
    }

    private static void AddAspNet(IServiceCollection services)
    {
        services.AddRouting();
        services.AddResponseCompression();
        services.AddHttpClient();
        services.AddHttpContextAccessor();
        services.AddScoped(
            typeof(HttpContext),
            serviceProvider => serviceProvider.GetService<IHttpContextAccessor>()?.HttpContext ?? new DefaultHttpContext());
        services.AddScoped(
            typeof(HttpRequest),
            serviceProvider => serviceProvider.GetRequiredService<HttpContext>().Request);
        services.AddSession();
        services.AddHsts(options =>
        {
            options.Preload = true;
            options.IncludeSubDomains = true;
            options.MaxAge = TimeSpan.FromMinutes(60);
        });
        services.AddMvc(options => options.EnableEndpointRouting = false);
        services.AddRazorPages();
        services.Configure<KestrelServerOptions>(options =>
        {
            options.Limits.MaxRequestBodySize = int.MaxValue;
        });
        services.AddEndpointsApiExplorer();
        services.AddSignalR();
    }
}


