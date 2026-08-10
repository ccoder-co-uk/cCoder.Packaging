// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Brokers.Loggings;

namespace cCoder.Packaging.Services.Foundations.Loggings;

internal sealed partial class LoggingFoundationService(
    ILoggingBroker loggingBroker)
    : ILoggingFoundationService
{
    public void LogError(Exception exception, string message) =>
        TryCatch(operation: () =>
        {
            ValidateLogError(exception: exception, message: message);
            loggingBroker.LogError(exception: exception, message: message);
        });
}