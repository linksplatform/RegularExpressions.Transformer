using System.Text.RegularExpressions;
using Xunit;

namespace Platform.RegularExpressions.Transformer.Tests
{
    /// <summary>
    /// <para>
    /// Represents the markov algorithms tests.
    /// </para>
    /// <para></para>
    /// </summary>
    public class MarkovAlgorithmsTests
    {
        /// <remarks>
        /// Example is from https://en.wikipedia.org/wiki/Markov_algorithm.
        /// </remarks>
        [Fact]
        public void BinaryToUnaryNumbersTest()
        {
            var rules = new SubstitutionRule[]
            {
                ("1", "0|", int.MaxValue),     // "1" -> "0|" repeated forever
                // | symbol should be escaped for regular expression pattern, but not in the substitution pattern
                (@"\|0", "0||", int.MaxValue), // "\|0" -> "0||" repeated forever 
                ("0", "", int.MaxValue),       // "0" -> "" repeated forever
            };
            var transformer = new TextTransformer(rules);
            var input = "101";
            var expectedOutput = "|||||";
            var output = transformer.Transform(input);
            Assert.Equal(expectedOutput, output);
        }

        /// <summary>
        /// Tests that terminating rules stop the algorithm immediately when applied.
        /// </summary>
        [Fact]
        public void TerminatingRuleStopsAlgorithmTest()
        {
            var rules = new SubstitutionRule[]
            {
                ("a", "b", 0, false),        // Regular rule: a -> b
                ("b", "STOP", 0, true),      // Terminating rule: b -> STOP (should stop here)
                ("STOP", "c", 0, false),     // This rule should never be applied
            };
            var transformer = new TextTransformer(rules);
            var input = "a";
            var expectedOutput = "STOP";
            var output = transformer.Transform(input);
            Assert.Equal(expectedOutput, output);
        }

        /// <summary>
        /// Tests that non-terminating rules continue processing.
        /// </summary>
        [Fact]
        public void NonTerminatingRulesContinueProcessingTest()
        {
            var rules = new SubstitutionRule[]
            {
                ("a", "b", 0, false),        // Regular rule: a -> b
                ("b", "c", 0, false),        // Regular rule: b -> c
                ("c", "d", 0, false),        // Regular rule: c -> d
            };
            var transformer = new TextTransformer(rules);
            var input = "a";
            var expectedOutput = "d";
            var output = transformer.Transform(input);
            Assert.Equal(expectedOutput, output);
        }

        /// <summary>
        /// Tests terminating rule with complex pattern.
        /// </summary>
        [Fact]
        public void TerminatingRuleWithComplexPatternTest()
        {
            var rules = new SubstitutionRule[]
            {
                (@"(\d+)", "[$1]", 0, false),         // Wrap numbers in brackets
                (@"\[42\]", "FORTY-TWO", 0, true),    // Terminating rule for [42]
                (@"\[(\d+)\]", "NUM:$1", 0, false),  // This should not be applied for [42]
            };
            var transformer = new TextTransformer(rules);
            var input = "42";
            var expectedOutput = "FORTY-TWO";
            var output = transformer.Transform(input);
            Assert.Equal(expectedOutput, output);
        }

        /// <summary>
        /// Tests that terminating rules work with multiple matches in text.
        /// </summary>
        [Fact]
        public void TerminatingRuleWithMultipleMatchesTest()
        {
            var rules = new SubstitutionRule[]
            {
                ("x", "X", 0, false),             // Regular rule: x -> X
                ("X", "TERMINATED", 0, true),     // Terminating rule: X -> TERMINATED
                ("y", "Y", 0, false),             // This rule should not be applied
            };
            var transformer = new TextTransformer(rules);
            var input = "xyx";  // Should transform both x to X, then first X to TERMINATED and stop
            var expectedOutput = "TERMINATEDyX";  // Both x converted to X, then first X terminated
            var output = transformer.Transform(input);
            Assert.Equal(expectedOutput, output);
        }

        /// <summary>
        /// Tests the Wikipedia Markov algorithm example with terminating rule.
        /// Example modified to include a terminating condition.
        /// </summary>
        [Fact]
        public void WikipediaExampleWithTerminatingRuleTest()
        {
            var rules = new SubstitutionRule[]
            {
                ("1", "0|", int.MaxValue),     // "1" -> "0|" repeated forever
                (@"\|0", "0||", int.MaxValue), // "\|0" -> "0||" repeated forever 
                ("0", "", int.MaxValue),       // "0" -> "" repeated forever
                (@"\|\|\|\|\|", "FIVE", true), // Terminating rule: five bars -> FIVE
            };
            var transformer = new TextTransformer(rules);
            var input = "101";
            var expectedOutput = "FIVE";  // Should transform to ||||| then to FIVE and stop
            var output = transformer.Transform(input);
            Assert.Equal(expectedOutput, output);
        }
    }
}
