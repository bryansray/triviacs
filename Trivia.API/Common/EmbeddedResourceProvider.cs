using System.IO;
using System.Reflection;

namespace Trivia.API.Common
{
    public class EmbeddedResourceProvider : IEmbeddedResourceProvider
    {
        private static readonly Assembly _assembly = Assembly.GetExecutingAssembly();
        
        public string Get(string name, string prefix = "Queries")
        {
            var fullPath = $"{_assembly.GetName().Name}.{prefix}.{name}";
            using (var stream = _assembly.GetManifestResourceStream(fullPath))
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}