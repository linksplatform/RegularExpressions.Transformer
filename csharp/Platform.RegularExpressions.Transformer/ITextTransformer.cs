using System.Runtime.CompilerServices;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.RegularExpressions.Transformer
{
    public interface ITextTransformer : ITransformer
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        string Transform(string sourceText);
    }
}
