using System.Collections.Generic;
using System.Linq;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.RegularExpressions.Transformer
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

        public IList<ITransformer> GenerateTransformersForEachRulesStep()
        {
            var transformers = new List<ITransformer>();
            for (int i = 1; i <= _substitutionRules.Count; i++)
            {
                transformers.Add(new Transformer(_substitutionRules.Take(i).ToList()));
            }
            return transformers;
        }
    }
}
