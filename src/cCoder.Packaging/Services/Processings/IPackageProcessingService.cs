using cCoder.Packaging.Models;
using cCoder.Data.Models.Packaging;

namespace cCoder.Packaging.Services.Processings;

public interface IPackageProcessingService
{
    cCoder.Data.Models.Packaging.Package ExportPackage(int appId, string packageName);

    cCoder.Data.Models.Packaging.Package[] ExportPackages(int appId, string[] packageNames);

    cCoder.Data.Models.Packaging.Package Get(Guid id);

    IQueryable<cCoder.Data.Models.Packaging.Package> GetAll(bool ignoreFilters = false);

    ValueTask<cCoder.Data.Models.Packaging.Package> AddAsync(cCoder.Data.Models.Packaging.Package entity);

    ValueTask<cCoder.Data.Models.Packaging.Package> UpdateAsync(cCoder.Data.Models.Packaging.Package entity);

    ValueTask DeleteAsync(Guid id);

    ValueTask<IEnumerable<Result<cCoder.Data.Models.Packaging.Package>>> AddOrUpdate(IEnumerable<cCoder.Data.Models.Packaging.Package> items);

    ValueTask DeleteAllAsync(IEnumerable<cCoder.Data.Models.Packaging.Package> items);
}
