// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Models;

namespace cCoder.Packaging.Exposures.Configuration;

public sealed class ConfigProvider(PackagingConfiguration configuration)
    : IConfigProvider
{
    public string GetPackageSourceSslPort() =>
        configuration.PackageSourceSslPort;
}