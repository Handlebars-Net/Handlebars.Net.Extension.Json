using HandlebarsDotNet.ObjectDescriptors;

namespace HandlebarsDotNet.Extension.Json
{
    /// <summary>
    /// 
    /// </summary>
    public static class JsonFeatureExtensions
    {
        private static readonly JsonDocumentObjectDescriptor JsonDocumentObjectDescriptor = new JsonDocumentObjectDescriptor();
        private static readonly JsonElementObjectDescriptor JsonElementObjectDescriptor = new JsonElementObjectDescriptor();

#if NET6_0_OR_GREATER
        private static readonly JsonNodeObjectDescriptor JsonNodeObjectDescriptor = new JsonNodeObjectDescriptor();
#endif

        /// <summary>
        /// Adds <see cref="IObjectDescriptorProvider"/>s required to support <c>System.Text.Json</c>. 
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static HandlebarsConfiguration UseJson(this HandlebarsConfiguration configuration)
        {
            var providers = configuration.ObjectDescriptorProviders;
            
            providers.Add(JsonDocumentObjectDescriptor);
            providers.Add(JsonElementObjectDescriptor);

#if NET6_0_OR_GREATER
            providers.Add(JsonNodeObjectDescriptor);
#endif

            return configuration;
        }
    }
}