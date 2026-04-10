using cCoder.Packaging.Brokers;
using cCoder.Packaging.Models;
using cCoder.Data.Models.Packaging;


namespace cCoder.Packaging.Services;

public interface IWorkflowPackageService
{
    ValueTask ImportPackageAsync(int appId, Package package);

    Package ExportPackage(int appId, string packageName);
}

internal class WorkflowPackageService(IWorkflowPackageManagerBroker workflowPackageManagerBroker)
    : IWorkflowPackageService
{
    public ValueTask ImportPackageAsync(int appId, Package package) =>
        workflowPackageManagerBroker.ImportPackageAsync(appId, package);

    public Package ExportPackage(int appId, string packageName) =>
        workflowPackageManagerBroker.ExportPackage(appId, packageName);
}


