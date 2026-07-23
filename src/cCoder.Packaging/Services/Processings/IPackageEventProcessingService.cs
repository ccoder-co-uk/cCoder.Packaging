// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;


namespace cCoder.Packaging.Services.Processings;

internal interface IPackageEventProcessingService
{
    ValueTask RaisePackageImportEventAsync(int appId, Package package);
    ValueTask RaisePackageAddEventAsync(Package newPackage);
    ValueTask RaisePackageUpdateEventAsync(Package updatedPackage);
    ValueTask RaisePackageDeleteEventAsync(Package deletedPackage);
}