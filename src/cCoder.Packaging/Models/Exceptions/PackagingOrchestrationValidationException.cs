// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

namespace cCoder.Packaging.Models.Exceptions;

internal sealed class PackagingOrchestrationValidationException(Exception innerException)
    : Exception("Packaging orchestration validation failed.", innerException);