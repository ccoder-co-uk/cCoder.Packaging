// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using Microsoft.AspNetCore.Http;

namespace cCoder.Packaging.Brokers.HttpContexts;

internal sealed class HttpContextBroker(IHttpContextAccessor httpContextAccessor)
    : IHttpContextBroker
{
    public string GetRequestDomain() =>
        httpContextAccessor.HttpContext?.Request.Host.Host ?? "localhost";
}