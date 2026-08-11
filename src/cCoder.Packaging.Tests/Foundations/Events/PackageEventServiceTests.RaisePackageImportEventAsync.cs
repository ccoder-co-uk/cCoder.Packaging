// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;
using cCoder.Eventing.Models;
using cCoder.Packaging.Models;
using FluentAssertions;
using Moq;
using System.Text.Json;
using Xunit;


namespace cCoder.Packaging.Tests.Foundations.Events;

public partial class PackageEventServiceTests
{
    [Theory]
    [InlineData(7)]
    [InlineData(null)]
    public async Task ShouldRaisePackageImportEventWithNamedPayloadAsync(
        int? appId)
    {
        // Given
        Package package = new()
        {
            Name = "Roles",
            Items =
            [
                new PackageItem
                {
                    Data = "{}",
                },
            ],
        };

        EventMessage<PackageImportEvent> actualMessage = null;

        packageEventBrokerMock
            .Setup(expression: x => x.RaisePackageImportEventAsync(message: It.IsAny<EventMessage<PackageImportEvent>>()))
            .Callback<EventMessage<PackageImportEvent>>(action: message => actualMessage = message)
            .Returns(value: ValueTask.CompletedTask);

        // When
        await service.RaisePackageImportEventAsync(
            appId: appId,
            package: package);

        // Then
        actualMessage.Should()
            .NotBeNull();

        actualMessage!.Data.AppId.Should()
            .Be(expected: appId);

        actualMessage.Data.Package.Should()
            .BeSameAs(expected: package);

        actualMessage.AuthInfo.Should()
            .NotBeNull();

        actualMessage.AuthInfo.SSOUserId.Should()
            .Be(expected: CurrentUserId);

        string serializedMessage = JsonSerializer.Serialize(value: actualMessage);

        string expectedAppId = appId.HasValue
            ? appId.Value.ToString()
            : "null";

        serializedMessage.Should()
            .Contain(expected: $"\"AppId\":{expectedAppId}");

        serializedMessage.Should()
            .Contain(expected: "\"Package\":");

        packageEventBrokerMock.Verify(
            expression: x => x.RaisePackageImportEventAsync(
                message: It.IsAny<EventMessage<PackageImportEvent>>()),
            times: Times.Once);

        packageEventBrokerMock.VerifyNoOtherCalls();
    }
}