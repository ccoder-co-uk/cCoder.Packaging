// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System;

namespace cCoder.Packaging.Models.Exceptions;

internal sealed class PackagingProcessingValidationException(Exception innerException)
    : Exception("Packaging processing validation failed.", innerException);