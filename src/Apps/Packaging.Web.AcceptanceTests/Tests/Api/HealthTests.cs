// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Net.Http;
using Xunit;
using Packaging.Web.AcceptanceTests.Infrastructure;

namespace Packaging.Web.AcceptanceTests.Tests.Api;

[Collection(WebAcceptanceCollection.Name)]
public sealed partial class HealthTests(WebAcceptanceFixture fixture)
{
    private readonly HttpClient client = fixture.Client;
}