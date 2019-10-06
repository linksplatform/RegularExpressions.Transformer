#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.RegexTransformer
{
    public class Context : IContext
    {
        public string Path { get; }

        public Context(string path) => Path = path;
    }
}
