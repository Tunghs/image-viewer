using CommunityToolkit.Mvvm.ComponentModel;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ImageViewer.Data
{
    public partial class ImageInfo : ObservableObject
    {
        [ObservableProperty]
        private string _width;

        [ObservableProperty]
        private string _height;

        [ObservableProperty]
        private string _format;

        [ObservableProperty]
        private string _size;

        public ImageInfo(string filePath)
        {
            FileInfo file = new FileInfo(filePath);
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                BitmapSource img = BitmapFrame.Create(fs);
                
            }
        }
    }
}
