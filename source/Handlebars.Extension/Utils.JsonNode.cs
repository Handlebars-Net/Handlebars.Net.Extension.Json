#if NET6_0_OR_GREATER

using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace HandlebarsDotNet.Extension.Json
{
    internal static partial class Utils
    {
        public static IEnumerable GetEnumerator(JsonNode jsonNode)
        {
            return jsonNode switch
            {
                JsonObject jsonObject => EnumerateObject(jsonObject),
                JsonArray _ => ArrayProperties,
                _ => Throw.ArgumentOutOfRangeException<IEnumerable>()
            };

            static IEnumerable<string> EnumerateObject(JsonObject jsonObject)
            {
                foreach (var property in jsonObject)
                {
                    yield return property.Key;
                }
            }
        }

        public static object? ExtractProperty(JsonNode? property)
        {
            if (property == null)
            {
                return null;
            }

            var type = property.GetType();
            if (type == typeof(JsonObject) || type == typeof(JsonArray))
            {
                return property;
            }

            return ExtractProperty(property.GetValue<JsonElement>());
        }
    }
}

#endif