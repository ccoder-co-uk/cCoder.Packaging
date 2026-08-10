// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Brokers.Loggings;
using cCoder.Packaging.Services.Foundations.Loggings;
using Moq;

namespace cCoder.Packaging.Tests.Foundations.Loggings;

public partial class LoggingFoundationServiceTests
{
    private readonly Mock<ILoggingBroker> loggingBrokerMock = new();
    private readonly LoggingFoundationService loggingFoundationService;

    public LoggingFoundationServiceTests() =>
        loggingFoundationService = new LoggingFoundationService(
            loggingBroker: loggingBrokerMock.Object);
}