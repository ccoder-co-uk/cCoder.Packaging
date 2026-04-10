using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Services.Foundations.Events;


namespace cCoder.Packaging.Services.Processings;

internal class PackageEventProcessingService(IPackageEventService eventService)
    : IPackageEventProcessingService
{
    public ValueTask RaisePackageImportEventAsync(int appId, Package package) =>
        eventService.RaisePackageImportEventAsync(appId, package);

    public ValueTask RaisePackageAddEventAsync(Package package) =>
        eventService.RaisePackageAddEventAsync(package);

    public ValueTask RaisePackageUpdateEventAsync(Package package) =>
        eventService.RaisePackageUpdateEventAsync(package);

    public ValueTask RaisePackageDeleteEventAsync(Package package) =>
        eventService.RaisePackageDeleteEventAsync(package);
}



