using System.IO;
using System.Linq;

namespace Katil.Common.Utilities
{
    public static class FileUtils
    {
        public static string GetFileExtension(string filename)
        {
            if (!filename.Contains("."))
            {
                return string.Empty;
            }

            return filename.Split(".").LastOrDefault();
        }

        public static void CheckIfNotExistsCreate(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new InvalidDataException("Wrong file path");
            }

            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
