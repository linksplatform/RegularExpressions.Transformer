#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.RegexTransformer
{
    public interface ITransformer
    {
        string Transform(string source, IContext context);
    }
}
