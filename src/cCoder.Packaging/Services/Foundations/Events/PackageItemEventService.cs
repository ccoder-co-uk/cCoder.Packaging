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
    public ValueTask RaisePackageItemAddEventAsync(PackageItem entity) =>
        TryCatch(operation: async () =>
        {
            ValidatePackageItemEventOnAdd(newPackageItem: entity);

            EventMessage<PackageItem> message = new()
            {
                AuthInfo = new EventAuthInfo { SSOUserId = authInfoBroker.GetSSOUserId() },
                Data = entity,
            };

            await packageItemEventBroker.RaisePackageItemAddEventAsync(message: message);
        });

    public ValueTask RaisePackageItemUpdateEventAsync(PackageItem entity) =>
        TryCatch(operation: async () =>
        {
            ValidatePackageItemEventOnUpdate(updatedPackageItem: entity);

            EventMessage<PackageItem> message = new()
            {
                AuthInfo = new EventAuthInfo { SSOUserId = authInfoBroker.GetSSOUserId() },
                Data = entity,
            };

            await packageItemEventBroker.RaisePackageItemUpdateEventAsync(message: message);
        });

    public ValueTask RaisePackageItemDeleteEventAsync(PackageItem entity) =>
        TryCatch(operation: async () =>
        {
            ValidatePackageItemEventOnDelete(deletedPackageItem: entity);

            EventMessage<PackageItem> message = new()
            {
                AuthInfo = new EventAuthInfo { SSOUserId = authInfoBroker.GetSSOUserId() },
                Data = entity,
            };

            await packageItemEventBroker.RaisePackageItemDeleteEventAsync(message: message);
        });
}