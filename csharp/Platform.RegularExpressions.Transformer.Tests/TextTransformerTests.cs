using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;

namespace Platform.RegularExpressions.Transformer.Tests
{
    /// <summary>
    /// <para>
    /// Represents the text transformer tests.
    /// </para>
    /// <para></para>
    /// </summary>
    public class TextTransformerTests
    {
        /// <summary>
        /// <para>
        /// Tests that debug output test.
        /// </para>
        /// <para></para>
        /// </summary>
        [Fact]
        public void DebugOutputTest()
        {
            var sourceText = "aaaa";
            var firstStepReferenceText = "bbbb";
            var secondStepReferenceText = "cccc";

            var transformer = new TextTransformer(new SubstitutionRule[] {
                (new Regex("a"), "b"),
                (new Regex("b"), "c")
            });

            var steps = transformer.GetSteps(sourceText);

            Assert.Equal(2, steps.Count);
            Assert.Equal(firstStepReferenceText, steps[0]);
            Assert.Equal(secondStepReferenceText, steps[1]);
        }

        /// <summary>
        /// <para>
        /// Tests that debug files output test.
        /// </para>
        /// <para></para>
        /// </summary>
        [Fact]
        public void DebugFilesOutputTest()
        {
            var sourceText = "aaaa";
            var firstStepReferenceText = "bbbb";
            var secondStepReferenceText = "cccc";

            var transformer = new TextTransformer(new SubstitutionRule[] {
                (new Regex("a"), "b"),
                (new Regex("b"), "c")
            });

            var targetFilename = Path.GetTempFileName();

            transformer.WriteStepsToFiles(sourceText, $"{targetFilename}.txt", skipFilesWithNoChanges: false);

            CheckAndCleanUpTwoRulesFiles(firstStepReferenceText, secondStepReferenceText, transformer, targetFilename);
        }

        private static void CheckAndCleanUpTwoRulesFiles(string firstStepReferenceText, string secondStepReferenceText, TextTransformer transformer, string targetFilename)
        {
            var firstStepReferenceFilename = $"{targetFilename}.0.txt";
            var firstStepRuleFilename = $"{targetFilename}.0.rule.txt";
            var secondStepReferenceFilename = $"{targetFilename}.1.txt";
            var secondStepRuleFilename = $"{targetFilename}.1.rule.txt";

            Assert.True(File.Exists(firstStepReferenceFilename));
            Assert.True(File.Exists(firstStepRuleFilename));
            Assert.True(File.Exists(secondStepReferenceFilename));
            Assert.True(File.Exists(secondStepRuleFilename));

            Assert.Equal(firstStepReferenceText, File.ReadAllText(firstStepReferenceFilename, Encoding.UTF8));
            Assert.Equal(transformer.Rules[0].ToString(), File.ReadAllText(firstStepRuleFilename, Encoding.UTF8));
            Assert.Equal(secondStepReferenceText, File.ReadAllText(secondStepReferenceFilename, Encoding.UTF8));
            Assert.Equal(transformer.Rules[1].ToString(), File.ReadAllText(secondStepRuleFilename, Encoding.UTF8));

            File.Delete(firstStepReferenceFilename);
            File.Delete(firstStepRuleFilename);
            File.Delete(secondStepReferenceFilename);
            File.Delete(secondStepRuleFilename);
        }

        /// <summary>
        /// <para>
        /// Tests that files with no changes skiped test.
        /// </para>
        /// <para></para>
        /// </summary>
        [Fact]
        public void FilesWithNoChangesSkipedTest()
        {
            var sourceText = "aaaa";
            var firstStepReferenceText = "bbbb";
            var thirdStepReferenceText = "cccc";

            var transformer = new TextTransformer(new SubstitutionRule[] {
                (new Regex("a"), "b"),
                (new Regex("x"), "y"),
                (new Regex("b"), "c")
            });

            var targetFilename = Path.GetTempFileName();

            transformer.WriteStepsToFiles(sourceText, $"{targetFilename}.txt", skipFilesWithNoChanges: true);

            CheckAndCleanUpThreeRulesFiles(firstStepReferenceText, thirdStepReferenceText, transformer, targetFilename);
        }

