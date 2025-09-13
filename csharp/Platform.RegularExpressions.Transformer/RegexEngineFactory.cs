using System;
using System.Runtime.CompilerServices;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.RegularExpressions.Transformer
{
    /// <summary>
    /// <para>
    /// Represents a factory for creating regex engines.
    /// </para>
    /// <para></para>
    /// </summary>
    public static class RegexEngineFactory
    {
        private static IRegexEngine? _defaultEngine;

        /// <summary>
        /// <para>
        /// Gets or sets the default regex engine used throughout the application.
        /// </para>
        /// <para></para>
        /// </summary>
        public static IRegexEngine DefaultEngine
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _defaultEngine ??= CreateEngine(RegexEngineType.SystemRegex);
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => _defaultEngine = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// <para>
        /// Creates a regex engine of the specified type.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="engineType">
        /// <para>The type of regex engine to create.</para>
        /// <para></para>
        /// </param>
        /// <returns>
        /// <para>A regex engine instance.</para>
        /// <para></para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IRegexEngine CreateEngine(RegexEngineType engineType)
        {
            return engineType switch
            {
                RegexEngineType.SystemRegex => new SystemRegexEngine(),
                RegexEngineType.PCRE2 => new PcreRegexEngine(),
                _ => throw new ArgumentOutOfRangeException(nameof(engineType), engineType, "Unknown regex engine type")
            };
        }

        /// <summary>
        /// <para>
        /// Sets the default regex engine type for the application.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="engineType">
        /// <para>The type of regex engine to use as default.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetDefaultEngine(RegexEngineType engineType)
        {
            DefaultEngine = CreateEngine(engineType);
        }

        /// <summary>
        /// <para>
        /// Gets the current default engine type.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <returns>
        /// <para>The current default engine type.</para>
        /// <para></para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RegexEngineType GetDefaultEngineType()
        {
            return DefaultEngine switch
            {
                SystemRegexEngine => RegexEngineType.SystemRegex,
                PcreRegexEngine => RegexEngineType.PCRE2,
                _ => RegexEngineType.SystemRegex
            };
        }
    }
}