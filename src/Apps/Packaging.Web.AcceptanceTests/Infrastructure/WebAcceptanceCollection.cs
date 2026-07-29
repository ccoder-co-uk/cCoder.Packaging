// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using Xunit;

namespace Packaging.Web.AcceptanceTests.Infrastructure;

[CollectionDefinition(Name)]
public sealed class WebAcceptanceCollection : ICollectionFixture<WebAcceptanceFixture>
{
    public const string Name = "Web acceptance";
}