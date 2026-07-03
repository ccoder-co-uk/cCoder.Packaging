using Microsoft.AspNetCore.Http;

namespace cCoder.Packaging.Brokers;

internal class AppDomainProvider(IHttpContextAccessor httpContextAccessor) : IAppDomainProvider
{
    public string GetDomain(int appId) =>
        httpContextAccessor.HttpContext?.Request.Host.Host ?? "localhost";
}
