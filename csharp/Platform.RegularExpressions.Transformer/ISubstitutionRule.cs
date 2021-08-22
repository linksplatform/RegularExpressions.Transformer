using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.RegularExpressions.Transformer
{
    /// <summary>
    /// <para>
    /// Defines the substitution rule.
    /// </para>
    /// <para></para>
    /// </summary>
    public interface ISubstitutionRule
    {
        /// <summary>
        /// <para>
        /// Gets the match pattern value.
        /// </para>
        /// <para></para>
        /// </summary>
        Regex MatchPattern
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
        }

        /// <summary>
        /// <para>
        /// Gets the substitution pattern value.
        /// </para>
        /// <para></para>
        /// </summary>
        string SubstitutionPattern
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
        }

        /// <summary>
        /// <para>
        /// Gets the maximum repeat count value.
        /// </para>
        /// <para></para>
        /// </summary>
        int MaximumRepeatCount
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
        }
    }
}