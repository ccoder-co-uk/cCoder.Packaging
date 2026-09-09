// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

namespace cCoder.Packaging.Models.Exceptions;

internal sealed class PackagingProcessingValidationException(Exception innerException)
    : Exception("Packaging processing validation failed.", innerException);