using System;
using System.Text.RegularExpressions;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.RegexTransformer
{
    public class SubstitutionRule : ISubstitutionRule
    {
        public static readonly TimeSpan DefaultMatchTimeout = TimeSpan.FromMinutes(5);
        public static readonly RegexOptions DefaultMatchPatternRegexOptions = RegexOptions.Compiled | RegexOptions.Multiline;
        public static readonly RegexOptions DefaultPathPatternRegexOptions = RegexOptions.Compiled | RegexOptions.Singleline;

        public Regex MatchPattern { get; set; }

        public string SubstitutionPattern { get; set; }

        public Regex PathPattern { get; set; }

        public int MaximumRepeatCount { get; set; }

        public SubstitutionRule(Regex matchPattern, string substitutionPattern, Regex pathPattern, int maximumRepeatCount, RegexOptions? matchPatternOptions, RegexOptions? pathPatternOptions, TimeSpan? matchTimeout)
        {
            MatchPattern = matchPattern;
            SubstitutionPattern = substitutionPattern;
            PathPattern = pathPattern;
            MaximumRepeatCount = maximumRepeatCount;
            OverrideMatchPatternOptions(matchPatternOptions ?? matchPattern.Options, matchTimeout ?? matchPattern.MatchTimeout);
            OverrideMatchPatternOptions(pathPatternOptions ?? pathPattern.Options, matchTimeout ?? pathPattern.MatchTimeout);
        }

        public SubstitutionRule(Regex matchPattern, string substitutionPattern, Regex pathPattern, int maximumRepeatCount, bool useDefaultOptions) : this(matchPattern, substitutionPattern, pathPattern, maximumRepeatCount, useDefaultOptions ? DefaultMatchPatternRegexOptions : (RegexOptions?)null, useDefaultOptions ? DefaultPathPatternRegexOptions : (RegexOptions?)null, useDefaultOptions ? DefaultMatchTimeout : (TimeSpan?)null) { }

        public SubstitutionRule(Regex matchPattern, string substitutionPattern, Regex pathPattern, int maximumRepeatCount) : this(matchPattern, substitutionPattern, pathPattern, maximumRepeatCount, true) { }

        public SubstitutionRule(Regex matchPattern, string substitutionPattern, int maximumRepeatCount) : this(matchPattern, substitutionPattern, null, maximumRepeatCount) { }

        public SubstitutionRule(Regex matchPattern, string substitutionPattern) : this(matchPattern, substitutionPattern, null, 0) { }

        public static implicit operator SubstitutionRule(ValueTuple<Regex, string> tuple) => new SubstitutionRule(tuple.Item1, tuple.Item2);

        public static implicit operator SubstitutionRule(ValueTuple<Regex, string, int> tuple) => new SubstitutionRule(tuple.Item1, tuple.Item2, tuple.Item3);

        public static implicit operator SubstitutionRule(ValueTuple<Regex, string, Regex, int> tuple) => new SubstitutionRule(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4);

        public void OverrideMatchPatternOptions(RegexOptions options, TimeSpan matchTimeout) => MatchPattern = MatchPattern.OverrideOptions(options, matchTimeout);

        public void OverridePathPatternOptions(RegexOptions options, TimeSpan matchTimeout) => PathPattern = PathPattern.OverrideOptions(options, matchTimeout);
    }
}
