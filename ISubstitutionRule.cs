using System.Text.RegularExpressions;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.RegularExpressions.Transformer
{
    public interface ISubstitutionRule
    {
        Regex MatchPattern { get; }
        string SubstitutionPattern { get; }
        Regex PathPattern { get; }
        int MaximumRepeatCount { get; }
    }
}