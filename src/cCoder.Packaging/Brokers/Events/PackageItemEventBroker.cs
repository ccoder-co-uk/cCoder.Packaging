using cCoder.Data.Models.Packaging;
using EventLibrary;
using EventLibrary.Models;


namespace cCoder.Packaging.Brokers.Events;

public class PackageItemEventBroker(IEventHub eventHub) : IPackageItemEventBroker
{
    public ValueTask RaisePackageItemAddEventAsync(EventMessage<PackageItem> message) =>
        eventHub.RaiseEventAsync("package_item_add", message);

    public ValueTask RaisePackageItemUpdateEventAsync(EventMessage<PackageItem> message) =>
        eventHub.RaiseEventAsync("package_item_update", message);

    public ValueTask RaisePackageItemDeleteEventAsync(EventMessage<PackageItem> message) =>
        eventHub.RaiseEventAsync("package_item_delete", message);
}







