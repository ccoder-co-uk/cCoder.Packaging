// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Models;
using cCoder.Data.Models.Packaging;


namespace cCoder.Packaging.Services.Aggregations;

internal interface IPackageManagerAggregationService
{
    Package ExportPackage(int appId, string packageName);
}