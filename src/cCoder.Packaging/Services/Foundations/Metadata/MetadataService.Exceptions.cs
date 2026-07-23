// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

namespace cCoder.Packaging.Services.Foundations.Metadata;

internal sealed partial class MetadataService
{
    private static TResult TryCatch<TResult>(Func<TResult> operation)
    {
        try
        {
            return operation();
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception exception)
        {
            throw new InvalidOperationException(
                message: "A metadata dependency error occurred.",
                innerException: exception);
        }
    }
}