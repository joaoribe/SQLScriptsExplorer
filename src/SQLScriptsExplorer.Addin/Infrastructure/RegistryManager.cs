using Microsoft.Win32;
using System;

namespace SQLScriptsExplorer.Addin.Infrastructure
{
    public static class RegistryManager
    {
        private static RegistryKey RootRegistry
        {
            get
            {
                var settingKeyRoot = Registry.CurrentUser.CreateSubKey("SQLScriptsExplorer.Addin");
                var settings = settingKeyRoot.CreateSubKey("Settings");

                return settings;
            }
        }

        public static string GetRegisterValue(string name)
        {
            string value = string.Empty;

            try
            {
                value = RootRegistry.GetValue(name).ToString();
            }
            catch { }

            return value;
        }

        public static void SaveRegisterValue(string name, string value)
        {
            try
            {
                RootRegistry.SetValue(name, value);
                RootRegistry.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
