using SQLScriptsExplorer.Addin.Infrastructure;
using SQLScriptsExplorer.Addin.Models;
using SQLScriptsExplorer.Addin.Models.Enums;
using SQLScriptsExplorer.Addin.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace SQLScriptsExplorer.Addin.Repository
{
    public class SettingsRepository : ISettingsRepository
    {
        private const string FOLDER_MAPPING = "FolderMapping";
        private const string SQL_PARSER = "SQLParser";
        private const string EXPAND_MAPPEDFOLDERS_ONLOAD = "ExpandMappedFoldersOnLoad";
        private const string ALLOWED_FILE_TYPES = "AllowedFileTypes";
        private const string SHOW_EXECUTEFILE_BUTTON = "ShowExecuteFileButton";
        private const string CONFIRM_SCRIPT_EXECUTION = "ConfirmScriptExecution";
        private const string SCRIPT_FILE_DOUBLE_CLICK_BEHAVIOUR = "ScriptFileDoubleClickBehaviour";

        public List<FolderMapping> FolderMapping { get; set; }

        public string SQLParserVersion { get; set; }

        public bool ExpandMappedFoldersOnLoad { get; set; }

        public string AllowedFileTypes { get; set; }

        public bool ShowExecuteFileButton { get; set; }

        public bool ConfirmScriptExecution { get; set; }

        public ScriptFileDoubleClickBehaviour ScriptFileDoubleClickBehaviour { get; set; }

        public SettingsRepository()
        {
            Refresh();
        }

        public void Refresh()
        {
            LoadFolderMapping();
            LoadFileExplorerSettings();
            LoadGenericSettings();
        }

        public void Save()
        {
            var jsonFolderMapping = Newtonsoft.Json.JsonConvert.SerializeObject(FolderMapping);

            RegistryManager.SaveRegisterValue(FOLDER_MAPPING, jsonFolderMapping);
            RegistryManager.SaveRegisterValue(SQL_PARSER, SQLParserVersion);
            RegistryManager.SaveRegisterValue(EXPAND_MAPPEDFOLDERS_ONLOAD, ExpandMappedFoldersOnLoad.ToString());
            RegistryManager.SaveRegisterValue(ALLOWED_FILE_TYPES, AllowedFileTypes);
            RegistryManager.SaveRegisterValue(SHOW_EXECUTEFILE_BUTTON, ShowExecuteFileButton.ToString());
            RegistryManager.SaveRegisterValue(CONFIRM_SCRIPT_EXECUTION, ConfirmScriptExecution.ToString());
            RegistryManager.SaveRegisterValue(SCRIPT_FILE_DOUBLE_CLICK_BEHAVIOUR, $"{(int)ScriptFileDoubleClickBehaviour}");
        }

        private void LoadFolderMapping()
        {
            FolderMapping = null;

            string folderMappingRegister = RegistryManager.GetRegisterValue(FOLDER_MAPPING);

            try
            {
                FolderMapping = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FolderMapping>>(folderMappingRegister);
            }
            catch
            {
                throw new Exception("Could not load your folders mapping configuration, it has now been reset.");
            }

            if (FolderMapping == null || FolderMapping.Count == 0)
            {
                FolderMapping = new List<FolderMapping>();

                var folderMapping = new FolderMapping()
                {
                    Alias = "Personal",
                    FolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "SQLScriptsExplorer"),
                };

                FolderMapping.Add(folderMapping);
            }
        }

        private void LoadFileExplorerSettings()
        {
            // Show Execute File Button and 
            var showExecuteFileButton = RegistryManager.GetRegisterValue(SHOW_EXECUTEFILE_BUTTON);
            if (string.IsNullOrEmpty(showExecuteFileButton))
                ShowExecuteFileButton = true;
            else
                ShowExecuteFileButton = bool.Parse(showExecuteFileButton);

            // Confirm Script Execution
            var confirmScriptExecution = RegistryManager.GetRegisterValue(CONFIRM_SCRIPT_EXECUTION);
            if (string.IsNullOrEmpty(confirmScriptExecution))
                ConfirmScriptExecution = true;
            else
                ConfirmScriptExecution = bool.Parse(confirmScriptExecution);

            // Script File Double Click Behaviour
            var scriptFileDoubleClickBehaviour = RegistryManager.GetRegisterValue(SCRIPT_FILE_DOUBLE_CLICK_BEHAVIOUR);
            if (string.IsNullOrEmpty(scriptFileDoubleClickBehaviour))
                ScriptFileDoubleClickBehaviour = ScriptFileDoubleClickBehaviour.OpenNewInstance;
            else
                ScriptFileDoubleClickBehaviour = (ScriptFileDoubleClickBehaviour)int.Parse(scriptFileDoubleClickBehaviour);
        }

        private void LoadGenericSettings()
        {
            // SQL Parser Version
            SQLParserVersion = RegistryManager.GetRegisterValue(SQL_PARSER);
            if (string.IsNullOrEmpty(SQLParserVersion))
            {
                SQLParserVersion = "SQL Server 2019";
            }

            // Expand Mapped Folders OnLoad
            var expandMappedFoldersOnLoad = RegistryManager.GetRegisterValue(EXPAND_MAPPEDFOLDERS_ONLOAD);
            if (string.IsNullOrEmpty(expandMappedFoldersOnLoad))
                ExpandMappedFoldersOnLoad = true;
            else
                ExpandMappedFoldersOnLoad = bool.Parse(expandMappedFoldersOnLoad);

            // Allowed File Types
            AllowedFileTypes = RegistryManager.GetRegisterValue(ALLOWED_FILE_TYPES);
            if (string.IsNullOrEmpty(AllowedFileTypes))
            {
                AllowedFileTypes = "*.sql";
            }
        }
    }
}
