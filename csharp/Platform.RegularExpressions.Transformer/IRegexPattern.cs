using System;
using System.Runtime.CompilerServices;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.RegularExpressions.Transformer
{
    /// <summary>
    /// <para>
    /// Defines the regex pattern interface that abstracts different regex engines.
    /// </para>
    /// <para></para>
    /// </summary>
    public interface IRegexPattern
    {
        /// <summary>
        /// <para>
        /// Gets the pattern string value.
        /// </para>
        /// <para></para>
        /// </summary>
        string Pattern
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
        }

        /// <summary>
        /// <para>
        /// Gets the timeout value.
        /// </para>
        /// <para></para>
        /// </summary>
        TimeSpan MatchTimeout
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
        }

        /// <summary>
        /// <para>
        /// Determines whether the specified input matches the pattern.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="input">
        /// <para>The input string.</para>
        /// <para></para>
        /// </param>
        /// <returns>
        /// <para>True if the pattern matches; otherwise, false.</para>
        /// <para></para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool IsMatch(string input);

        /// <summary>
        /// <para>
        /// Replaces all matches in the input string with the replacement pattern.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="input">
        /// <para>The input string.</para>
        /// <para></para>
        /// </param>
        /// <param name="replacement">
        /// <para>The replacement pattern.</para>
        /// <para></para>
        /// </param>
        /// <returns>
        /// <para>The modified string with replacements.</para>
        /// <para></para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        string Replace(string input, string replacement);

        /// <summary>
        /// <para>
        /// Creates a new regex pattern with overridden options.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="timeout">
        /// <para>The match timeout.</para>
        /// <para></para>
        /// </param>
        /// <returns>
        /// <para>A new regex pattern with the specified timeout.</para>
        /// <para></para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IRegexPattern WithTimeout(TimeSpan timeout);
    }
}