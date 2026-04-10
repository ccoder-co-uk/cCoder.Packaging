using cCoder.Data.Models.Packaging;


namespace cCoder.Packaging.Services.Processings;

public interface IPackageEventProcessingService
{
    ValueTask RaisePackageImportEventAsync(int appId, Package package);
    ValueTask RaisePackageAddEventAsync(Package entity);
    ValueTask RaisePackageUpdateEventAsync(Package entity);
    ValueTask RaisePackageDeleteEventAsync(Package entity);
}







