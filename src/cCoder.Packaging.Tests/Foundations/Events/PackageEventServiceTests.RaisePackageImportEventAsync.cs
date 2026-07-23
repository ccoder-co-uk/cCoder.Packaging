// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;
using cCoder.Eventing.Models;
using FluentAssertions;
using Moq;
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

        EventMessage<(int, Package)> actualMessage = null;

        packageEventBrokerMock
            .Setup(expression: x => x.RaisePackageImportEventAsync(message: It.IsAny<EventMessage<(int, Package)>>()))
            .Callback<EventMessage<(int, Package)>>(action: message => actualMessage = message)
            .Returns(value: ValueTask.CompletedTask);

        // When
        await service.RaisePackageImportEventAsync(appId: 7, package: package);

        // Then
        actualMessage.Should()
            .NotBeNull();

        actualMessage!.Data.Item1.Should()
            .Be(expected: 7);

        actualMessage.Data.Item2.Should()
            .BeSameAs(expected: package);

        actualMessage.AuthInfo.Should()
            .NotBeNull();

        actualMessage.AuthInfo.SSOUserId.Should()
            .Be(expected: CurrentUserId);

        packageEventBrokerMock.Verify(
expression: x => x.RaisePackageImportEventAsync(message: It.IsAny<EventMessage<(int, Package)>>()),
times: Times.Once
        );

        packageEventBrokerMock.VerifyNoOtherCalls();
    }
}