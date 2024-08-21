using OpenCvSharp;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Core.ImageProcessing
{
    public class ImageSimpleEditor
    {

        public (int width, int height) GetImageSize(string filePath)
        {
            using (Mat img = Cv2.ImRead(filePath, ImreadModes.Unchanged))
            {
                return (img.Width, img.Height);
            }
        }

        public void Resize(string srcPath, string dstPath, int width, int height)
        {
            using (Mat img = Cv2.ImRead(srcPath, ImreadModes.Unchanged))
            using (Mat dst = img.Resize(new OpenCvSharp.Size(width, height), 0, 0, InterpolationFlags.Cubic))
            {
                Cv2.ImWrite(dstPath ,dst);
            }
        }
    }
}
