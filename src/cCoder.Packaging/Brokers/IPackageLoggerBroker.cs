// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;
using cCoder.CodeAnalysis.Exposures;

namespace cCoder.Packaging.Brokers;

internal interface IPackageLoggerBroker : IUtilityBroker
{
    void LogPackageItemImport(PackageItem packageItem, string packageSource);
}