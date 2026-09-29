// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using cCoder.Data.Models.Packaging;

namespace cCoder.Packaging.Brokers.PackageTransfers;

internal sealed class UnavailablePackageTransferBroker
    : IPackageTransferBroker
{
    public string GetRequestDomain() =>
        throw new NotSupportedException(
            "App package export requires a composition-root package transfer broker.");

    public ValueTask<Package[]> ExportPackagesAsync(
        int appId,
        string[] packageNames,
        string sourceApi) =>
        throw new NotSupportedException(
            "App package export requires a composition-root package transfer broker.");
}