// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data;

namespace cCoder.Packaging.Exposures.Configuration;

public sealed class ConfigProvider(Config config) : IConfigProvider
{
    public IDictionary<string, string> GetSettings() =>
        config.Settings;
}