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

        private static readonly JsonNodeObjectDescriptor JsonNodeObjectDescriptor = new JsonNodeObjectDescriptor();

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

            providers.Add(JsonNodeObjectDescriptor);

            return configuration;
        }
    }
}