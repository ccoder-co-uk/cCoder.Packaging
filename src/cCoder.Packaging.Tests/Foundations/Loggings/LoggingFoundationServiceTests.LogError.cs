// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using Moq;
using Xunit;

namespace cCoder.Packaging.Tests.Foundations.Loggings;

public partial class LoggingFoundationServiceTests
{
    [Fact]
    public void ShouldLogError()
    {
        // Given
        Exception exception = new(message: "Controller request failed.");
        const string message = "Controller request failed.";

        // When
        loggingFoundationService.LogError(
            exception: exception,
            message: message);

        // Then
        loggingBrokerMock.Verify(
            expression: broker => broker.LogError(
                exception: exception,
                message: message,
                args: It.IsAny<object[]>()),
            times: Times.Once);

        loggingBrokerMock.VerifyNoOtherCalls();
    }
}