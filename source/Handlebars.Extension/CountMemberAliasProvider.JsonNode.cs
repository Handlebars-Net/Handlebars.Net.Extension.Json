using System;
using System.Text.Json.Nodes;
using HandlebarsDotNet.PathStructure;

namespace HandlebarsDotNet.Extension.Json
{
    public partial class CountMemberAliasProvider : IMemberAliasProvider<JsonNode>
    {
        public bool TryGetMemberByAlias(JsonNode instance, Type targetType, ChainSegment memberAlias, out object? value)
        {
            if (!EqualsIgnoreCase("count", memberAlias) && !EqualsIgnoreCase("length", memberAlias))
            {
                value = null;
                return false;
            }
            if (!(instance is JsonArray jsonArray))
            {
                value = null;
                return false;
            }

            value = jsonArray.Count;
            return true;
        }
    }
}
