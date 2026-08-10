// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Models;
using cCoder.Eventing.Models;


namespace cCoder.Packaging.Brokers.Events;

public interface IPackageEventBroker
{
    ValueTask RaisePackageImportEventAsync(EventMessage<PackageImportEvent> message);
    ValueTask RaisePackageAddEventAsync(EventMessage<Package> message);
    ValueTask RaisePackageUpdateEventAsync(EventMessage<Package> message);
    ValueTask RaisePackageDeleteEventAsync(EventMessage<Package> message);
}