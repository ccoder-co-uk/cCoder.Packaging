using cCoder.Data.Models.Packaging;
using EventLibrary.Models;


namespace cCoder.Packaging.Brokers.Events;

public interface IPackageItemEventBroker
{
    ValueTask RaisePackageItemAddEventAsync(EventMessage<PackageItem> message);
    ValueTask RaisePackageItemUpdateEventAsync(EventMessage<PackageItem> message);
    ValueTask RaisePackageItemDeleteEventAsync(EventMessage<PackageItem> message);
}







