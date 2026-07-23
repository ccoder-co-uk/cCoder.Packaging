// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Models;
using cCoder.Data.Models.Packaging;

namespace cCoder.Packaging.Services.Processings;

public interface IPackageItemProcessingService
{
    cCoder.Data.Models.Packaging.PackageItem Get(Guid id);

    IQueryable<cCoder.Data.Models.Packaging.PackageItem> GetAll(bool ignoreFilters = false);

    ValueTask<cCoder.Data.Models.Packaging.PackageItem> AddAsync(cCoder.Data.Models.Packaging.PackageItem entity);

    ValueTask<cCoder.Data.Models.Packaging.PackageItem> UpdateAsync(cCoder.Data.Models.Packaging.PackageItem entity);

    ValueTask DeleteAsync(Guid id);

    ValueTask<IEnumerable<Result<cCoder.Data.Models.Packaging.PackageItem>>> AddOrUpdate(IEnumerable<cCoder.Data.Models.Packaging.PackageItem> items);

    ValueTask DeleteAllAsync(IEnumerable<cCoder.Data.Models.Packaging.PackageItem> items);
}