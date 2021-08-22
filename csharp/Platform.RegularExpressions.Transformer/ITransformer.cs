using System.Collections.Generic;
using System.Runtime.CompilerServices;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.RegularExpressions.Transformer
{
    /// <summary>
    /// <para>
    /// Defines the transformer.
    /// </para>
    /// <para></para>
    /// </summary>
    public interface ITransformer
    {
        /// <summary>
        /// <para>
        /// Gets the rules value.
        /// </para>
        /// <para></para>
        /// </summary>
        IList<ISubstitutionRule> Rules
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
        }
    }
}
