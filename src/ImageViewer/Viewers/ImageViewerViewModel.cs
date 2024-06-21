using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using ImageViewer.Bases;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ImageViewer.Viewers
{
    public partial class ImageViewerViewModel : ViewModelBase
    {
        #region Fields
        private List<string> _imageFiles = new List<string>();
        private List<string> _supportedImageFormat = new List<string>()
        {
            ".jpg", ".jpeg", ".png", ".bmp"
        };
        #endregion

        #region UI Variable
        [ObservableProperty]
        private string _imagePath;
        #endregion

        public ImageViewerViewModel()
        {
            int a = 0;
        }

        [RelayCommand]
        public void OnFileDrop(DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                return;
            }

            var dropItems = (string[])e.Data.GetData(DataFormats.FileDrop);
            var dropItem = dropItems[0];

            // 폴더
            if (IsDirectory(dropItem))
            {
                AddFilesFromDirectoryPath(dropItem);
                ShowImage(_imageFiles[0]);
            }
            else // 파일
            {
                AddFilesFromFilePath(dropItem);
                ShowImage(dropItem);
            }
        }

        #region Public Methods
        public bool IsEnabledFormat(string filename)
        {
            if (_supportedImageFormat.Any(x => filename.ToLower().EndsWith(x)))
            {
                return true;
            }
            return false;
        }

        public void AddFilesFromFilePath(string filePath)
        {
            string directoryPath = System.IO.Path.GetDirectoryName(filePath);
            AddFilesFromDirectoryPath(directoryPath);
        }

        public void AddFilesFromDirectoryPath(string directoryPath)
        {
            string[] files = Directory.GetFiles(directoryPath, "*.*");
            _imageFiles = files.ToList();
        }
        #endregion

        #region Private Methods
        public bool IsDirectory(string path)
        {
            if (path == null)
            {
                return false;
            }

            FileAttributes attr = File.GetAttributes(path);

            if (attr.HasFlag(FileAttributes.Directory))
            {
                return true;
            }
            else
            { 
                return false;
            }
        }

        private void ShowImage(string filePath)
        {
            ImagePath = filePath;
        }
        #endregion
    }
}
