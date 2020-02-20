using System.Text.RegularExpressions;
using Xunit;

namespace Platform.RegularExpressions.Transformer.Tests
{
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
                (@"\|0", "0||", int.MaxValue), // "\|0" -> "0||" repeated forever (| symbol should be escaped for regular expression pattern)
                ("0", "", int.MaxValue),       // "0" -> "" repeated forever
            };
            var transformer = new Transformer(rules);
            var input = "101";
            var expectedOutput = "|||||";
            var output = transformer.Transform(input, null);
            Assert.Equal(expectedOutput, output);
        }
    }
}
