// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.CodeAnalysis.Exposures;
using cCoder.Packaging.Brokers;
using cCoder.Packaging.Brokers.Events;
using cCoder.Packaging.Brokers.Loggings;
using FluentAssertions;
using Xunit;

namespace cCoder.Packaging.Tests;

public sealed partial class ArchitectureBoundaryTests
{
    [Fact]
    public void UtilityBrokers_WhenUsedCrossCutting_AreExplicitlyMarked()
    {
        // Given
        Type[] utilityBrokers =
        [
            typeof(IAuthInfoBroker),
            typeof(IAuthorizationBroker),
            typeof(ILoggingBroker),
        ];

        // When
        bool allBrokersAreUtilities = utilityBrokers.All(predicate: brokerType =>
            typeof(IUtilityBroker).IsAssignableFrom(c: brokerType));

        // Then
        allBrokersAreUtilities.Should()
            .BeTrue();
    }

    [Fact]
    public void OrdinaryEventBrokers_WhenUsedByFoundations_AreNotUtilityBrokers()
    {
        // Given
        Type[] ordinaryBrokers =
        [
            typeof(IPackageEventBroker),
            typeof(IPackageItemEventBroker),
        ];

        // When
        bool anyBrokerIsAUtility = ordinaryBrokers.Any(predicate: brokerType =>
            typeof(IUtilityBroker).IsAssignableFrom(c: brokerType));

        // Then
        anyBrokerIsAUtility.Should()
            .BeFalse();
    }
}