// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Models.Exceptions;

namespace cCoder.Packaging.Services.Orchestrations;

internal sealed partial class PackageOrchestrationService
{
    private static T TryCatch<T>(Func<T> operation)
    {
        try
        {
            return operation();
        }
        catch (PackagingProcessingValidationException innerException)
        {
            throw new PackagingOrchestrationValidationException(innerException: innerException);
        }
        catch (PackagingProcessingDependencyException innerException)
        {
            throw new PackagingOrchestrationDependencyException(innerException: innerException);
        }
        catch (Exception innerException)
        {
            throw new PackagingOrchestrationServiceException(innerException: innerException);
        }
    }

    private static async ValueTask TryCatch(Func<ValueTask> operation)
    {
        try
        {
            await operation();
        }
        catch (PackagingProcessingValidationException innerException)
        {
            throw new PackagingOrchestrationValidationException(innerException: innerException);
        }
        catch (PackagingProcessingDependencyException innerException)
        {
            throw new PackagingOrchestrationDependencyException(innerException: innerException);
        }
        catch (Exception innerException)
        {
            throw new PackagingOrchestrationServiceException(innerException: innerException);
        }
    }

    private static async ValueTask<T> TryCatch<T>(Func<ValueTask<T>> operation)
    {
        try
        {
            return await operation();
        }
        catch (PackagingProcessingValidationException innerException)
        {
            throw new PackagingOrchestrationValidationException(innerException: innerException);
        }
        catch (PackagingProcessingDependencyException innerException)
        {
            throw new PackagingOrchestrationDependencyException(innerException: innerException);
        }
        catch (Exception innerException)
        {
            throw new PackagingOrchestrationServiceException(innerException: innerException);
        }
    }
}