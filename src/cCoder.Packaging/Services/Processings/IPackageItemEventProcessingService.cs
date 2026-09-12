// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;


namespace cCoder.Packaging.Services.Processings;

internal interface IPackageItemEventProcessingService
{
    ValueTask RaisePackageItemAddEventAsync(PackageItem packageItem);
    ValueTask RaisePackageItemUpdateEventAsync(PackageItem packageItem);
    ValueTask RaisePackageItemDeleteEventAsync(PackageItem packageItem);
}