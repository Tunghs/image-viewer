using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Imgae_Viewer
{
    static class FileController
    {
        [DllImport("gdi32")]
        static extern int DeleteObject(IntPtr o);

        static public Bitmap GetBitmapWithImagePath(string imagePath)
        {
            if (!File.Exists(imagePath))
                return null;
            Bitmap resultBitmap;

            using (FileStream fs = new FileStream(imagePath, FileMode.Open))
            {
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, (int)fs.Length);
                MemoryStream ms = new MemoryStream(bytes);
                resultBitmap = Bitmap.FromStream(ms) as Bitmap;
                ms.Dispose();
            }
            return resultBitmap;
        }

        static public BitmapImage GetBitmapImage(Bitmap bitmap)
        {
            BitmapImage resultBitmapImage;
            Bitmap tempBitmap = new Bitmap(bitmap);
            if (tempBitmap == null)
                return null;
            using (MemoryStream memory = new MemoryStream())
            {
                tempBitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                resultBitmapImage = new BitmapImage();
                resultBitmapImage.BeginInit();
                resultBitmapImage.StreamSource = memory;
                resultBitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                resultBitmapImage.EndInit();
                resultBitmapImage.Freeze();
            }
            return resultBitmapImage;
        }

        static public BitmapSource GetBitmapSource(Bitmap bitmap)
        {
            BitmapSource resultBitmapSource = null;
            IntPtr ip = IntPtr.Zero;
            try
            {
                if (bitmap == null)
                    return null;
                ip = bitmap.GetHbitmap();
                resultBitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(ip,
                   IntPtr.Zero, System.Windows.Int32Rect.Empty,
                   System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
                resultBitmapSource?.Freeze();
                bitmap.Dispose();
            }
            catch (Exception e)
            {
            }
            finally
            {
                DeleteObject(ip);
            }
            return resultBitmapSource;
        }
    }

    
}
