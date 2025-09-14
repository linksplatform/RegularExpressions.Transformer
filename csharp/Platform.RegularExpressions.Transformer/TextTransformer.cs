using System.Collections.Generic;
using System.Runtime.CompilerServices;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.RegularExpressions.Transformer
{
    /// <summary>
    /// <para>
    /// Represents the text transformer.
    /// </para>
    /// </summary>
    /// <seealso cref="ITextTransformer"/>
    public class TextTransformer : ITextTransformer
    {
        /// <summary>
        /// <para>
        /// Gets or sets the rules value.
        /// </para>
        /// </summary>
        public IList<ISubstitutionRule> Rules
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private set;
        }

        /// <summary>
        /// <para>
        /// Initializes a new <see cref="TextTransformer"/> instance.
        /// </para>
        /// </summary>
        /// <param name="substitutionRules">
        /// <para>A substitution rules.</para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TextTransformer(IList<ISubstitutionRule> substitutionRules)
        {
            Rules = substitutionRules;
        }

        /// <summary>
        /// <para>
        /// Transforms the source.
        /// </para>
        /// </summary>
        /// <param name="source">
        /// <para>The source.</para>
        /// </param>
        /// <returns>
        /// <para>The string</para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string Transform(string source)
        {
            var baseTrasformer = new TextSteppedTransformer(Rules);
            baseTrasformer.Reset(source);
            while (baseTrasformer.Next());
            return baseTrasformer.Text;
        }
    }
}
