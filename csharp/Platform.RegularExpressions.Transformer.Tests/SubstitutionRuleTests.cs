using System.Text.RegularExpressions;
using Xunit;

namespace Platform.RegularExpressions.Transformer.Tests
{
    /// <summary>
    /// <para>
    /// Represents the substitution rule tests.
    /// </para>
    /// <para></para>
    /// </summary>
    public class SubstitutionRuleTests
    {
        /// <summary>
        /// <para>
        /// Tests that options override test.
        /// </para>
        /// <para></para>
        /// </summary>
        [Fact]
        public void OptionsOverrideTest()
        {
            SubstitutionRule rule = (new Regex(@"^\s*?\#pragma[\sa-zA-Z0-9\/]+$"), "", 0);
            Assert.Equal(RegexOptions.Compiled | RegexOptions.Multiline, rule.MatchPattern.Options);
        }
    }
}
