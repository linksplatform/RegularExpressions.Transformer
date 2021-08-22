using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.RegularExpressions.Transformer
{
    /// <summary>
    /// <para>
    /// Represents the regex extensions.
    /// </para>
    /// <para></para>
    /// </summary>
    public static class RegexExtensions
    {
        /// <summary>
        /// <para>
        /// Overrides the options using the specified regex.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="regex">
        /// <para>The regex.</para>
        /// <para></para>
        /// </param>
        /// <param name="options">
        /// <para>The options.</para>
        /// <para></para>
        /// </param>
        /// <param name="matchTimeout">
        /// <para>The match timeout.</para>
        /// <para></para>
        /// </param>
        /// <returns>
        /// <para>The regex</para>
        /// <para></para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Regex OverrideOptions(this Regex regex, RegexOptions options, TimeSpan matchTimeout)
        {
            if (regex == null)
            {
                return null;
            }
            return new Regex(regex.ToString(), options, matchTimeout);
        }
    }
}
