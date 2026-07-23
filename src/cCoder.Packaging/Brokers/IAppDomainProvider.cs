// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

namespace cCoder.Packaging.Brokers;

public interface IAppDomainProvider
{
    string GetDomain(int appId);
}