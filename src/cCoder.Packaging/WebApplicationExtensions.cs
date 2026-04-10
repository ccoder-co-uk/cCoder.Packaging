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
        log?.LogInformation("Initialising Packaging");
        PopulateMetadataTypeCache(app);
        return app;
    }

    private static void PopulateMetadataTypeCache(WebApplication app)
    {
        IMetadataTypeCache metadataTypeCache = app.Services.GetRequiredService<IMetadataTypeCache>();

        if (!metadataTypeCache.Contains(MetadataScope))
        {
            metadataTypeCache.Set(
                MetadataScope,
                app.Services
                    .GetRequiredService<IPackagingMetadataTypeService>()
                    .GetKnownMetadata()
                    .Select(static metadata => JsonSerializer.Serialize(metadata)));
        }
    }
}
