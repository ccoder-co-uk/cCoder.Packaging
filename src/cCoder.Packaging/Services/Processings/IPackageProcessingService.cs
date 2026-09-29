// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using System.Linq;
using cCoder.Data.Models.Packaging;

namespace cCoder.Packaging.Services.Processings;

internal interface IPackageProcessingService
{
    Package ExportPackage(int appId, string packageName);
    Package[] ExportPackages(int appId, string[] packageNames);
    Package[] ExportCommonCachePackages();
    Package GetPackage(Guid packageId);
    IQueryable<Package> GetAllPackages(bool ignoreFilters = false);
    ValueTask<Package> AddPackageAsync(Package newPackage);
    ValueTask<Package> UpdatePackageAsync(Package updatedPackage);
    ValueTask DeletePackageAsync(Guid packageId);
}