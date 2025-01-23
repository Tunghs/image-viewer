using ImageViewer.Core.ImageProcessing;

using System.Windows.Controls.Primitives;

namespace ImageViewer.Services
{
    public interface IImageProcessingService
    {
        public void Crop(string src, string dst);
        public (int, int) GetImageSize(string filePath);
        public void Resize(string src, string dst, int targetWidht, int targetHeight);

        public void Resize(string src, string dst, double ratio);
    }

    public class ImageProcessingService : IImageProcessingService
    {
        #region Fields
        private ImageSimpleEditor _simpleEditor = new ImageSimpleEditor();
        #endregion

        public void Crop(string src, string dst)
        {
            
        }

        public (int, int) GetImageSize(string filePath)
        {
            return _simpleEditor.GetImageSize(filePath);
        }

        public void Resize(string src, string dst, int targetWidht, int targetHeight)
        {
            _simpleEditor.Resize(src, dst, targetWidht, targetHeight);
        }

        public void Resize(string src, string dst, double ratio)
        {
            _simpleEditor.Resize(src, dst, ratio);
        }
    }
}
