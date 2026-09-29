// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System;

namespace cCoder.Packaging.Models.Exceptions;

internal sealed class PackagingOrchestrationServiceException(Exception innerException)
    : Exception("The Packaging orchestration service failed.", innerException);