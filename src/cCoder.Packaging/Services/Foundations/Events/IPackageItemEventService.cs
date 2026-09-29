// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Threading.Tasks;
using cCoder.Data.Models.Packaging;


namespace cCoder.Packaging.Services.Foundations.Events;

internal interface IPackageItemEventService
{
    ValueTask RaisePackageItemAddEventAsync(PackageItem packageItem);
    ValueTask RaisePackageItemUpdateEventAsync(PackageItem packageItem);
    ValueTask RaisePackageItemDeleteEventAsync(PackageItem packageItem);
}