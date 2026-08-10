// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Services.Foundations.Loggings;

namespace cCoder.Packaging.Exposures;

internal sealed class PackagingLoggingManager(
    ILoggingFoundationService loggingFoundationService)
    : IPackagingLoggingManager
{
    public void LogError(Exception exception, string message) =>
        loggingFoundationService.LogError(
            exception: exception,
            message: message);
}