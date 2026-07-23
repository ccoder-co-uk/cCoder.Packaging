// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Models;

namespace cCoder.Packaging.Services.Processings;

internal interface IPackageItemProcessingService
{
    PackageItem GetPackageItem(Guid packageItemId);
    IQueryable<PackageItem> GetAllPackageItems(bool ignoreFilters = false);
    ValueTask<PackageItem> AddPackageItemAsync(PackageItem newPackageItem);
    ValueTask<PackageItem> UpdatePackageItemAsync(PackageItem updatedPackageItem);
    ValueTask DeletePackageItemAsync(Guid packageItemId);

    ValueTask<IEnumerable<Result<PackageItem>>> AddOrUpdatePackageItemsAsync(
        IEnumerable<PackageItem> packageItems);

    ValueTask DeleteAllPackageItemsAsync(IEnumerable<PackageItem> deletedPackageItems);
}