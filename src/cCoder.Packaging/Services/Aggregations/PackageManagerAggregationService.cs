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

}