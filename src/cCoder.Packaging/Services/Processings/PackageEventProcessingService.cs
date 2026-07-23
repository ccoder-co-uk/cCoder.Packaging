// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Services.Foundations.Events;


namespace cCoder.Packaging.Services.Processings;

internal class PackageEventProcessingService(IPackageEventService eventService)
    : IPackageEventProcessingService
{
    public ValueTask RaisePackageImportEventAsync(int appId, Package package) =>
        eventService.RaisePackageImportEventAsync(appId:appId, package:package);

    public ValueTask RaisePackageAddEventAsync(Package package) =>
        eventService.RaisePackageAddEventAsync(entity:package);

    public ValueTask RaisePackageUpdateEventAsync(Package package) =>
        eventService.RaisePackageUpdateEventAsync(entity:package);

    public ValueTask RaisePackageDeleteEventAsync(Package package) =>
        eventService.RaisePackageDeleteEventAsync(entity:package);
}