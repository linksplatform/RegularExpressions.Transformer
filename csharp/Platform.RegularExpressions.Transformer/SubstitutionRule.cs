using System;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.RegularExpressions.Transformer
{
    /// <summary>
    /// <para>
    /// Represents the substitution rule.
    /// </para>
    /// <para></para>
    /// </summary>
    /// <seealso cref="ISubstitutionRule"/>
    public class SubstitutionRule : ISubstitutionRule
    {
        /// <summary>
        /// <para>
        /// The from minutes.
        /// </para>
        /// <para></para>
        /// </summary>
        public static readonly TimeSpan DefaultMatchTimeout = TimeSpan.FromMinutes(5);
        /// <summary>
        /// <para>
        /// The multiline.
        /// </para>
        /// <para></para>
        /// </summary>
        public static readonly RegexOptions DefaultMatchPatternRegexOptions = RegexOptions.Compiled | RegexOptions.Multiline;

        /// <summary>
        /// <para>
        /// Gets or sets the match pattern value.
        /// </para>
        /// <para></para>
        /// </summary>
        public IRegexPattern MatchPattern
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set;
        }

        /// <summary>
        /// <para>
        /// Gets the legacy System.Text.RegularExpressions.Regex pattern for backward compatibility.
        /// </para>
        /// <para></para>
        /// </summary>
        public Regex LegacyMatchPattern
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (MatchPattern is SystemRegexPattern systemPattern)
                {
                    return systemPattern.UnderlyingRegex;
                }
                
                // For PCRE patterns, create a fallback System.Text.RegularExpressions.Regex
                // This maintains backward compatibility but may not have exact feature parity
                return new Regex(MatchPattern.Pattern, SubstitutionRule.DefaultMatchPatternRegexOptions, MatchPattern.MatchTimeout);
            }
        }

        /// <summary>
        /// <para>
        /// Gets or sets the substitution pattern value.
        /// </para>
        /// <para></para>
        /// </summary>
        public string SubstitutionPattern
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set;
        }

        /// <summary>
        /// <para>
        /// Gets or sets the path pattern value.
        /// </para>
        /// <para></para>
        /// </summary>
        public IRegexPattern? PathPattern
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set;
        }

        /// <summary>
        /// <para>
        /// Gets the legacy System.Text.RegularExpressions.Regex path pattern for backward compatibility.
        /// </para>
        /// <para></para>
        /// </summary>
        public Regex? LegacyPathPattern
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (PathPattern == null)
                    return null;
                    
                if (PathPattern is SystemRegexPattern systemPattern)
                {
                    return systemPattern.UnderlyingRegex;
                }
                
                // For PCRE patterns, create a fallback System.Text.RegularExpressions.Regex
                return new Regex(PathPattern.Pattern, SubstitutionRule.DefaultMatchPatternRegexOptions, PathPattern.MatchTimeout);
            }
        }

        /// <summary>
        /// <para>
        /// Gets or sets the maximum repeat count value.
        /// </para>
        /// <para></para>
        /// </summary>
        public int MaximumRepeatCount
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set;
        }

        /// <summary>
        /// <para>
        /// Initializes a new <see cref="SubstitutionRule"/> instance.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="matchPattern">
        /// <para>A match pattern.</para>
        /// <para></para>
        /// </param>
        /// <param name="substitutionPattern">
        /// <para>A substitution pattern.</para>
        /// <para></para>
        /// </param>
        /// <param name="maximumRepeatCount">
        /// <para>A maximum repeat count.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SubstitutionRule(IRegexPattern matchPattern, string substitutionPattern, int maximumRepeatCount)
        {
            MatchPattern = matchPattern;
            SubstitutionPattern = substitutionPattern;
            MaximumRepeatCount = maximumRepeatCount;
        }

        /// <summary>
        /// <para>
        /// Initializes a new <see cref="SubstitutionRule"/> instance.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="matchPattern">
        /// <para>A match pattern.</para>
        /// <para></para>
        /// </param>
        /// <param name="substitutionPattern">
        /// <para>A substitution pattern.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SubstitutionRule(IRegexPattern matchPattern, string substitutionPattern) : this(matchPattern, substitutionPattern, 0) { }

        /// <summary>
        /// <para>
        /// Initializes a new <see cref="SubstitutionRule"/> instance with legacy System.Text.RegularExpressions.Regex.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="matchPattern">
        /// <para>A match pattern.</para>
        /// <para></para>
        /// </param>
        /// <param name="substitutionPattern">
        /// <para>A substitution pattern.</para>
        /// <para></para>
        /// </param>
        /// <param name="maximumRepeatCount">
        /// <para>A maximum repeat count.</para>
        /// <para></para>
        /// </param>
        /// <param name="matchPatternOptions">
        /// <para>A match pattern options.</para>
        /// <para></para>
        /// </param>
        /// <param name="matchTimeout">
        /// <para>A match timeout.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SubstitutionRule(Regex matchPattern, string substitutionPattern, int maximumRepeatCount, RegexOptions? matchPatternOptions, TimeSpan? matchTimeout)
        {
            var engine = RegexEngineFactory.DefaultEngine;
            var pattern = engine.CreatePattern(matchPattern.ToString(), matchTimeout ?? matchPattern.MatchTimeout);
            MatchPattern = pattern;
            SubstitutionPattern = substitutionPattern;
            MaximumRepeatCount = maximumRepeatCount;
            OverrideMatchPatternOptions(matchPatternOptions ?? matchPattern.Options, matchTimeout ?? matchPattern.MatchTimeout);
        }

        /// <summary>
        /// <para>
        /// Initializes a new <see cref="SubstitutionRule"/> instance with legacy System.Text.RegularExpressions.Regex.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="matchPattern">
        /// <para>A match pattern.</para>
        /// <para></para>
        /// </param>
        /// <param name="substitutionPattern">
        /// <para>A substitution pattern.</para>
        /// <para></para>
        /// </param>
        /// <param name="maximumRepeatCount">
        /// <para>A maximum repeat count.</para>
        /// <para></para>
        /// </param>
        /// <param name="useDefaultOptions">
        /// <para>A use default options.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SubstitutionRule(Regex matchPattern, string substitutionPattern, int maximumRepeatCount, bool useDefaultOptions) : this(matchPattern, substitutionPattern, maximumRepeatCount, useDefaultOptions ? DefaultMatchPatternRegexOptions : (RegexOptions?)null, useDefaultOptions ? DefaultMatchTimeout : (TimeSpan?)null) { }

        /// <summary>
        /// <para>
        /// Initializes a new <see cref="SubstitutionRule"/> instance with legacy System.Text.RegularExpressions.Regex.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="matchPattern">
        /// <para>A match pattern.</para>
        /// <para></para>
        /// </param>
        /// <param name="substitutionPattern">
        /// <para>A substitution pattern.</para>
        /// <para></para>
        /// </param>
        /// <param name="maximumRepeatCount">
        /// <para>A maximum repeat count.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SubstitutionRule(Regex matchPattern, string substitutionPattern, int maximumRepeatCount) : this(matchPattern, substitutionPattern, maximumRepeatCount, true) { }

        /// <summary>
        /// <para>
        /// Initializes a new <see cref="SubstitutionRule"/> instance with legacy System.Text.RegularExpressions.Regex.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="matchPattern">
        /// <para>A match pattern.</para>
        /// <para></para>
        /// </param>
        /// <param name="substitutionPattern">
        /// <para>A substitution pattern.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SubstitutionRule(Regex matchPattern, string substitutionPattern) : this(matchPattern, substitutionPattern, 0) { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SubstitutionRule(ValueTuple<string, string> tuple) => new SubstitutionRule(RegexEngineFactory.DefaultEngine.CreatePattern(tuple.Item1), tuple.Item2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SubstitutionRule(ValueTuple<Regex, string> tuple) => new SubstitutionRule(tuple.Item1, tuple.Item2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SubstitutionRule(ValueTuple<string, string, int> tuple) => new SubstitutionRule(RegexEngineFactory.DefaultEngine.CreatePattern(tuple.Item1), tuple.Item2, tuple.Item3);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SubstitutionRule(ValueTuple<Regex, string, int> tuple) => new SubstitutionRule(tuple.Item1, tuple.Item2, tuple.Item3);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SubstitutionRule(ValueTuple<IRegexPattern, string> tuple) => new SubstitutionRule(tuple.Item1, tuple.Item2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SubstitutionRule(ValueTuple<IRegexPattern, string, int> tuple) => new SubstitutionRule(tuple.Item1, tuple.Item2, tuple.Item3);

        /// <summary>
        /// <para>
        /// Overrides the match pattern options using the specified options.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="options">
        /// <para>The options.</para>
        /// <para></para>
        /// </param>
        /// <param name="matchTimeout">
        /// <para>The match timeout.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OverrideMatchPatternOptions(RegexOptions options, TimeSpan matchTimeout) 
        {
            if (MatchPattern is SystemRegexPattern systemPattern)
            {
                MatchPattern = new SystemRegexPattern(systemPattern.UnderlyingRegex.OverrideOptions(options, matchTimeout));
            }
            else
            {
                MatchPattern = MatchPattern.WithTimeout(matchTimeout);
            }
        }

        /// <summary>
        /// <para>
        /// Overrides the path pattern options using the specified options.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="options">
        /// <para>The options.</para>
        /// <para></para>
        /// </param>
        /// <param name="matchTimeout">
        /// <para>The match timeout.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OverridePathPatternOptions(RegexOptions options, TimeSpan matchTimeout) 
        {
            if (PathPattern == null) 
                return;
                
            if (PathPattern is SystemRegexPattern systemPattern)
            {
                PathPattern = new SystemRegexPattern(systemPattern.UnderlyingRegex.OverrideOptions(options, matchTimeout));
            }
            else
            {
                PathPattern = PathPattern.WithTimeout(matchTimeout);
            }
        }

        /// <summary>
        /// <para>
        /// Returns the string.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <returns>
        /// <para>The string</para>
        /// <para></para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append('"');
            sb.Append(MatchPattern?.ToString() ?? "");
            sb.Append('"');
            sb.Append(" -> ");
            sb.Append('"');
            sb.Append(SubstitutionPattern);
            sb.Append('"');
            if (PathPattern != null)
            {
                sb.Append(" on files ");
                sb.Append('"');
                sb.Append(PathPattern.ToString());
                sb.Append('"');
            }
            if (MaximumRepeatCount > 0)
            {
                if (MaximumRepeatCount >= int.MaxValue)
                {
                    sb.Append(" repeated forever");
                }
                else
                {
                    sb.Append(" repeated up to ");
                    sb.Append(MaximumRepeatCount);
                    sb.Append(" times");
                }
            }
            return sb.ToString();
        }
    }
}
