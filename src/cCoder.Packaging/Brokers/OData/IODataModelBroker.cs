// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using Microsoft.OData.ModelBuilder;

namespace cCoder.Packaging.Brokers.OData;

internal interface IODataModelBroker
{
    void ConfigureODataModel(ODataConventionModelBuilder builder);
}