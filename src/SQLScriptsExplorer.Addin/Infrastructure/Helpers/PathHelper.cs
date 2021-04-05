using System.IO;

namespace SQLScriptsExplorer.Addin.Infrastructure.Helpers
{
    public static class PathHelper
    {
        public static string GetDirectoryTemporaryPath(string path)
        {
            do
            {
                path = $"{path}_{Utils.GenerateID(7)}";

            } while (Directory.Exists(path));

            return path;
        }

        public static bool IsNetworkPath(string path)
        {
            if (!path.StartsWith(@"/") && !path.StartsWith(@"\"))
            {
                string rootPath = Path.GetPathRoot(path);
                DriveInfo driveInfo = new DriveInfo(rootPath);

                return driveInfo.DriveType == DriveType.Network;
            }

            return true;
        }

        public static string GetNextNewFileNameAvailable(string path)
        {
            var fileCount = 1;
            var fileName = string.Empty;
            var fileFullPath = string.Empty;

            do
            {
                fileName = $"New File {fileCount++}.sql";
                fileFullPath = Path.Combine(path, fileName);
            } while (File.Exists(fileFullPath));

            return fileName;
        }

        public static string GetNextNewFolderNameAvailable(string path)
        {
            var directoryCount = 1;
            var directoryName = string.Empty;
            var directoryFullPath = string.Empty;

            do
            {
                directoryName = $"New Folder {directoryCount++}";
                directoryFullPath = Path.Combine(path, directoryName);
            } while (Directory.Exists(directoryFullPath));

            return directoryName;
        }
    }
}
