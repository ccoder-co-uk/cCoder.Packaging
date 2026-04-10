using cCoder.Data;
using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Brokers.Events;
using EventLibrary.Models;


namespace cCoder.Packaging.Services.Foundations.Events;

internal class PackageItemEventService(
    IPackageItemEventBroker packageItemEventBroker,
    ICoreAuthInfo authInfo
) : IPackageItemEventService
{
    public async ValueTask RaisePackageItemAddEventAsync(PackageItem entity)
    {
        EventMessage<PackageItem> message = new()
        {
            AuthInfo = new EventAuthInfo { SSOUserId = authInfo.SSOUserId },
            Data = entity,
        };

        await packageItemEventBroker.RaisePackageItemAddEventAsync(message);
    }

    public async ValueTask RaisePackageItemUpdateEventAsync(PackageItem entity)
    {
        EventMessage<PackageItem> message = new()
        {
            AuthInfo = new EventAuthInfo { SSOUserId = authInfo.SSOUserId },
            Data = entity,
        };

        await packageItemEventBroker.RaisePackageItemUpdateEventAsync(message);
    }

    public async ValueTask RaisePackageItemDeleteEventAsync(PackageItem entity)
    {
        EventMessage<PackageItem> message = new()
        {
            AuthInfo = new EventAuthInfo { SSOUserId = authInfo.SSOUserId },
            Data = entity,
        };

        await packageItemEventBroker.RaisePackageItemDeleteEventAsync(message);
    }
}









