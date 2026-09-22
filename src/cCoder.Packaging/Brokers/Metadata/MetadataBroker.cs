// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using cCoder.Packaging.Api.OData;
using cCoder.Packaging.Dependencies.OData;

namespace cCoder.Packaging.Brokers.Metadata;

internal sealed class MetadataBroker : IMetadataBroker
{
    private static readonly Dictionary<Type, string> Lookup = new()
    {
        { typeof(short), "number" },
        { typeof(int), "number" },
        { typeof(long), "number" },
        { typeof(short?), "number" },
        { typeof(int?), "number" },
        { typeof(long?), "number" },
        { typeof(ushort), "number" },
        { typeof(uint), "number" },
        { typeof(ulong), "number" },
        { typeof(ushort?), "number" },
        { typeof(uint?), "number" },
        { typeof(ulong?), "number" },
        { typeof(byte), "number" },
        { typeof(byte?), "number" },
        { typeof(decimal), "number" },
        { typeof(decimal?), "number" },
        { typeof(string), "string" },
        { typeof(DateTime), "date" },
        { typeof(DateTime?), "date" },
        { typeof(TimeSpan), "time" },
        { typeof(TimeSpan?), "time" },
        { typeof(DateTimeOffset), "date" },
        { typeof(DateTimeOffset?), "date" },
        { typeof(Guid), "guid" },
        { typeof(Guid?), "guid" },
        { typeof(bool), "bool" },
        { typeof(bool?), "bool" },
        { typeof(double), "number" },
        { typeof(double?), "number" },
        { typeof(float), "number" },
        { typeof(float?), "number" },
    };

    public MetadataContainer CreateMetadataContainer(
        Type type,
        bool isEntity,
        bool hasEndpoint)
    {
        bool isValueType = type.IsValueType | type == typeof(string);

        Func<PropertyContainer[]>[] propertySelectors =
        [
            () => type.GetProperties()
                .Select(selector: CreatePropertyContainer)
                .ToArray(),
            () => [],
        ];

        PropertyContainer[] properties =
            propertySelectors[Convert.ToInt32(value: isValueType)]
                .Invoke();

        return new MetadataContainer
        {
            IsValueType = isValueType,
            Type = GetTypeName(type: type),
            Name = type.Name,
            DisplayName = type.Name,
            Description = type.Name,
            ServerType = type.AssemblyQualifiedName,
            ServerTypeName = GetCSharpTypeName(type: type),
            Properties = properties,
            IsEntity = isEntity,
            IsJoinEntity = isEntity & IsJoinType(type: type),
            HasEndpoint = hasEndpoint,
        };
    }

    private static PropertyContainer CreatePropertyContainer(PropertyInfo property)
    {
        Func<bool>[] nullableTypeSelectors =
        [
            () => false,
            () => property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>),
        ];

        bool isNullableType =
            nullableTypeSelectors[Convert.ToInt32(value: property.PropertyType.IsGenericType)]
                .Invoke();

        return new PropertyContainer
        {
            Name = property.Name,
            Type = GetTypeName(type: property.PropertyType),
            ServerType = property.PropertyType.ToString(),
            ServerTypeName = GetCSharpTypeName(type: property.PropertyType),
            IsValueType = property.PropertyType.IsValueType | property.PropertyType == typeof(string),
            DisplayName = property.Name,
            ShortDisplayName = property.Name,
            Description = property.Name,
            IsReadOnly = !property.CanWrite,
            Template = new[]
            {
                property.Name,
                "key",
            }[
                Convert.ToInt32(
                    value: property.GetCustomAttribute<KeyAttribute>() != null
                        | property.Name == "Id")],
            IsRequired = (!isNullableType & property.PropertyType.IsValueType)
                | property.GetCustomAttribute<RequiredAttribute>() != null,
        };
    }

    private static string GetTypeName(Type type)
    {
        Func<string>[] typeNameSelectors =
        [
            () => Lookup.GetValueOrDefault(key: type, defaultValue: "object"),
            () => "array",
            () => "string",
        ];

        bool isString = type == typeof(string);
        bool isEnumerable = typeof(IEnumerable).IsAssignableFrom(c: type);
        int selectorIndex = Convert.ToInt32(value: isString) * 2;
        selectorIndex += Convert.ToInt32(value: isEnumerable & !isString);

        return typeNameSelectors[selectorIndex].Invoke();
    }

    private static string GetCSharpTypeName(Type type)
    {
        Func<string>[] typeNameSelectors =
        [
            () => type.Name,
            () =>
            {
                IEnumerable<string> genericNames = type.GenericTypeArguments
                    .Select(selector: argument => GetCSharpTypeName(type: argument));

                return $"{type.Name.Split(separator: '`')[0]}<{string.Join(separator: ",", values: genericNames)}>"
                    .Replace(oldValue: "System.Object", newValue: "dynamic");
            },
        ];

        return typeNameSelectors[Convert.ToInt32(value: type.IsGenericType)]();
    }

    private static bool IsJoinType(Type type)
    {
        TableAttribute table = type.GetCustomAttribute<TableAttribute>();

        return table != null
            && type.GetProperties().Length == 4
            && type.GetProperties()
                .Where(predicate: property =>
                    property.PropertyType.IsValueType
                    || property.PropertyType == typeof(string))
                .All(predicate: property =>
                    property.GetCustomAttribute<ForeignKeyAttribute>() != null);
    }
}