// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Models.Exceptions;

namespace cCoder.Packaging.Services.Foundations.PackageManagers;

internal sealed partial class AppSecurityPackageService
{
    private static T TryCatch<T>(Func<T> operation)
    {
        try
        {
            return operation();
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

    private static async ValueTask TryCatch(Func<ValueTask> operation)
    {
        try
        {
            await operation();
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

internal sealed partial class ContentManagementPackageService
{
    private static T TryCatch<T>(Func<T> operation)
    {
        try
        {
            return operation();
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

    private static async ValueTask TryCatch(Func<ValueTask> operation)
    {
        try
        {
            await operation();
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

internal sealed partial class DocumentManagementPackageService
{
    private static T TryCatch<T>(Func<T> operation)
    {
        try
        {
            return operation();
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

    private static async ValueTask TryCatch(Func<ValueTask> operation)
    {
        try
        {
            await operation();
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

internal sealed partial class SchedulingPackageService
{
    private static T TryCatch<T>(Func<T> operation)
    {
        try
        {
            return operation();
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

    private static async ValueTask TryCatch(Func<ValueTask> operation)
    {
        try
        {
            await operation();
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

internal sealed partial class WorkflowPackageService
{
    private static T TryCatch<T>(Func<T> operation)
    {
        try
        {
            return operation();
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

    private static async ValueTask TryCatch(Func<ValueTask> operation)
    {
        try
        {
            await operation();
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