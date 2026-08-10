// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

namespace cCoder.Packaging.Services.Foundations.Loggings;

internal sealed partial class LoggingFoundationService
{
    private static void ValidateLogError(Exception exception, string message)
    {
        ArgumentNullException.ThrowIfNull(argument: exception);
        ArgumentException.ThrowIfNullOrWhiteSpace(argument: message);
    }
}