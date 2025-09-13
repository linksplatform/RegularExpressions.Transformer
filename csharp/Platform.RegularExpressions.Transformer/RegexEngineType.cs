using System.Runtime.CompilerServices;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.RegularExpressions.Transformer
{
    /// <summary>
    /// <para>
    /// Represents the available regex engine types.
    /// </para>
    /// <para></para>
    /// </summary>
    public enum RegexEngineType
    {
        /// <summary>
        /// <para>
        /// System.Text.RegularExpressions engine (default .NET regex engine).
        /// </para>
        /// <para></para>
        /// </summary>
        SystemRegex = 0,

        /// <summary>
        /// <para>
        /// PCRE2 engine (Perl Compatible Regular Expressions).
        /// </para>
        /// <para></para>
        /// </summary>
        PCRE2 = 1
    }
}