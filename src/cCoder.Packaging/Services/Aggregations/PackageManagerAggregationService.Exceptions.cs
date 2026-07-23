// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Models.Exceptions;

namespace cCoder.Packaging.Services.Aggregations;

internal sealed partial class PackageManagerAggregationService
{
    private static T TryCatch<T>(Func<T> operation)
    {
        try
        {
            return operation();
        }
        catch (PackagingOrchestrationValidationException innerException)
        {
            throw new PackagingOrchestrationValidationException(innerException: innerException);
        }
        catch (PackagingOrchestrationDependencyException innerException)
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
        catch (PackagingOrchestrationValidationException innerException)
        {
            throw new PackagingOrchestrationValidationException(innerException: innerException);
        }
        catch (PackagingOrchestrationDependencyException innerException)
        {
            throw new PackagingOrchestrationDependencyException(innerException: innerException);
        }
        catch (Exception innerException)
        {
            throw new PackagingOrchestrationServiceException(innerException: innerException);
        }
    }
}