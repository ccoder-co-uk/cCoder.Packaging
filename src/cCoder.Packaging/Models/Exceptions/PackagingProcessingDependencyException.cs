// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System;

namespace cCoder.Packaging.Models.Exceptions;

internal sealed class PackagingProcessingDependencyException(Exception innerException)
    : Exception("A Packaging processing dependency failed.", innerException);