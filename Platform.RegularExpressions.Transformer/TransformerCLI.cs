using System.Diagnostics;
using System.IO;
using System.Text;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.RegularExpressions.Transformer
{
    public class TransformerCLI
    {
        private readonly ITransformer _transformer;

        public TransformerCLI(ITransformer transformer) => _transformer = transformer;

        public bool Run(string[] args, out string message)
        {
            message = "";
            var sourcePath = GetArgOrDefault(args, 0);
            if (!File.Exists(sourcePath))
            {
                message = $"{sourcePath} file does not exist.";
                return false;
            }
            var targetPath = GetArgOrDefault(args, 1);
            if (string.IsNullOrWhiteSpace(targetPath))
            {
                targetPath = ChangeToTargetExtension(sourcePath);
            }
            else if (Directory.Exists(targetPath) && File.GetAttributes(targetPath).HasFlag(FileAttributes.Directory))
            {
                targetPath = Path.Combine(targetPath, GetTargetFileName(sourcePath));
            }
            else if (LooksLikeDirectoryPath(targetPath))
            {
                Directory.CreateDirectory(targetPath);
                targetPath = Path.Combine(targetPath, GetTargetFileName(sourcePath));
            }
            if (File.Exists(targetPath))
            {
                var applicationPath = Process.GetCurrentProcess().MainModule.FileName;
                var targetFileLastUpdateDateTime = new FileInfo(targetPath).LastWriteTimeUtc;
                if (new FileInfo(sourcePath).LastWriteTimeUtc < targetFileLastUpdateDateTime && new FileInfo(applicationPath).LastWriteTimeUtc < targetFileLastUpdateDateTime)
                {
                    return true;
                }
            }
            File.WriteAllText(targetPath, _transformer.Transform(File.ReadAllText(sourcePath, Encoding.UTF8), new Context(sourcePath)), Encoding.UTF8);
            message = $"{targetPath} file written.";
            return true;
        }

        private static string GetTargetFileName(string sourcePath) => ChangeToTargetExtension(Path.GetFileName(sourcePath));

        private static string ChangeToTargetExtension(string path) => Path.ChangeExtension(path, ".cpp");

        private static bool LooksLikeDirectoryPath(string targetPath) => targetPath.EndsWith(Path.DirectorySeparatorChar) || targetPath.EndsWith(Path.AltDirectorySeparatorChar);

        private static string GetArgOrDefault(string[] args, int index) => args.Length > index ? args[index] : null;
    }
}
