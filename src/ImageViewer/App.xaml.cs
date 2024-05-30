using System.Windows;

namespace ImageViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private List<string> _supportedImageFormat = new List<string>()
        {
            ".jpg", ".jpeg", ".png", ".bmp"
        };

        public App()
        {
            new Bootstrapper();
        }

        public bool IsEnabledFormat(string filename)
        {
            if (_supportedImageFormat.Any(x => filename.ToLower().EndsWith(x)))
            {
                return true;
            }
            return false;
        }
    }

}
