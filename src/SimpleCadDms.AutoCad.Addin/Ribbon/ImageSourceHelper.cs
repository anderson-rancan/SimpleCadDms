using System.Windows.Media.Imaging;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace SimpleCadDms.AutoCad.Addin.Ribbon
{
    public static class ImageSourceHelper
    {
        public static BitmapImage GetBitmap(Bitmap image)
        {
            using (var stream = new MemoryStream())
            {
                image.Save(stream, ImageFormat.Png);

                var bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.StreamSource = stream;
                bmp.EndInit();

                return bmp;
            }
        }
    }
}
