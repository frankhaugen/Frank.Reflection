// This code is copied from Namotion.Reflection and modified:
//-----------------------------------------------------------------------
// <copyright file="ReflectionExtensions.cs" company="NSwag">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/NSwag/NSwag/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

namespace Frank.Reflection;

/// <summary>Provides extension methods for reflection.</summary>
public static class TypeExtensions
{
    /// <summary>Gets a human readable type name (e.g. DictionaryOfStringAndObject).</summary>
    /// <param name="type">The type.</param>
    /// <returns>The type name.</returns>
    public static string GetDisplayName(this Type type)
    {
        var nType = type;
        if (nType.IsConstructedGenericType)
        {
            return GetName(nType).FirstToken('`') + "Of" +
                   string.Join("And", nType.GenericTypeArguments
                                           .Select(GetDisplayName));
        }

        return GetName(nType);
    }

    private static string GetName(Type type)
    {
        return
            type.Name == "Int16" ? GetNullableDisplayName(type, "Short") :
            type.Name == "Int32" ? GetNullableDisplayName(type, "Integer") :
            type.Name == "Int64" ? GetNullableDisplayName(type, "Long") :
            GetNullableDisplayName(type, type.Name);
    }

    private static string GetNullableDisplayName(Type type, string actual)
    {
        return (IsNullableType(type) ? "Nullable" : "") + actual;
    }

    private static bool IsNullableType(Type type)
    {
        return type.IsConstructedGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
    }
}