// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;


namespace cCoder.Packaging.Services.Processings;

internal interface IPackageItemEventProcessingService
{
    ValueTask RaisePackageItemAddEventAsync(PackageItem newPackageItem);
    ValueTask RaisePackageItemUpdateEventAsync(PackageItem updatedPackageItem);
    ValueTask RaisePackageItemDeleteEventAsync(PackageItem deletedPackageItem);
}