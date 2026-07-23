// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Models.Exceptions;

namespace cCoder.Packaging.Services.Foundations.Metadata;

internal sealed partial class MetadataService
{
    private static TResult TryCatch<TResult>(Func<TResult> operation)
    {
        try
        {
            return operation();
        }
        catch (ArgumentException innerException)
        {
            throw new PackagingValidationException(innerException: innerException);
        }
        catch (PackagingValidationException innerException)
        {
            throw new PackagingValidationException(innerException: innerException);
        }
        catch (PackagingDependencyException innerException)
        {
            throw new PackagingDependencyException(innerException: innerException);
        }
        catch (Exception innerException)
        {
            throw new PackagingServiceException(innerException: innerException);
        }
    }
}