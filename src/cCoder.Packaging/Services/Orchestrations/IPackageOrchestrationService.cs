using cCoder.Packaging.Models;
using cCoder.Data.Models.Packaging;

namespace cCoder.Packaging.Services.Orchestrations;

public interface IPackageOrchestrationService
{
    IEnumerable<Package> Export(int appId, string[] packageNames = null);

    ValueTask ImportPackageAsync(int appId, Package package);

    Package Get(Guid id);

    IQueryable<Package> GetAll(bool ignoreFilters = false);

    ValueTask<Package> AddAsync(Package entity);

    ValueTask<Package> UpdateAsync(Package entity);

    ValueTask DeleteAsync(Guid id);

    ValueTask<IEnumerable<Result<Package>>> AddOrUpdate(IEnumerable<Package> items);

    ValueTask DeleteAllAsync(IEnumerable<Package> items);
}
