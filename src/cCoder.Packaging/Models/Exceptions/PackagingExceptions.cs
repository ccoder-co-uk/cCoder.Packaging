// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

namespace cCoder.Packaging.Models.Exceptions;

internal sealed class PackagingValidationException(Exception innerException)
    : Exception("Packaging validation failed.", innerException);

internal sealed class PackagingDependencyException(Exception innerException)
    : Exception("A Packaging dependency failed.", innerException);

internal sealed class PackagingServiceException(Exception innerException)
    : Exception("The Packaging service failed.", innerException);

internal sealed class PackagingProcessingValidationException(Exception innerException)
    : Exception("Packaging processing validation failed.", innerException);

internal sealed class PackagingProcessingDependencyException(Exception innerException)
    : Exception("A Packaging processing dependency failed.", innerException);

internal sealed class PackagingProcessingServiceException(Exception innerException)
    : Exception("The Packaging processing service failed.", innerException);

internal sealed class PackagingOrchestrationValidationException(Exception innerException)
    : Exception("Packaging orchestration validation failed.", innerException);

internal sealed class PackagingOrchestrationDependencyException(Exception innerException)
    : Exception("A Packaging orchestration dependency failed.", innerException);

internal sealed class PackagingOrchestrationServiceException(Exception innerException)
    : Exception("The Packaging orchestration service failed.", innerException);