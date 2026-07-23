// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;

namespace cCoder.Packaging.Brokers;

internal interface IPackageLoggerBroker
{
    void LogPackageItemImport(PackageItem packageItem, string packageSource);
}