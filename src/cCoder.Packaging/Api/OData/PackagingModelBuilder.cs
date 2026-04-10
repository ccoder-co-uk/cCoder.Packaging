using cCoder.Packaging.Models;
using cCoder.Data.Models.Packaging;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace cCoder.Packaging.Api.OData;

internal class PackagingModelBuilder : ODataModelBuilder
{
    public PackagingModelBuilder(ODataConventionModelBuilder builder = null)
        : base(builder)
    {
    }

    public override ODataModel Build()
    {
        return new ODataModel
        {
            Context = "Core",
            Description = "Packaging endpoints for the platform.",
            EDMModel = BuildEdmModel()
        };
    }

    public void Configure()
    {
        ConfigureModel();
    }

    private IEdmModel BuildEdmModel()
    {
        ConfigureModel();
        return base.Builder.GetEdmModel();
    }

    private void ConfigureModel()
    {
        AddCommonComplextypes();
        AddSet<Package, Guid>();
        AddSet<PackageItem, Guid>();
        base.Builder.Namespace = "";
    }
}
