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
                    packageSource: package.SourceApi);

                await ImportPackageItemAsync(
                    appId: appId,
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
        PackageItem packageItem)
    {
        if (packageItem.Type is "Core/Calendar" or "Core/CalendarEvent")
        {
            Package planningPackage = new("Planning") { Items = [packageItem] };

            await schedulingPackageService.ImportPackageAsync(
                appId: appId,
                package: planningPackage);

            return;
        }

        if (packageItem.Type == "Core/FlowDefinition")
        {
            Package workflowPackage = new("Workflow") { Items = [packageItem] };

            await workflowPackageService.ImportPackageAsync(
                appId: appId,
                package: workflowPackage);

            return;
        }

        if (packageItem.Type == "Core/FolderRole")
        {
            Package documentPackage =
                new("DocumentManagement") { Items = [packageItem] };

            await documentManagementPackageService.ImportPackageAsync(
                appId: appId,
                package: documentPackage);

            return;
        }

        if (packageItem.Type == "Core/Role")
        {
            Package appSecurityPackage =
                new("AppSecurity") { Items = [packageItem] };

            await appSecurityPackageService.ImportPackageAsync(
                appId: appId,
                package: appSecurityPackage);

            return;
        }

        Package contentPackage =
            new("ContentManagement") { Items = [packageItem] };

        await contentManagementPackageService.ImportPackageAsync(
            appId: appId,
            package: contentPackage);
    }

}