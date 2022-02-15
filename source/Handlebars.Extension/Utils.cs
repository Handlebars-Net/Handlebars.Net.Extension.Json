using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;
using HandlebarsDotNet.Runtime;

namespace HandlebarsDotNet.Extension.Json
{
    internal static partial class Utils
    {
        private static readonly string[] ArrayProperties = { "length" };

        public static IEnumerable GetEnumerator(JsonElement document)
        {
            return document.ValueKind switch
            {
                JsonValueKind.Object => EnumerateObject(),
                JsonValueKind.Array => ArrayProperties,
                _ => Throw.ArgumentOutOfRangeException<IEnumerable>()
            };

            IEnumerable<string> EnumerateObject()
            {
                foreach (var property in document.EnumerateObject())
                {
                    yield return property.Name;
                }
            }
        }

        public static object? ExtractProperty(JsonElement property)
        {
            switch (property.ValueKind)
            {
                case JsonValueKind.Object:
                case JsonValueKind.Array:
                    return property;

                case JsonValueKind.String when property.TryGetDateTime(out var v):
                    return v;
                
                case JsonValueKind.String when property.TryGetGuid(out var v):
                    return v;
                
                case JsonValueKind.String when property.TryGetDateTimeOffset(out var v):
                    return v;
                
                case JsonValueKind.String:
                    return property.GetString();

                case JsonValueKind.Number when property.TryGetInt32(out var v):
                    return v;
                
                case JsonValueKind.Number when property.TryGetInt64(out var v):
                    return v;

                case JsonValueKind.Number when property.TryGetDecimal(out var v):
                    return v;
                
                case JsonValueKind.Number:
                    return property.GetDouble();

                case JsonValueKind.True:
                    return BoxedValues.True;

                case JsonValueKind.False:
                    return BoxedValues.False;

                case JsonValueKind.Undefined:
                case JsonValueKind.Null:
                    return null;

                default:
                    return Throw.ArgumentOutOfRangeException<object>();
            }
        }
        
        private static class Throw
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static T ArgumentOutOfRangeException<T>() => throw new ArgumentOutOfRangeException();
        }
    }
}