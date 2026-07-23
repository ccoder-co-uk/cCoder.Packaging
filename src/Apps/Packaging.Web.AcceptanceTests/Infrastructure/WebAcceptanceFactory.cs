// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace Packaging.Web.AcceptanceTests.Infrastructure;

internal sealed class WebAcceptanceFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment(environment: "Acceptance");

        builder.ConfigureAppConfiguration(configureDelegate: (_, configuration) =>
        {
            configuration.AddInMemoryCollection(
initialData: [
                new KeyValuePair<string, string>("ConnectionStrings:Core", "Data Source=.;Initial Catalog=packaging-acceptance;Trusted_Connection=True;Trust Server Certificate=true;Encrypt=True"),
                new KeyValuePair<string, string>("ConnectionStrings:SSO", "Data Source=.;Initial Catalog=sso-acceptance;Trusted_Connection=True;Trust Server Certificate=true;Encrypt=True"),
                new KeyValuePair<string, string>("Settings:DecryptionKey", "000000000000000000000000000000000000000000000000"),
                new KeyValuePair<string, string>("Settings:enableExternalEventing", "false"),
            ]);
        });
    }
}