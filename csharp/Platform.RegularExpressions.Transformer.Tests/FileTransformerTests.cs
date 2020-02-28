using System.IO;
using Xunit;

namespace Platform.RegularExpressions.Transformer.Tests
{
    public class FileTransformerTests
    {
        [Fact]
        public void FolderToFolderTransfomationTest()
        {
            var tempPath = Path.GetTempPath();
            var sourceFolderPath = Path.Combine(tempPath, "FileTransformerTestsFolderToFolderTransfomationTestSourceFolder");
            var targetFolderPath = Path.Combine(tempPath, "FileTransformerTestsFolderToFolderTransfomationTestTargetFolder");

            var baseTransformer = new TextTransformer(new SubstitutionRule[]
            {
                ("a", "b"),
                ("b", "c")
            });
            var fileTransformer = new FileTransformer(baseTransformer, ".cs", ".cpp");

            // Delete before creation (if previous test failed)
            if (Directory.Exists(sourceFolderPath))
            {
                Directory.Delete(sourceFolderPath, true);
            }
            if (Directory.Exists(targetFolderPath))
            {
                Directory.Delete(targetFolderPath, true);
            }

            Directory.CreateDirectory(sourceFolderPath);
            Directory.CreateDirectory(targetFolderPath);

            File.WriteAllText(Path.Combine(sourceFolderPath, "a.cs"), "a a a");
            var aFolderPath = Path.Combine(sourceFolderPath, "A");
            Directory.CreateDirectory(aFolderPath);
            Directory.CreateDirectory(Path.Combine(sourceFolderPath, "B"));
            File.WriteAllText(Path.Combine(aFolderPath, "b.cs"), "b b b");
            File.WriteAllText(Path.Combine(sourceFolderPath, "x.txt"), "should not be translated");

            fileTransformer.Transform(sourceFolderPath, $"{targetFolderPath}{Path.DirectorySeparatorChar}");

            var aCppFile = Path.Combine(targetFolderPath, "a.cpp");
            Assert.True(File.Exists(aCppFile));
            Assert.Equal("c c c", File.ReadAllText(aCppFile));
            Assert.True(Directory.Exists(Path.Combine(targetFolderPath, "A")));
            Assert.False(Directory.Exists(Path.Combine(targetFolderPath, "B")));
            var bCppFile = Path.Combine(targetFolderPath, "A", "b.cpp");
            Assert.True(File.Exists(bCppFile));
            Assert.Equal("c c c", File.ReadAllText(bCppFile));
            Assert.False(File.Exists(Path.Combine(targetFolderPath, "x.txt")));
            Assert.False(File.Exists(Path.Combine(targetFolderPath, "x.cpp")));

            Directory.Delete(sourceFolderPath, true);
            Directory.Delete(targetFolderPath, true);
        }
    }
}
