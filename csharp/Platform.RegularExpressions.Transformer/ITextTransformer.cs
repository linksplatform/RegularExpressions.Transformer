using System.Runtime.CompilerServices;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.RegularExpressions.Transformer
{
    /// <summary>
    /// <para>
    /// Defines the text transformer.
    /// </para>
    /// </summary>
    /// <seealso cref="ITransformer"/>
    public interface ITextTransformer : ITransformer
    {
        /// <summary>
        /// <para>
        /// Transforms the source text.
        /// </para>
        /// </summary>
        /// <param name="sourceText">
        /// <para>The source text.</para>
        /// </param>
        /// <returns>
        /// <para>The string</para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        string Transform(string sourceText);
    }
}
