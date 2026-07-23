// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Exposures.Setup;

namespace cCoder.Packaging.Services.Foundations.Baselines;

internal sealed partial class BaselineService : IBaselineService
{
    public Package[] GetBaselinePackages() =>
        TryCatch(operation: () =>
        {
            ValidateBaselinePackagesOnGet();

            return UIBaseline.Packages;
        });
}