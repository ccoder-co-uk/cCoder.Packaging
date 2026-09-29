// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System;

namespace cCoder.Packaging.Models.Exceptions;

internal sealed class PackagingOrchestrationDependencyException(Exception innerException)
    : Exception("A Packaging orchestration dependency failed.", innerException);