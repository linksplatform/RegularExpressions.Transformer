using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.RegularExpressions.Transformer
{
    public class TextSteppedTransformer : ITransformer
    {
        public IList<ISubstitutionRule> Rules
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set;
        }

        public string Text
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set;
        }

        public int Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TextSteppedTransformer(IList<ISubstitutionRule> rules, string text, int current) => Reset(rules, text, current);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TextSteppedTransformer(IList<ISubstitutionRule> rules, string text) => Reset(rules, text);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TextSteppedTransformer(IList<ISubstitutionRule> rules) => Reset(rules);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TextSteppedTransformer() => Reset();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset(IList<ISubstitutionRule> rules, string text, int current)
        {
            Rules = rules;
            Text = text;
            Current = current;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset(IList<ISubstitutionRule> rules, string text) => Reset(rules, text, -1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset(IList<ISubstitutionRule> rules) => Reset(rules, "", -1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset(string text) => Reset(Rules, text, -1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset() => Reset(Array.Empty<ISubstitutionRule>(), "", -1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Next()
        {
            var current = Current + 1;
            if (current >= Rules.Count)
            {
                return false;
            }
            var rule = Rules[current];
            var matchPattern = rule.MatchPattern;
            var substitutionPattern = rule.SubstitutionPattern;
            var maximumRepeatCount = rule.MaximumRepeatCount;
            var replaceCount = 0;
            var text = Text;
            do
            {
                text = matchPattern.Replace(text, substitutionPattern);
                replaceCount++;
            }
            while ((maximumRepeatCount == int.MaxValue || replaceCount <= maximumRepeatCount) && matchPattern.IsMatch(text));
            Text = text;
            Current = current;
            return true;
        }
    }
}
