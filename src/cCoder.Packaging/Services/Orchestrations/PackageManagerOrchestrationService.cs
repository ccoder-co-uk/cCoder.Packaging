using System.Security;
using cCoder.Packaging.Brokers;
using cCoder.Packaging.Models;
using cCoder.Data.Models.Packaging;


namespace cCoder.Packaging.Services.Orchestrations;

internal class PackageManagerOrchestrationService(
    ILogger<PackageManagerOrchestrationService> logger,
    IAuthorizationBroker authorizationBroker,
    IAppSecurityPackageService appSecurityPackageService,
    ISchedulingPackageService schedulingPackageService,
    IWorkflowPackageService workflowPackageService,
    IDocumentManagementPackageService documentManagementPackageService,
    IContentManagementPackageService contentManagementPackageService
) : IPackageManagerOrchestrationService
{
    public async ValueTask ImportPackageAsync(int appId, Package package)
    {
        try
        {
            if (package.Items is null || package.Items.Count == 0)
                return;

            if (!authorizationBroker.IsAdminOfApp(appId))
                throw new SecurityException("Access Denied!");

            foreach (PackageItem item in package.Items)
            {
                logger.LogDebug(
                    "Importing {ItemType} items from {PackageSource}",
                    item.Type,
                    package.SourceApi
                );

                if (item.Type is "Core/Calendar" or "Core/CalendarEvent")
                {
                    await schedulingPackageService.ImportPackageAsync(
                        appId,
                        new Package("Planning") { Items = [item] }
                    );
                    continue;
                }

                if (item.Type == "Core/FlowDefinition")
                {
                    await workflowPackageService.ImportPackageAsync(
                        appId,
                        new Package("Workflow") { Items = [item] }
                    );
                    continue;
                }

                if (item.Type == "Core/FolderRole")
                {
                    await documentManagementPackageService.ImportPackageAsync(
                        appId,
                        new Package("DocumentManagement") { Items = [item] }
                    );
                    continue;
                }

                if (item.Type == "Core/Role")
                {
                    await appSecurityPackageService.ImportPackageAsync(
                        appId,
                        new Package("AppSecurity") { Items = [item] }
                    );
                    continue;
                }

                await contentManagementPackageService.ImportPackageAsync(
                    appId,
                    new Package("ContentManagement") { Items = [item] }
                );
            }
        }
        catch (Exception exception)
        {
            logger.LogWarning(exception, "Exception importing package");
            if (exception.InnerException is not null)
                logger.LogWarning(exception.InnerException, "Inner exception importing package");

            throw;
        }
    }

    public Package ExportPackage(int appId, string packageName)
    {
        if (!authorizationBroker.IsAdminOfApp(appId))
            throw new SecurityException("Access Denied!");

        if (packageName is "Calendars" or "CalendarEvents")
            return schedulingPackageService.ExportPackage(appId, packageName);

        if (packageName == "Workflows")
            return workflowPackageService.ExportPackage(appId, packageName);

        if (packageName == "FolderRoles")
            return documentManagementPackageService.ExportPackage(appId, packageName);

        if (packageName == "Roles")
            return appSecurityPackageService.ExportPackage(appId, packageName);

        return contentManagementPackageService.ExportPackage(appId, packageName);
    }
}


