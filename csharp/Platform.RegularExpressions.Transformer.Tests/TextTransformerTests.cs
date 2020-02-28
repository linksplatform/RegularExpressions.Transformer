using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;

namespace Platform.RegularExpressions.Transformer.Tests
{
    public class TextTransformerTests
    {
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

            var firstStepReferenceFilename = $"{targetFilename}.0.txt";
            var secondStepReferenceFilename = $"{targetFilename}.1.txt";

            Assert.True(File.Exists(firstStepReferenceFilename));
            Assert.True(File.Exists(secondStepReferenceFilename));

            Assert.Equal(firstStepReferenceText, File.ReadAllText(firstStepReferenceFilename, Encoding.UTF8));
            Assert.Equal(secondStepReferenceText, File.ReadAllText(secondStepReferenceFilename, Encoding.UTF8));

            File.Delete(firstStepReferenceFilename);
            File.Delete(secondStepReferenceFilename);
        }

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

            var firstStepReferenceFilename = $"{targetFilename}.0.txt";
            var secondStepReferenceFilename = $"{targetFilename}.1.txt";
            var thirdStepReferenceFilename = $"{targetFilename}.2.txt";

            Assert.True(File.Exists(firstStepReferenceFilename));
            Assert.False(File.Exists(secondStepReferenceFilename));
            Assert.True(File.Exists(thirdStepReferenceFilename));

            Assert.Equal(firstStepReferenceText, File.ReadAllText(firstStepReferenceFilename, Encoding.UTF8));
            Assert.Equal(thirdStepReferenceText, File.ReadAllText(thirdStepReferenceFilename, Encoding.UTF8));

            File.Delete(firstStepReferenceFilename);
            File.Delete(secondStepReferenceFilename);
            File.Delete(thirdStepReferenceFilename);
        }
    }
}
