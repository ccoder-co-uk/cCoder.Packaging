// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Services.Foundations.PackageManagers;

namespace cCoder.Packaging.Services.Aggregations;

internal sealed partial class PackageManagerAggregationService(
    IPackageManagerTelemetryService packageManagerTelemetryService,
    IAppSecurityPackageService appSecurityPackageService,
    ISchedulingPackageService schedulingPackageService,
    IWorkflowPackageService workflowPackageService,
    IDocumentManagementPackageService documentManagementPackageService,
    IContentManagementPackageService contentManagementPackageService)
    : IPackageManagerAggregationService
{
    public ValueTask ImportPackageAsync(int appId, Package package) =>
        TryCatch(operation: async () =>
        {
            ValidatePackageOnImport(appId: appId, package: package);

            if (package.Items is null || package.Items.Count == 0)
            {
                return;
            }

            packageManagerTelemetryService.EnsurePackageAdmin(appId: appId);

            foreach (PackageItem packageItem in package.Items)
            {
                packageManagerTelemetryService.LogPackageItemImport(
                    packageItem: packageItem,
                    packageSource: package.SourceApi ?? string.Empty);

                await ImportPackageItemAsync(
                    appId: appId,
                    packageName: package.Name,
                    packageItem: packageItem);
            }
        });

    public Package ExportPackage(int appId, string packageName) =>
        TryCatch(operation: () =>
        {
            ValidatePackageOnExport(appId: appId, packageName: packageName);
            packageManagerTelemetryService.EnsurePackageAdmin(appId: appId);

            return packageName switch
            {
                "Calendars" or "CalendarEvents" =>
                    schedulingPackageService.ExportPackage(
                        appId: appId,
                        packageName: packageName),
                "Workflows" =>
                    workflowPackageService.ExportPackage(
                        appId: appId,
                        packageName: packageName),
                "FolderRoles" =>
                    documentManagementPackageService.ExportPackage(
                        appId: appId,
                        packageName: packageName),
                "Roles" =>
                    appSecurityPackageService.ExportPackage(
                        appId: appId,
                        packageName: packageName),
                _ =>
                    contentManagementPackageService.ExportPackage(
                        appId: appId,
                        packageName: packageName),
            };
        });

    private async ValueTask ImportPackageItemAsync(
        int appId,
        string packageName,
        PackageItem packageItem)
    {
        PackageItem canonicalPackageItem = ToCanonicalDomainPackageItem(
            packageItem: packageItem);

        if (packageItem.Type is
            "Workflow/Calendar"
            or "Workflow/CalendarEvent"
            or "Core/Calendar"
            or "Core/CalendarEvent")
        {
            Package planningPackage = new("Planning")
            {
                Items = [canonicalPackageItem],
            };

            await schedulingPackageService.ImportPackageAsync(
                appId: appId,
                package: planningPackage);

            return;
        }

        if (packageItem.Type is
            "Workflow/FlowDefinition"
            or "Core/FlowDefinition")
        {
            Package workflowPackage = new("Workflow")
            {
                Items = [canonicalPackageItem],
            };

            await workflowPackageService.ImportPackageAsync(
                appId: appId,
                package: workflowPackage);

            return;
        }

        if (packageItem.Type is
            "DocumentManagement/FolderRole"
            or "Core/FolderRole")
        {
            Package documentPackage =
                new("DocumentManagement")
                {
                    Items = [canonicalPackageItem],
                };

            await documentManagementPackageService.ImportPackageAsync(
                appId: appId,
                package: documentPackage);

            return;
        }

        if (packageItem.Type is
            "AppSecurity/Role"
            or "Core/Role")
        {
            Package appSecurityPackage =
                new("AppSecurity")
                {
                    Items = [canonicalPackageItem],
                };

            await appSecurityPackageService.ImportPackageAsync(
                appId: appId,
                package: appSecurityPackage);

            return;
        }

        Package contentPackage =
            new(packageName)
            {
                Items = [packageItem],
            };

        await contentManagementPackageService.ImportPackageAsync(
            appId: appId,
            package: contentPackage);
    }

    private static PackageItem ToCanonicalDomainPackageItem(
        PackageItem packageItem) =>
        new()
        {
            Id = packageItem.Id,
            PackageId = packageItem.PackageId,
            Type = packageItem.Type switch
            {
                string type when type.StartsWith(
                    value: "AppSecurity/",
                    comparisonType: StringComparison.OrdinalIgnoreCase) =>
                    $"Core/{type["AppSecurity/".Length..]}",
                string type when type.StartsWith(
                    value: "DocumentManagement/",
                    comparisonType: StringComparison.OrdinalIgnoreCase) =>
                    $"Core/{type["DocumentManagement/".Length..]}",
                string type when type.StartsWith(
                    value: "Workflow/",
                    comparisonType: StringComparison.OrdinalIgnoreCase) =>
                    $"Core/{type["Workflow/".Length..]}",
                _ => packageItem.Type,
            },
            Data = packageItem.Data,
        };
}