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
    [Fact]
    public async Task ShouldRaisePackageImportEventWhenRaisePackageImportEventAsync()
    {
        // Given
        Package package = new("Roles")
        {
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
        await service.RaisePackageImportEventAsync(appId: 7, package: package);

        // Then
        actualMessage.Should()
            .NotBeNull();

        actualMessage!.Data.AppId.Should()
            .Be(expected: 7);

        actualMessage.Data.Package.Should()
            .BeSameAs(expected: package);

        actualMessage.AuthInfo.Should()
            .NotBeNull();

        actualMessage.AuthInfo.SSOUserId.Should()
            .Be(expected: CurrentUserId);

        string serializedMessage = JsonSerializer.Serialize(value: actualMessage);

        serializedMessage.Should()
            .Contain(expected: "\"AppId\":7");

        serializedMessage.Should()
            .Contain(expected: "\"Package\":");

        packageEventBrokerMock.Verify(
            expression: x => x.RaisePackageImportEventAsync(
                message: It.IsAny<EventMessage<PackageImportEvent>>()),
            times: Times.Once);

        packageEventBrokerMock.VerifyNoOtherCalls();
    }
}