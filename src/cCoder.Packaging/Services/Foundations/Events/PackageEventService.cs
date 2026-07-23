// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Brokers;
using cCoder.Packaging.Brokers.Events;
using cCoder.Eventing.Models;


namespace cCoder.Packaging.Services.Foundations.Events;

internal sealed partial class PackageEventService(
    IPackageEventBroker packageEventBroker,
    IAuthInfoBroker authInfoBroker)
    : IPackageEventService
{
    public ValueTask RaisePackageImportEventAsync(int appId, Package package) =>
        TryCatch(operation: async () =>
        {
            ValidatePackageEventOnImport(appId: appId, package: package);

            EventMessage<(int, Package)> message = new()
            {
                AuthInfo = new EventAuthInfo { SSOUserId = authInfoBroker.GetSSOUserId() },
                Data = (appId, package),
            };

            await packageEventBroker.RaisePackageImportEventAsync(message: message);
        });

    public ValueTask RaisePackageAddEventAsync(Package package) =>
        TryCatch(operation: async () =>
        {
            ValidatePackageEventOnAdd(newPackage: package);

            EventMessage<Package> message = new()
            {
                AuthInfo = new EventAuthInfo { SSOUserId = authInfoBroker.GetSSOUserId() },
                Data = package,
            };

            await packageEventBroker.RaisePackageAddEventAsync(message: message);
        });

    public ValueTask RaisePackageUpdateEventAsync(Package package) =>
        TryCatch(operation: async () =>
        {
            ValidatePackageEventOnUpdate(updatedPackage: package);

            EventMessage<Package> message = new()
            {
                AuthInfo = new EventAuthInfo { SSOUserId = authInfoBroker.GetSSOUserId() },
                Data = package,
            };

            await packageEventBroker.RaisePackageUpdateEventAsync(message: message);
        });

    public ValueTask RaisePackageDeleteEventAsync(Package package) =>
        TryCatch(operation: async () =>
        {
            ValidatePackageEventOnDelete(deletedPackage: package);

            EventMessage<Package> message = new()
            {
                AuthInfo = new EventAuthInfo { SSOUserId = authInfoBroker.GetSSOUserId() },
                Data = package,
            };

            await packageEventBroker.RaisePackageDeleteEventAsync(message: message);
        });
}