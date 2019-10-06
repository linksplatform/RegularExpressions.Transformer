using System.Collections.Generic;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.RegexTransformer
{
    public class Transformer : ITransformer
    {
        private readonly IList<ISubstitutionRule> _substitutionRules;

        public Transformer(IList<ISubstitutionRule> substitutionRules) => _substitutionRules = substitutionRules;

        public string Transform(string source, IContext context)
        {
            var current = source;
            for (var i = 0; i < _substitutionRules.Count; i++)
            {
                var rule = _substitutionRules[i];
                var matchPattern = rule.MatchPattern;
                var substitutionPattern = rule.SubstitutionPattern;
                var pathPattern = rule.PathPattern;
                var maximumRepeatCount = rule.MaximumRepeatCount;
                if (pathPattern == null || pathPattern.IsMatch(context.Path))
                {
                    var replaceCount = 0;
                    do
                    {
                        current = matchPattern.Replace(current, substitutionPattern);
                        if (++replaceCount > maximumRepeatCount)
                        {
                            break;
                        }
                    }
                    while (matchPattern.IsMatch(current));
                }
            }
            return current;
        }
    }
}
