// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System;
using System.Text.Json;
using cCoder.Data.Exposures;
using cCoder.Packaging.Services.Foundations;


namespace cCoder.Packaging;

public static class WebApplicationExtensions
{
    private const string MetadataScope = "Packaging";

    public static WebApplication UsePackagingExposure(this WebApplication app, ILogger log = null)
    {
        log?.LogInformation(message:"Initialising Packaging");
        PopulateMetadataTypeCache(app:app);
        return app;
    }

    private static void PopulateMetadataTypeCache(WebApplication app)
    {
        IMetadataTypeCache metadataTypeCache = app.Services.GetRequiredService<IMetadataTypeCache>();

        if (!metadataTypeCache.Contains(scope:MetadataScope))
        {
            metadataTypeCache.Set(
scope:                MetadataScope,
typeSetPayloads:                app.Services
                    .GetRequiredService<IPackagingMetadataTypeService>()
                    .GetKnownMetadata()
                    .Select(selector:static metadata => JsonSerializer.Serialize(value:metadata)));
        }
    }
}