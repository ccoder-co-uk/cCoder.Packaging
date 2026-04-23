using cCoder.Data;
using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Brokers.Events;
using cCoder.Eventing.Models;


namespace cCoder.Packaging.Services.Foundations.Events;

internal class PackageEventService(IPackageEventBroker packageEventBroker, ICoreAuthInfo authInfo)
    : IPackageEventService
{
    public async ValueTask RaisePackageImportEventAsync(int appId, Package package)
    {
        EventMessage<(int, Package)> message = new()
        {
            AuthInfo = new EventAuthInfo { SSOUserId = authInfo.SSOUserId },
            Data = (appId, package),
        };

        await packageEventBroker.RaisePackageImportEventAsync(message);
    }

    public async ValueTask RaisePackageAddEventAsync(Package package)
    {
        EventMessage<Package> message = new()
        {
            AuthInfo = new EventAuthInfo { SSOUserId = authInfo.SSOUserId },
            Data = package,
        };

        await packageEventBroker.RaisePackageAddEventAsync(message);
    }

    public async ValueTask RaisePackageUpdateEventAsync(Package package)
    {
        EventMessage<Package> message = new()
        {
            AuthInfo = new EventAuthInfo { SSOUserId = authInfo.SSOUserId },
            Data = package,
        };

        await packageEventBroker.RaisePackageUpdateEventAsync(message);
    }

    public async ValueTask RaisePackageDeleteEventAsync(Package package)
    {
        EventMessage<Package> message = new()
        {
            AuthInfo = new EventAuthInfo { SSOUserId = authInfo.SSOUserId },
            Data = package,
        };

        await packageEventBroker.RaisePackageDeleteEventAsync(message);
    }
}

