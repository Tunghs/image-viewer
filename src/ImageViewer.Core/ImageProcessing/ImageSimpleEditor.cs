using OpenCvSharp;

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
                Cv2.ImWrite(dstPath, dst);
            }
        }

        public void Resize(string srcPath, string dstPath, double ratio)
        {
            using (Mat img = Cv2.ImRead(srcPath, ImreadModes.Unchanged))
            using (Mat dst = img.Resize(new OpenCvSharp.Size((int)(img.Width * ratio), (int)(img.Height * ratio)), 0, 0, InterpolationFlags.Cubic))
            {
                Cv2.ImWrite(dstPath, dst);
            }
        }
    }
}