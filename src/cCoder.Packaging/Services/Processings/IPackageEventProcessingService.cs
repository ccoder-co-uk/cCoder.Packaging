// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Threading.Tasks;
using cCoder.Data.Models.Packaging;


namespace cCoder.Packaging.Services.Processings;

internal interface IPackageEventProcessingService
{
    ValueTask RaisePackageImportEventAsync(int? appId, Package package);
    ValueTask RaisePackageAddEventAsync(Package package);
    ValueTask RaisePackageUpdateEventAsync(Package package);
    ValueTask RaisePackageDeleteEventAsync(Package package);
}