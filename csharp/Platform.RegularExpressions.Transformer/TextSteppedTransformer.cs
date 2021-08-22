using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.RegularExpressions.Transformer
{
    /// <summary>
    /// <para>
    /// Represents the text stepped transformer.
    /// </para>
    /// <para></para>
    /// </summary>
    /// <seealso cref="ITransformer"/>
    public class TextSteppedTransformer : ITransformer
    {
        /// <summary>
        /// <para>
        /// Gets or sets the rules value.
        /// </para>
        /// <para></para>
        /// </summary>
        public IList<ISubstitutionRule> Rules
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set;
        }

        /// <summary>
        /// <para>
        /// Gets or sets the text value.
        /// </para>
        /// <para></para>
        /// </summary>
        public string Text
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set;
        }

        /// <summary>
        /// <para>
        /// Gets or sets the current value.
        /// </para>
        /// <para></para>
        /// </summary>
        public int Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set;
        }

        /// <summary>
        /// <para>
        /// Initializes a new <see cref="TextSteppedTransformer"/> instance.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="rules">
        /// <para>A rules.</para>
        /// <para></para>
        /// </param>
        /// <param name="text">
        /// <para>A text.</para>
        /// <para></para>
        /// </param>
        /// <param name="current">
        /// <para>A current.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TextSteppedTransformer(IList<ISubstitutionRule> rules, string text, int current) => Reset(rules, text, current);

        /// <summary>
        /// <para>
        /// Initializes a new <see cref="TextSteppedTransformer"/> instance.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="rules">
        /// <para>A rules.</para>
        /// <para></para>
        /// </param>
        /// <param name="text">
        /// <para>A text.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TextSteppedTransformer(IList<ISubstitutionRule> rules, string text) => Reset(rules, text);

        /// <summary>
        /// <para>
        /// Initializes a new <see cref="TextSteppedTransformer"/> instance.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="rules">
        /// <para>A rules.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TextSteppedTransformer(IList<ISubstitutionRule> rules) => Reset(rules);

        /// <summary>
        /// <para>
        /// Initializes a new <see cref="TextSteppedTransformer"/> instance.
        /// </para>
        /// <para></para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TextSteppedTransformer() => Reset();

        /// <summary>
        /// <para>
        /// Resets the rules.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="rules">
        /// <para>The rules.</para>
        /// <para></para>
        /// </param>
        /// <param name="text">
        /// <para>The text.</para>
        /// <para></para>
        /// </param>
        /// <param name="current">
        /// <para>The current.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset(IList<ISubstitutionRule> rules, string text, int current)
        {
            Rules = rules;
            Text = text;
            Current = current;
        }

        /// <summary>
        /// <para>
        /// Resets the rules.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="rules">
        /// <para>The rules.</para>
        /// <para></para>
        /// </param>
        /// <param name="text">
        /// <para>The text.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset(IList<ISubstitutionRule> rules, string text) => Reset(rules, text, -1);

        /// <summary>
        /// <para>
        /// Resets the rules.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="rules">
        /// <para>The rules.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset(IList<ISubstitutionRule> rules) => Reset(rules, "", -1);

        /// <summary>
        /// <para>
        /// Resets the text.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="text">
        /// <para>The text.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset(string text) => Reset(Rules, text, -1);

        /// <summary>
        /// <para>
        /// Resets this instance.
        /// </para>
        /// <para></para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset() => Reset(Array.Empty<ISubstitutionRule>(), "", -1);

        /// <summary>
        /// <para>
        /// Determines whether this instance next.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <returns>
        /// <para>The bool</para>
        /// <para></para>
        /// </returns>
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
