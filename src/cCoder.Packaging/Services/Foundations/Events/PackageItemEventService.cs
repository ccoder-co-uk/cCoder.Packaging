// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Brokers;
using cCoder.Packaging.Brokers.Events;
using cCoder.Eventing.Models;


namespace cCoder.Packaging.Services.Foundations.Events;

internal sealed partial class PackageItemEventService(
    IPackageItemEventBroker packageItemEventBroker,
    IAuthInfoBroker authInfoBroker
) : IPackageItemEventService
{
    public ValueTask RaisePackageItemAddEventAsync(PackageItem packageItem) =>
        TryCatch(operation: async () =>
        {
            ValidatePackageItemEventOnAdd(newPackageItem: packageItem);

            EventMessage<PackageItem> message = new()
            {
                AuthInfo = new EventAuthInfo { SSOUserId = authInfoBroker.GetSSOUserId() },
                Data = packageItem,
            };

            await packageItemEventBroker.RaisePackageItemAddEventAsync(message: message);
        });

    public ValueTask RaisePackageItemUpdateEventAsync(PackageItem packageItem) =>
        TryCatch(operation: async () =>
        {
            ValidatePackageItemEventOnUpdate(updatedPackageItem: packageItem);

            EventMessage<PackageItem> message = new()
            {
                AuthInfo = new EventAuthInfo { SSOUserId = authInfoBroker.GetSSOUserId() },
                Data = packageItem,
            };

            await packageItemEventBroker.RaisePackageItemUpdateEventAsync(message: message);
        });

    public ValueTask RaisePackageItemDeleteEventAsync(PackageItem packageItem) =>
        TryCatch(operation: async () =>
        {
            ValidatePackageItemEventOnDelete(deletedPackageItem: packageItem);

            EventMessage<PackageItem> message = new()
            {
                AuthInfo = new EventAuthInfo { SSOUserId = authInfoBroker.GetSSOUserId() },
                Data = packageItem,
            };

            await packageItemEventBroker.RaisePackageItemDeleteEventAsync(message: message);
        });
}