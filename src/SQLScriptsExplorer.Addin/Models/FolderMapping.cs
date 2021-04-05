using System.Drawing;
using System.IO;

namespace SQLScriptsExplorer.Addin.Models
{
    public class FolderMapping
    {
        public string Alias { get; set; }
        public string FolderPath { get; set; }
        public Image IsValid
        {
            get
            {
                
                Bitmap imageError = (Bitmap)Properties.Resources.ResourceManager.GetObject("Error");

                if (string.IsNullOrWhiteSpace(Alias))
                    return imageError;

                if (string.IsNullOrWhiteSpace(FolderPath) || !Directory.Exists(FolderPath))
                    return imageError;

                return (Bitmap)Properties.Resources.ResourceManager.GetObject("Success");
            }
        }
    }
}
