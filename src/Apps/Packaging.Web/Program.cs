// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

namespace Packaging.Web;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args: args);
        builder.Services.AddPackagingWebApplication(configuration: builder.Configuration);

        WebApplication app = builder.Build();
        app.UsePackagingWebApplication();
        app.Run();
    }
}