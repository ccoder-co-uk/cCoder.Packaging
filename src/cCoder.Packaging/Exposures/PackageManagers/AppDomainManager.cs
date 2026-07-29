// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using Microsoft.AspNetCore.Http;

namespace cCoder.Packaging.Exposures.PackageManagers;

internal class AppDomainManager(IHttpContextAccessor httpContextAccessor) : IAppDomainManager
{
    public string GetDomain(int appId) =>
        httpContextAccessor.HttpContext?.Request.Host.Host ?? "localhost";
}