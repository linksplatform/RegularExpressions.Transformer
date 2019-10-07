using System;
using System.Text.RegularExpressions;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.Regex.Transformer
{
    public static class RegexExtensions
    {
        public static Regex OverrideOptions(this Regex regex, RegexOptions options, TimeSpan matchTimeout) => new Regex(regex.ToString(), options, matchTimeout);
    }
}
