// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Models.Exceptions;

namespace cCoder.Packaging.Services.Foundations.Loggings;

internal sealed partial class LoggingFoundationService
{
    private static void TryCatch(Action operation)
    {
        try
        {
            operation();
        }
        catch (ArgumentException innerException)
        {
            throw new PackagingValidationException(
                innerException: innerException);
        }
        catch (PackagingDependencyException innerException)
        {
            throw new PackagingDependencyException(
                innerException: innerException);
        }
        catch (Exception innerException)
        {
            throw new PackagingDependencyException(
                innerException: innerException);
        }
    }
}