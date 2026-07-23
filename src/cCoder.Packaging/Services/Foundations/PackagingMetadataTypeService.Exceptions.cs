// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Models.Exceptions;

namespace cCoder.Packaging.Services.Foundations;

internal sealed partial class PackagingMetadataTypeService
{
    private static T TryCatch<T>(Func<T> operation)
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