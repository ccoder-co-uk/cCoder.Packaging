// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using cCoder.Packaging.Api.OData;

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
        bool isValueType = type.IsValueType || type == typeof(string);
        PropertyContainer[] properties = isValueType
            ? []
            : type.GetProperties()
                .Select(selector: CreatePropertyContainer)
                .ToArray();

        return new MetadataContainer
        {
            IsValueType = isValueType,
            Type = GetTypeName(type: type),
            Name = type.Name,
            DisplayName = type.Name,
            Description = type.Name,
            ServerType = type.AssemblyQualifiedName,
            ServerTypeName = type.GetCSharpTypeName(),
            Properties = properties,
            IsEntity = isEntity,
            IsJoinEntity = isEntity && type.IsJoinType(),
            HasEndpoint = hasEndpoint,
        };
    }

    private static PropertyContainer CreatePropertyContainer(PropertyInfo property) =>
        new()
        {
            Name = property.Name,
            Type = GetTypeName(type: property.PropertyType),
            ServerType = property.PropertyType.ToString(),
            ServerTypeName = property.PropertyType.GetCSharpTypeName(),
            IsValueType = property.PropertyType.IsValueType || property.PropertyType == typeof(string),
            DisplayName = property.Name,
            ShortDisplayName = property.Name,
            Description = property.Name,
            IsReadOnly = !property.CanWrite,
            Template = property.GetCustomAttribute<KeyAttribute>() != null || property.Name == "Id"
                ? "key"
                : property.Name,
            IsRequired = (!(property.PropertyType.IsGenericType
                    && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                && property.PropertyType.IsValueType)
                || property.GetCustomAttribute<RequiredAttribute>() != null,
        };

    private static string GetTypeName(Type type)
    {
        Func<string>[] typeNameSelectors =
        [
            () => Lookup.TryGetValue(key: type, out string name) ? name : "object",
            () => "array",
            () => "string",
        ];

        int selectorIndex = Convert.ToInt32(typeof(IEnumerable).IsAssignableFrom(c: type));
        selectorIndex = Convert.ToInt32(type == typeof(string)) * 2 + selectorIndex;

        return typeNameSelectors[selectorIndex]();
    }
}