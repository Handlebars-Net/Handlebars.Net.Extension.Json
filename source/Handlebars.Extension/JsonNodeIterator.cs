#if NET6_0_OR_GREATER

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;
using HandlebarsDotNet.Collections;
using HandlebarsDotNet.Compiler;
using HandlebarsDotNet.Iterators;
using HandlebarsDotNet.PathStructure;
using HandlebarsDotNet.Runtime;
using HandlebarsDotNet.ValueProviders;

namespace HandlebarsDotNet.Extension.Json
{
    public class JsonNodeIterator : IIterator
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Iterate(
            in EncodedTextWriter writer,
            BindingContext context,
            ChainSegment[] blockParamsVariables,
            object input,
            TemplateDelegate template,
            TemplateDelegate ifEmpty
        )
        {
            Iterate(writer, context, blockParamsVariables, (JsonNode)input, template, ifEmpty);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Iterate(
            in EncodedTextWriter writer,
            BindingContext context,
            ChainSegment[] blockParamsVariables,
            JsonNode input,
            TemplateDelegate template,
            TemplateDelegate ifEmpty
        )
        {
            switch (input)
            {
                case JsonObject jsonObject:
                    IterateObject(writer, context, blockParamsVariables, jsonObject, template, ifEmpty);
                    break;
                case JsonArray jsonArray:
                    IterateArray(writer, context, blockParamsVariables, jsonArray, template, ifEmpty);
                    break;

                default:
                    Throw.ArgumentOutOfRangeException();
                    break;
            }
        }

        private static void IterateObject(
            in EncodedTextWriter writer,
            BindingContext context,
            ChainSegment[] blockParamsVariables,
            JsonObject target,
            TemplateDelegate template,
            TemplateDelegate ifEmpty
        )
        {
            using var innerContext = context.CreateFrame();
            var iterator = new ObjectIteratorValues(innerContext);
            var blockParamsValues = new BlockParamsValues(innerContext, blockParamsVariables);

            blockParamsValues.CreateProperty(0, out var _0);
            blockParamsValues.CreateProperty(1, out var _1);

            var enumerator = ExtendedEnumerator<KeyValuePair<string, JsonNode?>>.Create(target.GetEnumerator());

            iterator.First = BoxedValues.True;
            iterator.Last = BoxedValues.False;

            int index = 0;
            while (enumerator.MoveNext())
            {
                var current = enumerator.Current;

                var currentValue = current.Value;
                iterator.Key = currentValue.Key;

                if (index == 1) iterator.First = BoxedValues.False;
                if (current.IsLast) iterator.Last = BoxedValues.True;

                iterator.Index = BoxedValues.Int(index);

                object? resolvedValue = currentValue.Value;

                blockParamsValues[_0] = resolvedValue;
                blockParamsValues[_1] = currentValue.Key;

                iterator.Value = resolvedValue;
                innerContext.Value = resolvedValue;

                template(writer, innerContext);

                ++index;
            }

            if (index == 0)
            {
                innerContext.Value = context.Value;
                ifEmpty(writer, innerContext);
            }
        }

        private static void IterateArray(
            in EncodedTextWriter writer,
            BindingContext context,
            ChainSegment[] blockParamsVariables,
            JsonArray target,
            TemplateDelegate template,
            TemplateDelegate ifEmpty
        )
        {
            using var innerContext = context.CreateFrame();
            var iterator = new IteratorValues(innerContext);
            var blockParamsValues = new BlockParamsValues(innerContext, blockParamsVariables);

            blockParamsValues.CreateProperty(0, out var _0);
            blockParamsValues.CreateProperty(1, out var _1);

            iterator.First = BoxedValues.True;
            iterator.Last = BoxedValues.False;

            var count = target.Count;
            var enumerator = target.GetEnumerator();

            var index = 0;
            var lastIndex = count - 1;
            while (enumerator.MoveNext())
            {
                var value = enumerator.Current;
                var objectIndex = BoxedValues.Int(index);

                if (index == 1) iterator.First = BoxedValues.False;
                if (index == lastIndex) iterator.Last = BoxedValues.True;

                iterator.Index = objectIndex;

                object? resolvedValue = value;

                blockParamsValues[_0] = resolvedValue;
                blockParamsValues[_1] = objectIndex;

                iterator.Value = resolvedValue;
                innerContext.Value = resolvedValue;

                template(writer, innerContext);

                ++index;
            }

            if (index == 0)
            {
                innerContext.Value = context.Value;
                ifEmpty(writer, innerContext);
            }
        }

        private static class Throw
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void ArgumentOutOfRangeException() => throw new ArgumentOutOfRangeException();
        }
    }
}

#endif