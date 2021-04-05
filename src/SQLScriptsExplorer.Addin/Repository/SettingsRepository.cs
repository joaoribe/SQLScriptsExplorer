using SQLScriptsExplorer.Addin.Infrastructure;
using SQLScriptsExplorer.Addin.Models;
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

        public List<FolderMapping> FolderMapping { get; set; }

        public string SQLParserVersion { get; set; }

        public bool ExpandMappedFoldersOnLoad { get; set; }

        public string AllowedFileTypes { get; set; }

        public SettingsRepository()
        {
            Refresh();
        }

        public void Refresh()
        {
            LoadFolderMapping();
            LoadOtherSettings();
        }

        public void Save()
        {
            var jsonFolderMapping = Newtonsoft.Json.JsonConvert.SerializeObject(FolderMapping);

            RegistryManager.SaveRegisterValue(FOLDER_MAPPING, jsonFolderMapping);
            RegistryManager.SaveRegisterValue(SQL_PARSER, SQLParserVersion);
            RegistryManager.SaveRegisterValue(EXPAND_MAPPEDFOLDERS_ONLOAD, ExpandMappedFoldersOnLoad.ToString());
            RegistryManager.SaveRegisterValue(ALLOWED_FILE_TYPES, AllowedFileTypes);
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

        private void LoadOtherSettings()
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
