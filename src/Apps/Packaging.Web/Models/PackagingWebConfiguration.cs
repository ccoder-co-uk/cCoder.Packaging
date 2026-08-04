// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Eventing.Models;
using cCoder.Packaging.Models;
using cCoder.Security.Models;

namespace Packaging.Web.Models;

public sealed class PackagingWebConfiguration
{
    public EventingConfiguration Eventing { get; set; }
    public PackagingConfiguration Packaging { get; set; }
    public SecurityConfiguration Security { get; set; }
}