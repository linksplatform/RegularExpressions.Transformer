using System;
using System.Runtime.CompilerServices;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.RegularExpressions.Transformer
{
    /// <summary>
    /// <para>
    /// Defines the regex engine interface for creating regex patterns.
    /// </para>
    /// <para></para>
    /// </summary>
    public interface IRegexEngine
    {
        /// <summary>
        /// <para>
        /// Gets the name of the regex engine.
        /// </para>
        /// <para></para>
        /// </summary>
        string Name
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
        }

        /// <summary>
        /// <para>
        /// Creates a regex pattern from the specified pattern string.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="pattern">
        /// <para>The regex pattern string.</para>
        /// <para></para>
        /// </param>
        /// <returns>
        /// <para>A regex pattern instance.</para>
        /// <para></para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IRegexPattern CreatePattern(string pattern);

        /// <summary>
        /// <para>
        /// Creates a regex pattern from the specified pattern string with timeout.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="pattern">
        /// <para>The regex pattern string.</para>
        /// <para></para>
        /// </param>
        /// <param name="timeout">
        /// <para>The match timeout.</para>
        /// <para></para>
        /// </param>
        /// <returns>
        /// <para>A regex pattern instance.</para>
        /// <para></para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IRegexPattern CreatePattern(string pattern, TimeSpan timeout);
    }
}