#if NET6_0_OR_GREATER

using System.Text.Json.Nodes;
using HandlebarsDotNet.MemberAccessors;
using HandlebarsDotNet.PathStructure;

namespace HandlebarsDotNet.Extension.Json
{
    internal class JsonNodeMemberAccessor : IMemberAccessor
    {
        private readonly CountMemberAliasProvider _aliasProvider = new CountMemberAliasProvider();

        public bool TryGetValue(object instance, ChainSegment memberName, out object? value)
        {
            var element = (JsonNode)instance;

            if (element is JsonObject jsonObject && jsonObject.TryGetPropertyValue(memberName.TrimmedValue, out var property))
            {
                value = Utils.ExtractProperty(property);
                return true;
            }

            if (element is JsonArray jsonArray && int.TryParse(memberName, out var index))
            {
                if (index >= jsonArray.Count)
                {
                    value = null;
                    return false;
                }

                var indexedElement = jsonArray[index];
                value = Utils.ExtractProperty(indexedElement);
                return true;
            }

            if (_aliasProvider.TryGetMemberByAlias(element, typeof(JsonNode), memberName, out value))
            {
                return true;
            }

            value = null;
            return false;
        }
    }
}

#endif