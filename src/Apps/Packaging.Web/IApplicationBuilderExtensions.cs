// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Security;
using cCoder.Packaging;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.OData;

namespace Packaging.Web;

public static class IApplicationBuilderExtensions
{
    public static IApplicationBuilder UsePackagingWebApplication(
        this WebApplication app)
    {
        app.UseHttpsRedirection();
        app.UseSession();
        app.UseStaticFiles();

        app.UseSwagger()
            .UseSwaggerUI(setupAction: options =>
            {
                options.SwaggerEndpoint(url: "/swagger/Packaging/swagger.json", name: "Packaging API");
                options.SwaggerEndpoint(url: "/swagger/Core/swagger.json", name: "Core API");
                options.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "Core API");
            })
            .UseODataBatching()
            .UseODataRouteDebug();

        app.MapGet(pattern: "/Health", handler: () => Results.Text(content: "OK"));
        app.MapGet(pattern: "/", handler: () => Results.Redirect(url: "/tools/index.html"));
        app.UseRouting();
        app.MapControllers();

        ILogger log = app.Services.GetRequiredService<ILogger<Program>>();
        app.UsePackagingExposure(log: log);

        app.UseExceptionHandler(configure: errorApplication =>
        {
            errorApplication.Run(handler: HandleUnhandledException);
        });

        return app;
    }

    private static async Task HandleUnhandledException(HttpContext context)
    {
        Exception exception = context.Features.Get<IExceptionHandlerPathFeature>()?.Error;

        context.Response.StatusCode =
            exception?.GetType() == typeof(SecurityException) ? 401 : 500;

        context.Response.ContentType = "application/json";

        if (exception is null)
        {
            return;
        }

        ILogger<Program> log = context.RequestServices
            .GetRequiredService<ILogger<Program>>();

        log.LogError(
            message: "{Message}\n{StackTrace}",
            args:
            [
                exception.Message,
                exception.StackTrace,
            ]);

        string response =
            "{ \"error\": \"" + exception.Message.Replace(oldValue: "\"", newValue: "'") + "\" }";

        await context.Response.WriteAsync(text: response);
    }
}