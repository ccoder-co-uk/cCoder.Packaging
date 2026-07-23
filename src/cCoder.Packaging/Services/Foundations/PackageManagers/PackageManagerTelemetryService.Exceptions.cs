// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Models.Exceptions;

namespace cCoder.Packaging.Services.Foundations.PackageManagers;

internal sealed partial class PackageManagerTelemetryService
{
    private static void TryCatch(Action operation)
    {
        try
        {
            operation();
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