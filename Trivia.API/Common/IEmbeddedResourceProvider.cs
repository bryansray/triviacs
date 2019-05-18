namespace Trivia.API.Common
{
    public interface IEmbeddedResourceProvider
    {
        string Get(string name, string prefix = "Queries");
    }
}