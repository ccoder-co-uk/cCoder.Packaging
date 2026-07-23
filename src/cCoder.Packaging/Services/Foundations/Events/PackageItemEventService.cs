// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data;
using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Brokers.Events;
using cCoder.Eventing.Models;


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

        await packageItemEventBroker.RaisePackageItemAddEventAsync(message:message);
    }

    public async ValueTask RaisePackageItemUpdateEventAsync(PackageItem entity)
    {
        EventMessage<PackageItem> message = new()
        {
            AuthInfo = new EventAuthInfo { SSOUserId = authInfo.SSOUserId },
            Data = entity,
        };

        await packageItemEventBroker.RaisePackageItemUpdateEventAsync(message:message);
    }

    public async ValueTask RaisePackageItemDeleteEventAsync(PackageItem entity)
    {
        EventMessage<PackageItem> message = new()
        {
            AuthInfo = new EventAuthInfo { SSOUserId = authInfo.SSOUserId },
            Data = entity,
        };

        await packageItemEventBroker.RaisePackageItemDeleteEventAsync(message:message);
    }
}