// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

namespace cCoder.Packaging.Models.Exceptions;

internal sealed class PackagingValidationException(Exception innerException)
    : Exception("Packaging validation failed.", innerException);