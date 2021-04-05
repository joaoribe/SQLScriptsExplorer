using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace SQLScriptsExplorer.Addin.Infrastructure
{
    public static class Utils
    {
        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();

        public static string GenerateID(int length)
        {
            lock (syncLock)
            {
                var characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
                var result = new char[length];

                for (int i = 0; i < length; i++)
                {
                    var randomCharacter = random.Next(characters.Length);

                    result[i] = characters[randomCharacter];
                }

                return new string(result);
            }
        }

        public static BitmapImage GetResourceImage(string name)
        {
            try
            {
                using (var memory = new MemoryStream())
                {

                    var bitmap = (Bitmap)Properties.Resources.ResourceManager.GetObject(name);
                    bitmap.Save(memory, ImageFormat.Png);

                    memory.Position = 0L;
                    var bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = memory;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();

                    return bitmapImage;
                }
            }
            catch (Exception ex)
            {
            }

            return default;
        }
    }
}
