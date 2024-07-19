using OpenCvSharp;

using System;
using System.Collections.Generic;
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
    }
}
