// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

#pragma warning disable STXFORMAT005, STXFORMAT009, STXTEST005

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Xunit;

namespace cCoder.Packaging.Tests;

public sealed partial class ControllerActionContractTests
{
    private static readonly Assembly ProductAssembly = typeof(Exposures.Controllers.PackageController).Assembly;

    public static IEnumerable<object[]> Actions => ProductAssembly.GetTypes()
        .Where(type => type.Namespace == "cCoder.Packaging.Exposures.Controllers")
        .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
        .Where(IsAction)
        .Select(method => new object[] { method });

    public static IEnumerable<object[]> HappyGapActions => Actions
        .Where(data => !HasExistingHappyEvidence(action: (MethodInfo)data[0]));

    public static IEnumerable<object[]> ModelStateActions => new[]
    {
        "PackageController.Post/1",
        "PackageController.PostImport/2",
        "PackageController.Put/2",
        "PackageItemController.Post/1",
        "PackageItemController.Put/2",
    }.Select(actionName => new object[] { FindAction(actionName: actionName) });

    public static IEnumerable<object[]> NotFoundActions => new[]
    {
        "PackageController.Get/1",
        "PackageItemController.Get/1",
    }.Select(actionName => new object[] { FindAction(actionName: actionName) });

    [Fact]
    public void Inventory_ShouldContainFourteenActions() =>
        Assert.Equal(expected: 14, actual: Actions.Count());

    [Theory]
    [MemberData(nameof(HappyGapActions))]
    public async Task Action_WhenDependencyCompletes_ShouldReturnSuccessAndLogInformation(MethodInfo action)
    {
        // Given / When
        Outcome outcome = await Invoke(action: action);

        // Then
        int? statusCode = (outcome.Result as IStatusCodeActionResult)?.StatusCode;
        Assert.True(condition: statusCode is null or >= 200 and < 400);
        Assert.Equal(expected: 1, actual: outcome.Logging.InformationCount);
        Assert.Equal(expected: 0, actual: outcome.Logging.WarningCount);
        Assert.Equal(expected: 0, actual: outcome.Logging.ErrorCount);
    }

    [Theory]
    [MemberData(nameof(Actions))]
    public Task Action_WhenDependencyRejectsRequest_ShouldReturn400AndWarn(MethodInfo action) =>
        AssertFailure(
            action: action,
            failure: CreatePackagingValidationException(),
            expectedStatus: 400,
            warningCount: 1,
            errorCount: 0);

    [Theory]
    [MemberData(nameof(Actions))]
    public Task Action_WhenDependencyRejectsAuthentication_ShouldReturn401AndWarn(MethodInfo action) =>
        AssertFailure(
            action: action,
            failure: new AuthenticationException(message: "Sensitive authentication detail."),
            expectedStatus: 401,
            warningCount: 1,
            errorCount: 0);

    [Theory]
    [MemberData(nameof(Actions))]
    public Task Action_WhenDependencyRejectsAuthorization_ShouldReturn403AndWarn(MethodInfo action) =>
        AssertFailure(
            action: action,
            failure: new SecurityException(message: "Sensitive authorization detail."),
            expectedStatus: 403,
            warningCount: 1,
            errorCount: 0);

    [Theory]
    [MemberData(nameof(Actions))]
    public Task Action_WhenDependencyFails_ShouldReturnDetailFree500AndLogError(MethodInfo action) =>
        AssertFailure(
            action: action,
            failure: new InvalidOperationException(message: "Sensitive unexpected detail."),
            expectedStatus: 500,
            warningCount: 0,
            errorCount: 1);

    [Theory]
    [MemberData(nameof(ModelStateActions))]
    public async Task Action_WhenModelStateIsInvalid_ShouldReturn400AndWarn(MethodInfo action)
    {
        // Given / When
        Outcome outcome = await Invoke(action: action, invalidModelState: true);

        // Then
        AssertClientError(outcome: outcome, expectedStatus: 400);
    }

    [Theory]
    [MemberData(nameof(NotFoundActions))]
    public async Task Action_WhenEntityIsMissing_ShouldReturn404AndWarn(MethodInfo action)
    {
        // Given / When
        Outcome outcome = await Invoke(action: action, emptyQuery: true);

        // Then
        AssertClientError(outcome: outcome, expectedStatus: 404);
    }

    private static bool IsAction(MethodInfo method)
    {
        Type returnType = method.ReturnType;

        return typeof(IActionResult).IsAssignableFrom(c: returnType)
            || returnType.IsGenericType
            && typeof(IActionResult).IsAssignableFrom(c: returnType.GetGenericArguments()[0]);
    }

    private static bool HasExistingHappyEvidence(MethodInfo action) =>
        action.DeclaringType!.Name == "PackageController"
        && (action.Name == "Get" && action.GetParameters().Length == 2
            || action.Name is "Post" or "PostImport");

    private static MethodInfo FindAction(string actionName)
    {
        string[] signature = actionName.Split('/');
        string[] parts = signature[0].Split('.');
        int parameterCount = int.Parse(signature[1]);

        return Actions
            .Select(data => (MethodInfo)data[0])
            .Single(action => action.DeclaringType!.Name == parts[0]
                && action.Name == parts[1]
                && action.GetParameters().Length == parameterCount);
    }

    private static Exception CreatePackagingValidationException()
    {
        Type exceptionType = ProductAssembly.GetType(
            name: "cCoder.Packaging.Models.Exceptions.PackagingOrchestrationValidationException")!;

        return (Exception)Activator.CreateInstance(
            type: exceptionType,
            bindingAttr: BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
            binder: null,
            args: new object[] { new Exception(message: "Sensitive validation detail.") },
            culture: null)!;
    }

