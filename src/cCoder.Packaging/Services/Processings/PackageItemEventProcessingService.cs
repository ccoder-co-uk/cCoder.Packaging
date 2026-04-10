using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Services.Foundations.Events;


namespace cCoder.Packaging.Services.Processings;

internal class PackageItemEventProcessingService(IPackageItemEventService eventService) : IPackageItemEventProcessingService
{
    public ValueTask RaisePackageItemAddEventAsync(PackageItem entity) => eventService.RaisePackageItemAddEventAsync(entity);

    public ValueTask RaisePackageItemUpdateEventAsync(PackageItem entity) => eventService.RaisePackageItemUpdateEventAsync(entity);

    public ValueTask RaisePackageItemDeleteEventAsync(PackageItem entity) => eventService.RaisePackageItemDeleteEventAsync(entity);
}








