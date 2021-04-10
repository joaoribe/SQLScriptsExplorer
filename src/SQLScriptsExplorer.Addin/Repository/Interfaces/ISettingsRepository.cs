using SQLScriptsExplorer.Addin.Models;
using System.Collections.Generic;

namespace SQLScriptsExplorer.Addin.Repository.Interfaces
{
    public interface ISettingsRepository
    {
        List<FolderMapping> FolderMapping { get; set; }
        string SQLParserVersion { get; set; }
        string AllowedFileTypes { get; set; }
        bool ExpandMappedFoldersOnLoad { get; set; }
        bool ShowExecuteFileButton { get; set; }
        bool ConfirmScriptExecution { get; set; }

        void Refresh();
        void Save();
    }
}