    private static async Task AssertFailure(
        MethodInfo action,
        Exception failure,
        int expectedStatus,
        int warningCount,
        int errorCount)
    {
        // Given / When
        Outcome outcome = await Invoke(action: action, failure: failure);

        // Then
        Assert.Equal(expected: expectedStatus, actual: (outcome.Result as IStatusCodeActionResult)?.StatusCode);
        Assert.Equal(expected: 0, actual: outcome.Logging.InformationCount);
        Assert.Equal(expected: warningCount, actual: outcome.Logging.WarningCount);
        Assert.Equal(expected: errorCount, actual: outcome.Logging.ErrorCount);
        string payload = (outcome.Result as ObjectResult)?.Value?.ToString() ?? string.Empty;
        Assert.DoesNotContain(expectedSubstring: "Sensitive", actualString: payload, comparisonType: StringComparison.OrdinalIgnoreCase);
    }

    private static void AssertClientError(Outcome outcome, int expectedStatus)
    {
        Assert.Equal(expected: expectedStatus, actual: (outcome.Result as IStatusCodeActionResult)?.StatusCode);
        Assert.Equal(expected: 0, actual: outcome.Logging.InformationCount);
        Assert.Equal(expected: 1, actual: outcome.Logging.WarningCount);
        Assert.Equal(expected: 0, actual: outcome.Logging.ErrorCount);
    }

    private static async Task<Outcome> Invoke(
        MethodInfo action,
        Exception failure = null,
        bool invalidModelState = false,
        bool emptyQuery = false)
    {
        ConstructorInfo constructor = action.DeclaringType!.GetConstructors().Single();
        ContractProxy logging = null;

        object[] dependencies = constructor.GetParameters()
            .Select(parameter =>
            {
                bool isLogging = parameter.ParameterType.FullName == "cCoder.Packaging.Brokers.Loggings.ILoggingBroker";
                ContractProxy proxy = CreateProxy(type: parameter.ParameterType);
                proxy.Failure = isLogging ? null : failure;
                proxy.EmptyQuery = !isLogging && emptyQuery;

                if (isLogging)
                {
                    logging = proxy;
                }

                return (object)proxy;
            })
            .ToArray();

        object controller = constructor.Invoke(parameters: dependencies);

        if (controller is ControllerBase controllerBase)
        {
            controllerBase.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext(),
            };

            if (invalidModelState)
            {
                controllerBase.ModelState.AddModelError(key: "contract", errorMessage: "Invalid request.");
            }
        }

        object[] arguments = action.GetParameters()
            .Select(parameter => Value(type: parameter.ParameterType, emptyQuery: false))
            .ToArray();

        object invocation = action.Invoke(obj: controller, parameters: arguments)!;
        IActionResult result = await Resolve(invocation: invocation);

        return new Outcome(Result: result, Logging: logging!);
    }

    private static ContractProxy CreateProxy(Type type) =>
        (ContractProxy)DispatchProxy.Create(interfaceType: type, proxyType: typeof(ContractProxy));

    private static object Value(Type type, bool emptyQuery)
    {
        if (type == typeof(string))
        {
            return "contract-value";
        }

        Type nullableType = Nullable.GetUnderlyingType(nullableType: type);

        if (nullableType is not null)
        {
            return Value(type: nullableType, emptyQuery: emptyQuery);
        }

        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IQueryable<>))
        {
            Type elementType = type.GetGenericArguments()[0];
            Array values = Array.CreateInstance(elementType: elementType, length: emptyQuery ? 0 : 1);

            if (!emptyQuery)
            {
                values.SetValue(value: Activator.CreateInstance(elementType), index: 0);
            }

            return typeof(Queryable)
                .GetMethods()
                .Single(method => method.Name == nameof(Queryable.AsQueryable)
                    && method.IsGenericMethodDefinition
                    && method.GetParameters().Length == 1)
                .MakeGenericMethod(elementType)
                .Invoke(obj: null, parameters: new object[] { values })!;
        }

        if (type == typeof(Task))
        {
            return Task.CompletedTask;
        }

        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Task<>))
        {
            Type resultType = type.GetGenericArguments()[0];
            object result = Value(type: resultType, emptyQuery: emptyQuery);

            return typeof(Task)
                .GetMethod(nameof(Task.FromResult))!
                .MakeGenericMethod(resultType)
                .Invoke(obj: null, parameters: new[] { result })!;
        }

        if (type.IsArray)
        {
            return Array.CreateInstance(elementType: type.GetElementType()!, length: 0);
        }

        if (type.IsValueType)
        {
            return Activator.CreateInstance(type)!;
        }

        return Activator.CreateInstance(type)!;
    }

    private static async Task<IActionResult> Resolve(object invocation)
    {
        if (invocation is IActionResult result)
        {
            return result;
        }

        Task task = (Task)invocation;
        await task;

        return (IActionResult)task.GetType().GetProperty("Result")!.GetValue(obj: task)!;
    }

    public class ContractProxy : DispatchProxy
    {
        public Exception Failure { get; set; }
        public bool EmptyQuery { get; set; }
        public int InformationCount { get; private set; }
        public int WarningCount { get; private set; }
        public int ErrorCount { get; private set; }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            if (targetMethod.Name == "LogInformation")
            {
                InformationCount++;
                return null;
            }

            if (targetMethod.Name == "LogWarning")
            {
                WarningCount++;
                return null;
            }

            if (targetMethod.Name == "LogError")
            {
                ErrorCount++;
                return null;
            }

            if (Failure is not null)
            {
                throw Failure;
            }

            return Value(type: targetMethod.ReturnType, emptyQuery: EmptyQuery);
        }
    }

    private sealed record Outcome(IActionResult Result, ContractProxy Logging);
}