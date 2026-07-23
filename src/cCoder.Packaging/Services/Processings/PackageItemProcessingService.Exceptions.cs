// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Models.Exceptions;

namespace cCoder.Packaging.Services.Processings;

internal sealed partial class PackageItemProcessingService
{
    private static T TryCatch<T>(Func<T> operation)
    {
        try
        {
            return operation();
        }
        catch (PackagingValidationException innerException)
        {
            throw new PackagingProcessingValidationException(innerException: innerException);
        }
        catch (PackagingDependencyException innerException)
        {
            throw new PackagingProcessingDependencyException(innerException: innerException);
        }
        catch (Exception innerException)
        {
            throw new PackagingProcessingServiceException(innerException: innerException);
        }
    }

    private static async ValueTask TryCatch(Func<ValueTask> operation)
    {
        try
        {
            await operation();
        }
        catch (PackagingValidationException innerException)
        {
            throw new PackagingProcessingValidationException(innerException: innerException);
        }
        catch (PackagingDependencyException innerException)
        {
            throw new PackagingProcessingDependencyException(innerException: innerException);
        }
        catch (Exception innerException)
        {
            throw new PackagingProcessingServiceException(innerException: innerException);
        }
    }

    private static async ValueTask<T> TryCatch<T>(Func<ValueTask<T>> operation)
    {
        try
        {
            return await operation();
        }
        catch (PackagingValidationException innerException)
        {
            throw new PackagingProcessingValidationException(innerException: innerException);
        }
        catch (PackagingDependencyException innerException)
        {
            throw new PackagingProcessingDependencyException(innerException: innerException);
        }
        catch (Exception innerException)
        {
            throw new PackagingProcessingServiceException(innerException: innerException);
        }
    }
}