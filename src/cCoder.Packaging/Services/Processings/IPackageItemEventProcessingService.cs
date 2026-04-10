using cCoder.Data.Models.Packaging;


namespace cCoder.Packaging.Services.Processings;

public interface IPackageItemEventProcessingService
{
    ValueTask RaisePackageItemAddEventAsync(PackageItem entity);
    ValueTask RaisePackageItemUpdateEventAsync(PackageItem entity);
    ValueTask RaisePackageItemDeleteEventAsync(PackageItem entity);
}







