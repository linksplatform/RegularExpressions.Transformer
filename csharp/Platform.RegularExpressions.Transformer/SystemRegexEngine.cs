using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.RegularExpressions.Transformer
{
    /// <summary>
    /// <para>
    /// Represents a regex engine implementation using System.Text.RegularExpressions.
    /// </para>
    /// <para></para>
    /// </summary>
    public class SystemRegexEngine : IRegexEngine
    {
        /// <summary>
        /// <para>
        /// The default regex options for the System regex engine.
        /// </para>
        /// <para></para>
        /// </summary>
        public static readonly RegexOptions DefaultRegexOptions = RegexOptions.Compiled | RegexOptions.Multiline;

        /// <summary>
        /// <para>
        /// The default match timeout for the System regex engine.
        /// </para>
        /// <para></para>
        /// </summary>
        public static readonly TimeSpan DefaultMatchTimeout = TimeSpan.FromMinutes(5);

        private readonly RegexOptions _options;
        private readonly TimeSpan _defaultTimeout;

        /// <summary>
        /// <para>
        /// Gets the name of the regex engine.
        /// </para>
        /// <para></para>
        /// </summary>
        public string Name
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => "System.Text.RegularExpressions";
        }

        /// <summary>
        /// <para>
        /// Initializes a new <see cref="SystemRegexEngine"/> instance.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="options">
        /// <para>The default regex options.</para>
        /// <para></para>
        /// </param>
        /// <param name="defaultTimeout">
        /// <para>The default match timeout.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SystemRegexEngine(RegexOptions options, TimeSpan defaultTimeout)
        {
            _options = options;
            _defaultTimeout = defaultTimeout;
        }

        /// <summary>
        /// <para>
        /// Initializes a new <see cref="SystemRegexEngine"/> instance with default options.
        /// </para>
        /// <para></para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SystemRegexEngine() : this(DefaultRegexOptions, DefaultMatchTimeout)
        {
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
        public IRegexPattern CreatePattern(string pattern)
        {
            var regex = new Regex(pattern, _options, _defaultTimeout);
            return new SystemRegexPattern(regex);
        }

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
        public IRegexPattern CreatePattern(string pattern, TimeSpan timeout)
        {
            var regex = new Regex(pattern, _options, timeout);
            return new SystemRegexPattern(regex);
        }
    }
}