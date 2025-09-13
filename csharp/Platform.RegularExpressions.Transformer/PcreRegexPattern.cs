using System;
using System.Runtime.CompilerServices;
using PCRE;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.RegularExpressions.Transformer
{
    /// <summary>
    /// <para>
    /// Represents a regex pattern implementation using PCRE.NET.
    /// </para>
    /// <para></para>
    /// </summary>
    public class PcreRegexPattern : IRegexPattern
    {
        private readonly PcreRegex _regex;
        private readonly TimeSpan _timeout;

        /// <summary>
        /// <para>
        /// Gets the pattern string value.
        /// </para>
        /// <para></para>
        /// </summary>
        public string Pattern
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _regex.ToString();
        }

        /// <summary>
        /// <para>
        /// Gets the timeout value.
        /// </para>
        /// <para></para>
        /// </summary>
        public TimeSpan MatchTimeout
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _timeout;
        }

        /// <summary>
        /// <para>
        /// Gets the underlying PCRE.NET regex instance.
        /// </para>
        /// <para></para>
        /// </summary>
        public PcreRegex UnderlyingRegex
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _regex;
        }

        /// <summary>
        /// <para>
        /// Initializes a new <see cref="PcreRegexPattern"/> instance.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="regex">
        /// <para>The underlying PCRE regex instance.</para>
        /// <para></para>
        /// </param>
        /// <param name="timeout">
        /// <para>The match timeout.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PcreRegexPattern(PcreRegex regex, TimeSpan timeout)
        {
            _regex = regex ?? throw new ArgumentNullException(nameof(regex));
            _timeout = timeout;
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
        public bool IsMatch(string input)
        {
            if (input == null)
                return false;

            return _regex.IsMatch(input);
        }

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
        public string Replace(string input, string replacement)
        {
            if (input == null)
                return null;

            return _regex.Replace(input, replacement);
        }

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
        public IRegexPattern WithTimeout(TimeSpan timeout)
        {
            return new PcreRegexPattern(_regex, timeout);
        }

        /// <summary>
        /// <para>
        /// Returns the string representation of the regex pattern.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <returns>
        /// <para>The pattern string.</para>
        /// <para></para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString() => _regex.ToString();
    }
}