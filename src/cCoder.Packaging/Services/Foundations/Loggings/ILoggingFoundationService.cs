// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

namespace cCoder.Packaging.Services.Foundations.Loggings;

internal interface ILoggingFoundationService
{
    void LogError(Exception exception, string message);
}