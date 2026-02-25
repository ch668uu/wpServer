using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace WallpaperLock
{
    class Program
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        private const int SPI_SETDESKWALLPAPER = 0x0014;
        private const int SPIF_UPDATEINIFILE = 0x01;
        private const int SPIF_SENDWININICHANGE = 0x02;

        static void Main(string[] args)
        {
            try
            {
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string imageFolder = Path.Combine(baseDirectory, "image");

                if (!Directory.Exists(imageFolder))
                {
                    Directory.CreateDirectory(imageFolder);
                    return;
                }

                string[] extensions = { "*.jpg", "*.jpeg", "*.png" };
                var files = extensions.SelectMany(ext => Directory.GetFiles(imageFolder, ext)).ToArray();

                if (files.Length > 0)
                {
                    Random rand = new Random();
                    string randomImagePath = files[rand.Next(files.Length)];

                    //设置壁纸
                    SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, randomImagePath, SPI_SETDESKWALLPAPER | SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "error_log.txt"),
                    DateTime.Now + ": " + ex.Message + "\r\n");
            }
        }
    }
}