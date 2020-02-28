using System.Collections.Generic;
using System.Runtime.CompilerServices;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.RegularExpressions.Transformer
{
    public class TextTransformer : ITextTransformer
    {
        public IList<ISubstitutionRule> Rules
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private set;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TextTransformer(IList<ISubstitutionRule> substitutionRules) => Rules = substitutionRules;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string Transform(string source)
        {
            var current = source;
            for (var i = 0; i < Rules.Count; i++)
            {
                var rule = Rules[i];
                var matchPattern = rule.MatchPattern;
                var substitutionPattern = rule.SubstitutionPattern;
                var maximumRepeatCount = rule.MaximumRepeatCount;
                var replaceCount = 0;
                do
                {
                    current = matchPattern.Replace(current, substitutionPattern);
                    replaceCount++;
                    if (maximumRepeatCount < int.MaxValue && replaceCount > maximumRepeatCount)
                    {
                        break;
                    }
                }
                while (matchPattern.IsMatch(current));
            }
            return current;
        }
    }
}
