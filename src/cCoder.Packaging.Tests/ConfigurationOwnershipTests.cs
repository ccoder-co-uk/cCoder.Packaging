// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data;
using cCoder.Packaging.Models;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace cCoder.Packaging.Tests;

public sealed partial class ConfigurationOwnershipTests
{
    [Fact]
    public void PackagingConfiguration_ShouldNotOwnPersistenceConfiguration()
    {
        // Given
        Type configurationType = typeof(PackagingConfiguration);

        // When
        string[] propertyNames = configurationType
            .GetProperties()
            .Select(selector: property => property.Name)
            .ToArray();

        // Then
        propertyNames.Should()
            .NotContain(unexpected: "ConnectionString");
    }

    [Fact]
    public void AddPackagingWeb_ShouldNotRegisterCoreDataServices()
    {
        // Given
        IServiceCollection services = new ServiceCollection();
        PackagingConfiguration configuration = new();

        typeof(PackagingConfiguration)
            .GetProperty(name: "ConnectionString")
            ?.SetValue(obj: configuration, value: "Server=(local);");

        // When
        services.AddPackagingWeb(configuration: configuration);

        // Then
        services.Should()
            .NotContain(predicate: descriptor =>
                descriptor.ServiceType == typeof(CoreDataContext));
    }
}