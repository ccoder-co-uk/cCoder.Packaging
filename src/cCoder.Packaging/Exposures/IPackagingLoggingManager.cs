// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

namespace cCoder.Packaging.Exposures;

internal interface IPackagingLoggingManager
{
    void LogError(Exception exception, string message);
}