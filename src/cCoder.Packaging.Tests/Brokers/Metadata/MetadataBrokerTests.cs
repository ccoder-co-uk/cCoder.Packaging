// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.ComponentModel.DataAnnotations.Schema;
using cCoder.Packaging.Brokers.Metadata;
using cCoder.Packaging.Api.OData;
using FluentAssertions;
using Xunit;

namespace cCoder.Packaging.Tests.Brokers.Metadata;

public sealed partial class MetadataBrokerTests
{
    private readonly MetadataBroker metadataBroker = new();

    [Fact]
    public void CreateMetadataContainer_WhenTypeIsGeneric_PreservesTheCSharpTypeName()
    {
        // Given
        Type genericType = typeof(Dictionary<string, object>);

        // When
        MetadataContainer result = metadataBroker.CreateMetadataContainer(
            type: genericType,
            isEntity: false,
            hasEndpoint: false);

        // Then
        result.ServerTypeName.Should()
            .Be(expected: "Dictionary<String,Object>");
    }

    [Fact]
    public void CreateMetadataContainer_WhenTypeIsAJoinEntity_IdentifiesTheJoinEntity()
    {
        // Given
        Type joinType = typeof(TestJoinEntity);

        // When
        MetadataContainer result = metadataBroker.CreateMetadataContainer(
            type: joinType,
            isEntity: true,
            hasEndpoint: true);

        // Then
        result.IsJoinEntity.Should()
            .BeTrue();
    }

    [Table("TestJoinEntities")]
    private sealed class TestJoinEntity
    {
        [ForeignKey(nameof(FirstId))]
        public int FirstId { get; set; }

        [ForeignKey(nameof(SecondId))]
        public int SecondId { get; set; }

        [ForeignKey(nameof(ThirdId))]
        public int ThirdId { get; set; }

        [ForeignKey(nameof(FourthId))]
        public int FourthId { get; set; }
    }
}