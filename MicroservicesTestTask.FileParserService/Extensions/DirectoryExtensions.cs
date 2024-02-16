namespace MicroservicesTestTask.FileParserService.Extensions
{
    internal static class DirectoryExtensions
    {
        public static void EnsureDirectoryExists(string directory)
        {
            if (!Directory.Exists(directory))
            {
                _ = Directory.CreateDirectory(directory);
            }
        }
    }
}
