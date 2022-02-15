#if NET6_0_OR_GREATER

using System;
using System.Collections;
using System.Text.Json.Nodes;
using HandlebarsDotNet.ObjectDescriptors;

namespace HandlebarsDotNet.Extension.Json
{
    internal class JsonNodeObjectDescriptor : IObjectDescriptorProvider
    {
        private static readonly Type Type = typeof(JsonNode);

        private readonly ObjectDescriptor _descriptor = new ObjectDescriptor(
            Type,
            JsonDocumentMemberAccessor,
            (descriptor, instance) => GetEnumerator(instance),
            self => new JsonNodeIterator()
        );

        private static readonly JsonNodeMemberAccessor JsonDocumentMemberAccessor = new JsonNodeMemberAccessor();

        public bool TryGetDescriptor(Type type, out ObjectDescriptor value)
        {
            if (!Type.IsAssignableFrom(type))
            {
                value = ObjectDescriptor.Empty;
                return false;
            }

            value = _descriptor;
            return true;
        }

        private static IEnumerable GetEnumerator(object instance)
        {
            var jsonNode = (JsonNode)instance;

            return Utils.GetEnumerator(jsonNode);
        }
    }
}

#endif