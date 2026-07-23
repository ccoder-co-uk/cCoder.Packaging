// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Dependencies;

namespace cCoder.Packaging.Services.Aggregations;

internal sealed partial class PackageManagerAggregationService
{
    private static void Validate(params object[] inputs) =>
        ValidationRulesEngine.Validate(inputs: inputs);

    private static void ValidatePackageOnImport(int appId, Package package) =>
        Validate(inputs: [appId, package]);

    private static void ValidatePackageOnExport(int appId, string packageName) =>
        Validate(inputs: [appId, packageName]);
}