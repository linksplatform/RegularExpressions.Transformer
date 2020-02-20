using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;

namespace Platform.RegularExpressions.Transformer.Tests
{
    public class TransformersTests
    {
        [Fact]
        public void DebugOutputTest()
        {
            var rule1 = (new Regex("a"), "b");
            var rule2 = (new Regex("b"), "c");

            var sourceText = "aaaa";
            var firstStepReferenceText = "bbbb";
            var secondStepReferenceText = "cccc";

            var sourceFilename = Path.GetTempFileName();
            File.WriteAllText(sourceFilename, sourceText, Encoding.UTF8);

            var transformer = new Transformer(new SubstitutionRule[] { rule1, rule2 });

            var targetFilename = Path.GetTempFileName();

            transformer.DebugOutput(sourceFilename, targetFilename, ".txt");

            var firstStepReferenceFilename = $"{targetFilename}.0.txt";
            var secondStepReferenceFilename = $"{targetFilename}.1.txt";

            Assert.True(File.Exists(firstStepReferenceFilename));
            Assert.True(File.Exists(secondStepReferenceFilename));

            Assert.Equal(firstStepReferenceText, File.ReadAllText(firstStepReferenceFilename, Encoding.UTF8));
            Assert.Equal(secondStepReferenceText, File.ReadAllText(secondStepReferenceFilename, Encoding.UTF8));

            File.Delete(sourceFilename);
            File.Delete(firstStepReferenceFilename);
            File.Delete(secondStepReferenceFilename);
        }
    }
}
