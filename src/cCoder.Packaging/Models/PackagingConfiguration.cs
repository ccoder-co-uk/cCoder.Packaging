// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

namespace cCoder.Packaging.Models;

public sealed class PackagingConfiguration
{
    public PackagingConfiguration()
    {
        ConnectionString = string.Empty;
        AssetsRoot =
            "https://raw.githubusercontent.com/ccoder-co-uk/" +
            "cCoder.Assets/main/Packages/";
        PackageSourceSslPort = "443";
        RootPath = "Api/Packaging";
    }

    public string ConnectionString { get; set; }

    public string AssetsRoot { get; set; }

    public string PackageSourceSslPort { get; set; }

    public string RootPath { get; set; }
}