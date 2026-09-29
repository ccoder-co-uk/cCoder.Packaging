// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using cCoder.Packaging.Api.OData;
using cCoder.Packaging.Brokers.Metadata;
using cCoder.Packaging.Services.Foundations.Metadata;
using FluentAssertions;
using Xunit;

namespace cCoder.Packaging.Tests.Foundations.Metadata;

public sealed partial class MetadataServiceTests
{
    private readonly MetadataService metadataService = new(
        metadataBroker: new MetadataBroker());

    [Fact]
    public void CreateMetadataContainer_WhenTypeIsGeneric_PreservesTheCSharpTypeName()
    {
        // Given
        Type genericType = typeof(Dictionary<string, object>);

        // When
        MetadataContainer result = metadataService.CreateMetadataContainer(
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
        MetadataContainer result = metadataService.CreateMetadataContainer(
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