// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

namespace cCoder.Packaging.Models.Exceptions;

internal sealed class PackagingDependencyException(Exception innerException)
    : Exception("A Packaging dependency failed.", innerException);