// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Api.OData;
using cCoder.Packaging.Models;
using Microsoft.OData.ModelBuilder;

namespace cCoder.Packaging.Brokers.OData;

internal sealed class ODataModelBroker : IODataModelBroker
{
    public void ConfigureODataModel(ODataConventionModelBuilder builder)
    {
        builder.ComplexType<MetadataContainerSet>();
        builder.ComplexType<MetadataContainer>();
        builder.ComplexType<PropertyContainer>();
        builder.ComplexType<AuditResultsByUser>();
        builder.ComplexType<AuditResultByProperty>();
        builder.EntitySet<Package>(name: nameof(Package));
        builder.EntitySet<PackageItem>(name: nameof(PackageItem));
        builder.Namespace = "";
    }
}