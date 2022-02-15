using System;
using System.Text.Json;
using HandlebarsDotNet.PathStructure;

namespace HandlebarsDotNet.Extension.Json
{
    public partial class CountMemberAliasProvider : IMemberAliasProvider<JsonElement>
    {
        public bool TryGetMemberByAlias(JsonElement instance, Type targetType, ChainSegment memberAlias, out object? value)
        {
            if (!EqualsIgnoreCase("count", memberAlias) && !EqualsIgnoreCase("length", memberAlias))
            {
                value = null;
                return false;
            }
            
            if (instance.ValueKind != JsonValueKind.Array)
            {
                value = null;
                return false;
            }

            value = instance.GetArrayLength();
            return true;
        }

        private static bool EqualsIgnoreCase(string a, ChainSegment b)
        {
            return string.Equals(a, b.TrimmedValue, StringComparison.OrdinalIgnoreCase);
        }
    }
}