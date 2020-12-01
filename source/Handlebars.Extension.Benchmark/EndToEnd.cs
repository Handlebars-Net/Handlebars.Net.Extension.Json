using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using BenchmarkDotNet.Attributes;
using HandlebarsDotNet;
using HandlebarsDotNet.Extension.Json;

namespace HandlebarsNet.Extension.Benchmark
{
    public class EndToEnd
    {
        private object _data;
        private HandlebarsTemplate<TextWriter, object, object> _default;

        [Params(5)]
        public int N { get; set; }
        
        [Params("json")]
        public string DataType { get; set; }
        
        [GlobalSetup]
        public void Setup()
        {
            const string template = @"
                childCount={{level1.Count}}
                {{#each level1}}
                    id={{id}}
                    childCount={{level2.Count}}
                    index=[{{@../../index}}:{{@../index}}:{{@index}}]
                    pow1=[{{pow1 @index}}]
                    pow2=[{{pow2 @index}}]
                    pow3=[{{pow3 @index}}]
                    pow4=[{{pow4 @index}}]
                    pow5=[{{#pow5 @index}}empty{{/pow5}}]
                    {{#each level2}}
                        id={{id}}
                        childCount={{level3.Count}}
                        index=[{{@../../index}}:{{@../index}}:{{@index}}]
                        pow1=[{{pow1 @index}}]
                        pow2=[{{pow2 @index}}]
                        pow3=[{{pow3 @index}}]
                        pow4=[{{pow4 @index}}]
                        pow5=[{{#pow5 @index}}empty{{/pow5}}]
                        {{#each level3}}
                            id={{id}}
                            index=[{{@../../index}}:{{@../index}}:{{@index}}]
                            pow1=[{{pow1 @index}}]
                            pow2=[{{pow2 @index}}]
                            pow3=[{{pow3 @index}}]
                            pow4=[{{pow4 @index}}]
                            pow5=[{{#pow5 @index}}empty{{/pow5}}]
                        {{/each}}
                    {{/each}}    
                {{/each}}";

            switch (DataType)
            {
                case "json":
                    var json = JsonSerializer.Serialize(new { level1 = ObjectLevel1Generator()});
                    _data = JsonDocument.Parse(json);
                    break;
            }

            var handlebars = Handlebars.Create();

            handlebars.Configuration.UseJson();
            
            handlebars.RegisterHelper("pow1", (output, context, arguments) => output.WriteSafeString(((int) arguments[0] * (int) arguments[0]).ToString()));
            handlebars.RegisterHelper("pow2", (output, context, arguments) => output.WriteSafeString(((int) arguments[0] * (int) arguments[0]).ToString()));
            handlebars.RegisterHelper("pow5", (output, options, context, arguments) => output.WriteSafeString(((int) arguments[0] * (int) arguments[0]).ToString()));

            using (var reader = new StringReader(template))
            {
                _default = handlebars.Compile(reader);
            }

            handlebars.RegisterHelper("pow3", (output, context, arguments) => output.WriteSafeString(((int) arguments[0] * (int) arguments[0]).ToString()));
            handlebars.RegisterHelper("pow4", (output, context, arguments) => output.WriteSafeString(((int) arguments[0] * (int) arguments[0]).ToString()));

            List<object> ObjectLevel1Generator()
            {
                var level = new List<object>();
                for (int i = 0; i < N; i++)
                {
                    level.Add(new
                    {
                        id = $"{i}",
                        level2 = ObjectLevel2Generator(i)
                    });
                }

                return level;
            }
            
            List<object> ObjectLevel2Generator(int id1)
            {
                var level = new List<object>();
                for (int i = 0; i < N; i++)
                {
                    level.Add(new
                    {
                        id = $"{id1}-{i}",
                        level3 = ObjectLevel3Generator(id1, i)
                    });
                }

                return level;
            }
            
            List<object> ObjectLevel3Generator(int id1, int id2)
            {
                var level = new List<object>();
                for (int i = 0; i < N; i++)
                {
                    level.Add(new
                    {
                        id = $"{id1}-{id2}-{i}"
                    });
                }

                return level;
            }
        }
        
        [Benchmark]
        public void Default() => _default(TextWriter.Null, _data);
    }
}