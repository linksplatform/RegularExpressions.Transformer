using System;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.RegularExpressions.Transformer
{
    public class SubstitutionRule : ISubstitutionRule
    {
        public static readonly TimeSpan DefaultMatchTimeout = TimeSpan.FromMinutes(5);
        public static readonly RegexOptions DefaultMatchPatternRegexOptions = RegexOptions.Compiled | RegexOptions.Multiline;

        public Regex MatchPattern
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set;
        }

        public string SubstitutionPattern
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set;
        }

        public Regex PathPattern
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set;
        }

        public int MaximumRepeatCount
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SubstitutionRule(Regex matchPattern, string substitutionPattern, int maximumRepeatCount, RegexOptions? matchPatternOptions, TimeSpan? matchTimeout)
        {
            MatchPattern = matchPattern;
            SubstitutionPattern = substitutionPattern;
            MaximumRepeatCount = maximumRepeatCount;
            OverrideMatchPatternOptions(matchPatternOptions ?? matchPattern.Options, matchTimeout ?? matchPattern.MatchTimeout);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SubstitutionRule(Regex matchPattern, string substitutionPattern, int maximumRepeatCount, bool useDefaultOptions) : this(matchPattern, substitutionPattern, maximumRepeatCount, useDefaultOptions ? DefaultMatchPatternRegexOptions : (RegexOptions?)null, useDefaultOptions ? DefaultMatchTimeout : (TimeSpan?)null) { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SubstitutionRule(Regex matchPattern, string substitutionPattern, int maximumRepeatCount) : this(matchPattern, substitutionPattern, maximumRepeatCount, true) { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SubstitutionRule(Regex matchPattern, string substitutionPattern) : this(matchPattern, substitutionPattern, 0) { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SubstitutionRule(ValueTuple<string, string> tuple) => new SubstitutionRule(new Regex(tuple.Item1), tuple.Item2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SubstitutionRule(ValueTuple<Regex, string> tuple) => new SubstitutionRule(tuple.Item1, tuple.Item2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SubstitutionRule(ValueTuple<string, string, int> tuple) => new SubstitutionRule(new Regex(tuple.Item1), tuple.Item2, tuple.Item3);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SubstitutionRule(ValueTuple<Regex, string, int> tuple) => new SubstitutionRule(tuple.Item1, tuple.Item2, tuple.Item3);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OverrideMatchPatternOptions(RegexOptions options, TimeSpan matchTimeout) => MatchPattern = MatchPattern.OverrideOptions(options, matchTimeout);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OverridePathPatternOptions(RegexOptions options, TimeSpan matchTimeout) => PathPattern = PathPattern.OverrideOptions(options, matchTimeout);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append('"');
            sb.Append(MatchPattern.ToString());
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
