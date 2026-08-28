// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models;
using cCoder.Eventing.Models;
using cCoder.Packaging.Models;
using cCoder.Security.Models;

namespace Packaging.Web.Models;

public sealed class AppConfiguration
{
    public AppConfiguration()
    {
        CoreData = new CoreDataConfiguration();
        Eventing = new EventingConfiguration();
        Packaging = new PackagingConfiguration();
        Security = new SecurityConfiguration();
        SecurityData = new SecurityDataConfiguration();
    }

    public CoreDataConfiguration CoreData { get; set; }

    public EventingConfiguration Eventing { get; set; }

    public PackagingConfiguration Packaging { get; set; }

    public SecurityConfiguration Security { get; set; }

    public SecurityDataConfiguration SecurityData { get; set; }
}