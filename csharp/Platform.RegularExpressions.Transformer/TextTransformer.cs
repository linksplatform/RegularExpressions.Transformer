using System.Collections.Generic;
using System.Runtime.CompilerServices;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.RegularExpressions.Transformer
{
    public class TextTransformer : ITextTransformer
    {
        private readonly TextSteppedTransformer _baseTransformer;

        public IList<ISubstitutionRule> Rules
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private set;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TextTransformer(IList<ISubstitutionRule> substitutionRules)
        {
            Rules = substitutionRules;
            _baseTransformer = new TextSteppedTransformer(substitutionRules);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string Transform(string source)
        {
            _baseTransformer.Reset(source);
            while (_baseTransformer.Next());
            return _baseTransformer.Text;
        }
    }
}
