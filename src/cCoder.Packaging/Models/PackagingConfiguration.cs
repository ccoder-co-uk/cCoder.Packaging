// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

namespace cCoder.Packaging.Models;

public sealed class PackagingConfiguration
{
    public string ConnectionString { get; set; }

    public string AssetsRoot { get; set; }

    public string PackageSourceSslPort { get; set; }

    public string RootPath { get; set; }
}