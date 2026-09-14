// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Exposures.PackageManagers;
using Microsoft.AspNetCore.Http;

<<<<<<<< HEAD:src/cCoder.Packaging/Brokers/HttpContexts/HttpContextBroker.cs
namespace cCoder.Packaging.Brokers.HttpContexts;

internal sealed class HttpContextBroker(IHttpContextAccessor httpContextAccessor)
    : IHttpContextBroker
========
namespace cCoder.Packaging.Dependencies.AspNet;

internal sealed class AppDomainDependency(IHttpContextAccessor httpContextAccessor)
    : IAppDomainManager
>>>>>>>> 950a129 (Align Packaging with strict architecture rules):src/cCoder.Packaging/Dependencies/AspNet/AppDomainDependency.cs
{
    public string GetRequestDomain() =>
        httpContextAccessor.HttpContext?.Request.Host.Host ?? "localhost";
}