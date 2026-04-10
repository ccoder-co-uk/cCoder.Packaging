using cCoder.Data.Models.Packaging;


namespace cCoder.Packaging.Services.Foundations.Events;

public interface IPackageItemEventService
{
    ValueTask RaisePackageItemAddEventAsync(PackageItem entity);
    ValueTask RaisePackageItemUpdateEventAsync(PackageItem entity);
    ValueTask RaisePackageItemDeleteEventAsync(PackageItem entity);
}