        private static void CheckAndCleanUpThreeRulesFiles(string firstStepReferenceText, string thirdStepReferenceText, TextTransformer transformer, string targetFilename)
        {
            var firstStepReferenceFilename = $"{targetFilename}.0.txt";
            var firstStepRuleFilename = $"{targetFilename}.0.rule.txt";
            var secondStepReferenceFilename = $"{targetFilename}.1.txt";
            var secondStepRuleFilename = $"{targetFilename}.1.rule.txt";
            var thirdStepReferenceFilename = $"{targetFilename}.2.txt";
            var thirdStepRuleFilename = $"{targetFilename}.2.rule.txt";

            Assert.True(File.Exists(firstStepReferenceFilename));
            Assert.True(File.Exists(firstStepReferenceFilename));
            Assert.False(File.Exists(secondStepReferenceFilename));
            Assert.False(File.Exists(secondStepRuleFilename));
            Assert.True(File.Exists(thirdStepReferenceFilename));
            Assert.True(File.Exists(thirdStepRuleFilename));

            Assert.Equal(firstStepReferenceText, File.ReadAllText(firstStepReferenceFilename, Encoding.UTF8));
            Assert.Equal(transformer.Rules[0].ToString(), File.ReadAllText(firstStepRuleFilename, Encoding.UTF8));
            Assert.Equal(thirdStepReferenceText, File.ReadAllText(thirdStepReferenceFilename, Encoding.UTF8));
            Assert.Equal(transformer.Rules[2].ToString(), File.ReadAllText(thirdStepRuleFilename, Encoding.UTF8));

            File.Delete(firstStepReferenceFilename);
            File.Delete(firstStepRuleFilename);
            File.Delete(secondStepReferenceFilename);
            File.Delete(secondStepRuleFilename);
            File.Delete(thirdStepReferenceFilename);
            File.Delete(thirdStepRuleFilename);
        }

        /// <summary>
        /// <para>
        /// Tests that debug output using transformers generation test.
        /// </para>
        /// <para></para>
        /// </summary>
        [Fact]
        public void DebugOutputUsingTransformersGenerationTest()
        {
            var sourceText = "aaaa";
            var firstStepReferenceText = "bbbb";
            var secondStepReferenceText = "cccc";

            var transformer = new TextTransformer(new SubstitutionRule[] {
                (new Regex("a"), "b"),
                (new Regex("b"), "c")
            });

            var steps = transformer.GenerateTransformersForEachRule().TransformWithAll(sourceText);

            Assert.Equal(2, steps.Count);
            Assert.Equal(firstStepReferenceText, steps[0]);
            Assert.Equal(secondStepReferenceText, steps[1]);
        }

        /// <summary>
        /// <para>
        /// Tests that debug files output using transformers generation test.
        /// </para>
        /// <para></para>
        /// </summary>
        [Fact]
        public void DebugFilesOutputUsingTransformersGenerationTest()
        {
            var sourceText = "aaaa";
            var firstStepReferenceText = "bbbb";
            var secondStepReferenceText = "cccc";

            var transformer = new TextTransformer(new SubstitutionRule[] {
                (new Regex("a"), "b"),
                (new Regex("b"), "c")
            });

            var targetFilename = Path.GetTempFileName();

            transformer.GenerateTransformersForEachRule().TransformWithAllToFiles(sourceText, $"{targetFilename}.txt", skipFilesWithNoChanges: false);

            CheckAndCleanUpTwoRulesFiles(firstStepReferenceText, secondStepReferenceText, transformer, targetFilename);
        }

        /// <summary>
        /// <para>
        /// Tests that files with no changes skiped when using transformers generation test.
        /// </para>
        /// <para></para>
        /// </summary>
        [Fact]
        public void FilesWithNoChangesSkipedWhenUsingTransformersGenerationTest()
        {
            var sourceText = "aaaa";
            var firstStepReferenceText = "bbbb";
            var thirdStepReferenceText = "cccc";

            var transformer = new TextTransformer(new SubstitutionRule[] {
                (new Regex("a"), "b"),
                (new Regex("x"), "y"),
                (new Regex("b"), "c")
            });

            var targetFilename = Path.GetTempFileName();

            transformer.GenerateTransformersForEachRule().TransformWithAllToFiles(sourceText, $"{targetFilename}.txt", skipFilesWithNoChanges: true);

            CheckAndCleanUpThreeRulesFiles(firstStepReferenceText, thirdStepReferenceText, transformer, targetFilename);
        }
    }
}
