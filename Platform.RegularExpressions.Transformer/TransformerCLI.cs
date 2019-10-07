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
                targetPath = Path.ChangeExtension(sourcePath, ".cpp");
            }
            else if ((Directory.Exists(targetPath) && File.GetAttributes(targetPath).HasFlag(FileAttributes.Directory)) || LooksLikeDirectoryPath(targetPath))
            {
                targetPath = Path.Combine(targetPath, Path.ChangeExtension(Path.GetFileName(sourcePath), ".cpp"));
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

        private static bool LooksLikeDirectoryPath(string targetPath) => targetPath.EndsWith(Path.DirectorySeparatorChar) || targetPath.EndsWith(Path.AltDirectorySeparatorChar);

        private static string GetArgOrDefault(string[] args, int index) => args.Length > index ? args[index] : null;
    }
}
