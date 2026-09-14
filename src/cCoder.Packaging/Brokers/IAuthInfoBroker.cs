// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

namespace cCoder.Packaging.Brokers;

using cCoder.CodeAnalysis.Exposures;

internal interface IAuthInfoBroker : IUtilityBroker
{
    string GetSSOUserId();
}