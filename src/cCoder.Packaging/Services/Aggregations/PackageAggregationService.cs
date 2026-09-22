// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Extensions;
using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Models.Results;
using cCoder.Packaging.Models;
using cCoder.Packaging.Services.Processings;

namespace cCoder.Packaging.Services.Aggregations;

internal sealed partial class PackageAggregationService(
    IPackageProcessingService packageProcessingService,
    IPackageItemProcessingService packageItemProcessingService,
    IPackageEventProcessingService packageEventProcessingService,
    IPackageExportProcessingService packageExportProcessingService)
    : IPackageAggregationService
{
    public ValueTask<Package[]> ExportPackagesAsync(
        int? appId,
        string[] packageNames = null) =>
        TryCatch(operation: async () =>
        {
            if (appId is null)
            {
                return packageProcessingService.ExportCommonCachePackages();
            }

            ValidatePackagesOnExport(
                appId: appId.Value,
                packageNames: packageNames);

            string sourceApi = packageExportProcessingService
                .GetPackageSourceApi(appId: appId.Value);

            return await packageExportProcessingService
                .ExportPackagesAsync(
                    appId: appId.Value,
                    packageNames: packageNames ?? [],
                    sourceApi: sourceApi);
        });

    public ValueTask ImportPackageAsync(int? appId, Package package) =>
        TryCatch(operation: async () =>
        {
            ValidatePackageOnImport(appId: appId, package: package);

            if (appId is null)
            {
                package.Description ??= string.Empty;
                package.Category ??= string.Empty;
                package.SourceApi ??= string.Empty;

                Package savedPackage = await packageProcessingService
                    .AddPackageAsync(newPackage: package);

                foreach (PackageItem packageItem in package.Items ?? [])
                {
                    packageItem.Id = Guid.Empty;
                    packageItem.PackageId = savedPackage.Id;
                    packageItem.Package = null;
                }

                if (package.Items?.Count > 0)
                {
                    _ = await packageItemProcessingService
                        .AddOrUpdatePackageItemsAsync(packageItems: package.Items);
                }

                return;
            }

            await packageEventProcessingService
                .RaisePackageImportEventAsync(appId: appId, package: package);
        });

    public Package GetPackage(Guid packageId) =>
        TryCatch(operation: () =>
        {
            ValidatePackageOnGet(packageId: packageId);

            return packageProcessingService.GetPackage(packageId: packageId);
        });

    public IQueryable<Package> GetAllPackages(bool ignoreFilters = false) =>
        TryCatch(operation: () =>
        {
            ValidateAllPackagesOnGet(ignoreFilters: ignoreFilters);

            return packageProcessingService
                .GetAllPackages(ignoreFilters: ignoreFilters);
        });

    public ValueTask<Package> AddPackageAsync(Package newPackage) =>
        TryCatch(operation: async () =>
        {
            ValidatePackageOnAdd(newPackage: newPackage);

            Package savedPackage = await packageProcessingService
                .AddPackageAsync(newPackage: newPackage);

            await packageEventProcessingService
                .RaisePackageAddEventAsync(package: savedPackage);

            return savedPackage;
        });

    public ValueTask<Package> UpdatePackageAsync(Package updatedPackage) =>
        TryCatch(operation: async () =>
        {
            ValidatePackageOnUpdate(updatedPackage: updatedPackage);

            Package savedPackage = await packageProcessingService
                .UpdatePackageAsync(updatedPackage: updatedPackage);

            if (updatedPackage.Items is not null && updatedPackage.Items.Any())
            {
                PackageItem[] deletedPackageItems = packageItemProcessingService
                    .GetAllPackageItems()
                    .Where(predicate: packageItem => packageItem.PackageId == savedPackage.Id)
                    .ToArray();

                await packageItemProcessingService
                    .DeleteAllPackageItemsAsync(
                        deletedPackageItems: deletedPackageItems);

                updatedPackage.Items.ForEach(action: packageItem =>
                {
                    packageItem.PackageId = savedPackage.Id;
                });

                await packageItemProcessingService
                    .AddOrUpdatePackageItemsAsync(
                        packageItems: updatedPackage.Items);
            }

            await packageEventProcessingService
                .RaisePackageUpdateEventAsync(package: savedPackage);

            return savedPackage;
        });

    public ValueTask DeletePackageAsync(Guid packageId) =>
        TryCatch(operation: async () =>
        {
            ValidatePackageOnDelete(packageId: packageId);

            Package deletedPackage = packageProcessingService
                .GetPackage(packageId: packageId);

            await packageEventProcessingService
                .RaisePackageDeleteEventAsync(package: deletedPackage);

            await packageProcessingService
                .DeletePackageAsync(packageId: packageId);
        });

    public ValueTask<IEnumerable<Result<Package>>> AddOrUpdatePackagesAsync(
        IEnumerable<Package> packages) =>
        TryCatch(operation: async () =>
        {
            ValidateOrUpdatePackagesOnAdd(packages: packages);
            List<Result<Package>> results = [];

            foreach (Package package in packages)
            {
                try
                {
                    bool isNewPackage = package.Id == Guid.Empty;

                    Package savedPackage = isNewPackage
                        ? await packageProcessingService
                            .AddPackageAsync(newPackage: package)
                        : await packageProcessingService
                            .UpdatePackageAsync(updatedPackage: package);

                    results.Add(item: new Result<Package>
                    {
                        Success = true,
                        Item = savedPackage,
                        Message = isNewPackage
                            ? "Added Successfully"
                            : "Updated Successfully",
                    });
                }
                catch (Exception exception)
                {
                    results.Add(item: new Result<Package>
                    {
                        Success = false,
                        Item = package,
                        Message = exception.Message,
                    });
                }
            }

            return results.AsEnumerable();
        });

    public ValueTask DeleteAllPackagesAsync(IEnumerable<Package> deletedPackages) =>
        TryCatch(operation: async () =>
        {
            ValidateAllPackagesOnDelete(deletedPackages: deletedPackages);

            foreach (Package deletedPackage in deletedPackages)
            {
                await packageProcessingService
                    .DeletePackageAsync(packageId: deletedPackage.Id);
            }
        });
}