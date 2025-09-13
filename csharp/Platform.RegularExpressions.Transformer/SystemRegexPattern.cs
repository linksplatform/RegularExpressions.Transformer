using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.RegularExpressions.Transformer
{
    /// <summary>
    /// <para>
    /// Represents a regex pattern implementation using System.Text.RegularExpressions.
    /// </para>
    /// <para></para>
    /// </summary>
    public class SystemRegexPattern : IRegexPattern
    {
        private readonly Regex _regex;

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
            get => _regex.MatchTimeout;
        }

        /// <summary>
        /// <para>
        /// Gets the underlying System.Text.RegularExpressions.Regex instance.
        /// </para>
        /// <para></para>
        /// </summary>
        public Regex UnderlyingRegex
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _regex;
        }

        /// <summary>
        /// <para>
        /// Initializes a new <see cref="SystemRegexPattern"/> instance.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="regex">
        /// <para>The underlying regex instance.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SystemRegexPattern(Regex regex)
        {
            _regex = regex ?? throw new ArgumentNullException(nameof(regex));
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
        public bool IsMatch(string input) => _regex.IsMatch(input);

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
        public string Replace(string input, string replacement) => _regex.Replace(input, replacement);

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
            var newRegex = new Regex(_regex.ToString(), _regex.Options, timeout);
            return new SystemRegexPattern(newRegex);
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