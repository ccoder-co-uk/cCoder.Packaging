// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System;

namespace cCoder.Packaging.Services.Foundations.Loggings;

internal interface ILoggingFoundationService
{
    void LogError(Exception exception, string message);
}