using cCoder.Data.Models.Packaging;


namespace cCoder.Packaging.Services.Foundations.Storages;

public interface IPackageItemService
{
    PackageItem Get(Guid id);
    IQueryable<PackageItem> GetAll(bool ignoreFilters = false);
    ValueTask<PackageItem> AddAsync(PackageItem packageItem);
    ValueTask<PackageItem> UpdateAsync(PackageItem packageItem);
    ValueTask DeleteAsync(Guid id);
}








