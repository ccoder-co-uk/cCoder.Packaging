// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Reflection;
using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Brokers.PackageManagers;
using cCoder.Packaging.Exposures.PackageManagers;
using FluentAssertions;
using Xunit;

namespace cCoder.Packaging.Tests.Brokers.PackageManagers;

public sealed partial class PackageManagerBrokerTests
{
    public static TheoryData<Type, Type> BrokerTypes => new()
    {
        { typeof(IAppSecurityPackageManager), typeof(AppSecurityPackageBroker) },
        { typeof(IContentManagementPackageManager), typeof(ContentManagementPackageBroker) },
        { typeof(IDocumentManagementPackageManager), typeof(DocumentManagementPackageBroker) },
        { typeof(ISchedulingPackageManager), typeof(SchedulingPackageBroker) },
        { typeof(IWorkflowPackageManager), typeof(WorkflowPackageBroker) },
    };

    [Theory]
    [MemberData(nameof(BrokerTypes))]
    public async Task PackageManagerBroker_WhenImporting_ForwardsTheExactRequest(
        Type managerType,
        Type brokerType)
    {
        // Given
        const int appId = 241;
        Package package = new() { Id = Guid.NewGuid(), Name = "Import" };
        RecordingPackageManager proxy = CreateProxy(managerType: managerType);

        object broker = Activator.CreateInstance(
            type: brokerType,
            args: [proxy]);

        MethodInfo method = brokerType.GetMethod(name: "ImportPackageAsync");

        // When
        ValueTask operation = (ValueTask)method.Invoke(
            obj: broker,
            parameters: [appId, package]);

        await operation;

        // Then
        proxy.AppId.Should()
            .Be(expected: appId);

        proxy.Package.Should()
            .BeSameAs(expected: package);

        proxy.ImportCallCount.Should()
            .Be(expected: 1);
    }

    [Theory]
    [MemberData(nameof(BrokerTypes))]
    public void PackageManagerBroker_WhenExporting_ForwardsAndReturnsTheExactPackage(
        Type managerType,
        Type brokerType)
    {
        // Given
        const int appId = 242;
        const string packageName = "Pages";
        Package expectedPackage = new() { Id = Guid.NewGuid(), Name = packageName };
        RecordingPackageManager proxy = CreateProxy(managerType: managerType);
        proxy.ExportResult = expectedPackage;

        object broker = Activator.CreateInstance(
            type: brokerType,
            args: [proxy]);

        MethodInfo method = brokerType.GetMethod(name: "ExportPackage");

        // When
        Package actualPackage = (Package)method.Invoke(
            obj: broker,
            parameters: [appId, packageName]);

        // Then
        actualPackage.Should()
            .BeSameAs(expected: expectedPackage);

        proxy.AppId.Should()
            .Be(expected: appId);

        proxy.PackageName.Should()
            .Be(expected: packageName);

        proxy.ExportCallCount.Should()
            .Be(expected: 1);
    }

    private static RecordingPackageManager CreateProxy(Type managerType) =>
        (RecordingPackageManager)DispatchProxy.Create(
            interfaceType: managerType,
            proxyType: typeof(RecordingPackageManager));

    public class RecordingPackageManager : DispatchProxy
    {
        public int AppId { get; private set; }
        public Package Package { get; private set; }
        public string PackageName { get; private set; }
        public Package ExportResult { get; set; }
        public int ImportCallCount { get; private set; }
        public int ExportCallCount { get; private set; }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            AppId = (int)args[0];

            if (targetMethod.Name == "ImportPackageAsync")
            {
                Package = (Package)args[1];
                ImportCallCount++;

                return ValueTask.CompletedTask;
            }

            PackageName = (string)args[1];
            ExportCallCount++;

            return ExportResult;
        }
    }
}