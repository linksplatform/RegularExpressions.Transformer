using System;
using System.Runtime.CompilerServices;
using PCRE;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.RegularExpressions.Transformer
{
    /// <summary>
    /// <para>
    /// Represents a regex engine implementation using PCRE.NET.
    /// </para>
    /// <para></para>
    /// </summary>
    public class PcreRegexEngine : IRegexEngine
    {
        /// <summary>
        /// <para>
        /// The default PCRE options for the PCRE regex engine.
        /// </para>
        /// <para></para>
        /// </summary>
        public static readonly PcreOptions DefaultPcreOptions = PcreOptions.Compiled | PcreOptions.MultiLine;

        /// <summary>
        /// <para>
        /// The default match timeout for the PCRE regex engine.
        /// </para>
        /// <para></para>
        /// </summary>
        public static readonly TimeSpan DefaultMatchTimeout = TimeSpan.FromMinutes(5);

        private readonly PcreOptions _options;
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
            get => "PCRE2";
        }

        /// <summary>
        /// <para>
        /// Initializes a new <see cref="PcreRegexEngine"/> instance.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="options">
        /// <para>The default PCRE options.</para>
        /// <para></para>
        /// </param>
        /// <param name="defaultTimeout">
        /// <para>The default match timeout.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PcreRegexEngine(PcreOptions options, TimeSpan defaultTimeout)
        {
            _options = options;
            _defaultTimeout = defaultTimeout;
        }

        /// <summary>
        /// <para>
        /// Initializes a new <see cref="PcreRegexEngine"/> instance with default options.
        /// </para>
        /// <para></para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PcreRegexEngine() : this(DefaultPcreOptions, DefaultMatchTimeout)
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
            var regex = new PcreRegex(pattern, _options);
            return new PcreRegexPattern(regex, _defaultTimeout);
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
            var regex = new PcreRegex(pattern, _options);
            return new PcreRegexPattern(regex, timeout);
        }
    }
}