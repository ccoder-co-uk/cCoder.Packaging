// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using System.Linq;
using cCoder.Data.Models.Packaging;

namespace cCoder.Packaging.Services.Foundations.Storages;

internal interface IPackageItemService
{
    PackageItem GetPackageItem(Guid packageItemId);
    IQueryable<PackageItem> GetAllPackageItems(bool ignoreFilters = false);
    ValueTask<PackageItem> AddPackageItemAsync(PackageItem newPackageItem);
    ValueTask<PackageItem> UpdatePackageItemAsync(PackageItem updatedPackageItem);
    ValueTask DeletePackageItemAsync(Guid packageItemId);
}