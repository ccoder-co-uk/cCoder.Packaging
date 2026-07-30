// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;

namespace cCoder.Packaging.Exposures;

public interface IPackageTransferManager
{
    Package ExportPackage(int appId, string packageName);
    ValueTask ImportPackageAsync(int appId, Package package);
}