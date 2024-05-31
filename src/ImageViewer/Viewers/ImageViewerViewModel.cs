using CommunityToolkit.Mvvm.Input;

using ImageViewer.Bases;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ImageViewer.Viewers
{
    public partial class ImageViewerViewModel : ViewModelBase
    {
        #region Fields
        private List<string> _supportedImageFormat = new List<string>()
        {
            ".jpg", ".jpeg", ".png", ".bmp"
        };
        #endregion

        #region Properties
        public List<string> Files { get; private set; }
        #endregion

        [RelayCommand]
        private void OnFileDrop(DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                return;
            }

            var dropItems = (string[])e.Data.GetData(DataFormats.FileDrop);
            var dropItem = dropItems[0];

            // 파일
            if (File.Exists(dropItem))
            {

            }
            // 폴더
            else if (Directory.Exists(dropItem))
            {

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

        }

        public void AddFilesFromDirectoryPath(string directoryPath)
        {

        }
        #endregion
    }
}
