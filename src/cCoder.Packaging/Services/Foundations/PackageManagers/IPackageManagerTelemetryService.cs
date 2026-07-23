// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;

namespace cCoder.Packaging.Services.Foundations.PackageManagers;

internal interface IPackageManagerTelemetryService
{
    void EnsurePackageAdmin(int appId);
    void LogPackageItemImport(PackageItem packageItem, string packageSource);
}