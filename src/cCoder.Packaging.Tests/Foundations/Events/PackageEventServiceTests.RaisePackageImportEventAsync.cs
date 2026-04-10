using cCoder.Data.Models.Packaging;
using EventLibrary.Models;
using FluentAssertions;
using Moq;
using Xunit;


namespace cCoder.Packaging.Tests.Foundations.Events;

public partial class PackageEventServiceTests
{
    [Fact]
    public async Task ShouldRaisePackageImportEventWhenRaisePackageImportEventAsync()
    {
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
            .Setup(x => x.RaisePackageImportEventAsync(It.IsAny<EventMessage<(int, Package)>>()))
            .Callback<EventMessage<(int, Package)>>(message => actualMessage = message)
            .Returns(ValueTask.CompletedTask);

        await service.RaisePackageImportEventAsync(7, package);

        actualMessage.Should().NotBeNull();
        actualMessage!.Data.Item1.Should().Be(7);
        actualMessage.Data.Item2.Should().BeSameAs(package);
        actualMessage.AuthInfo.Should().NotBeNull();
        actualMessage.AuthInfo.SSOUserId.Should().Be(CurrentUserId);
        packageEventBrokerMock.Verify(
            x => x.RaisePackageImportEventAsync(It.IsAny<EventMessage<(int, Package)>>()),
            Times.Once
        );
        packageEventBrokerMock.VerifyNoOtherCalls();
    }
}

