// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

namespace cCoder.Packaging.Exposures.Configuration;

public interface IConfigProvider
{
    IDictionary<string, string> GetSettings();
}