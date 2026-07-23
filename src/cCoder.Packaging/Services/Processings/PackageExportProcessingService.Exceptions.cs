// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Models.Exceptions;

namespace cCoder.Packaging.Services.Processings;

internal sealed partial class PackageExportProcessingService
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
}