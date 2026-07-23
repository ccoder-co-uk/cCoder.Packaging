// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Packaging.Web.AcceptanceTests.Infrastructure;

public sealed class WebAcceptanceFixture : IAsyncLifetime
{
    internal WebAcceptanceFactory Factory { get; private set; } = null!;

    public HttpClient Client { get; private set; } = null!;

    public Task InitializeAsync()
    {
        Factory = new WebAcceptanceFactory();

        Client = Factory.CreateClient(options: new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
            BaseAddress = new Uri("https://localhost"),
        });

        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        Client?.Dispose();

        if (Factory is not null)
        {
            await Factory.DisposeAsync();
        }
    }
}

[CollectionDefinition(Name)]
public sealed class WebAcceptanceCollection : ICollectionFixture<WebAcceptanceFixture>
{
    public const string Name = "Web acceptance";
}