using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;

using ImageViewer.Bases;
using ImageViewer.Data;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ImageViewer.Viewers
{
    public partial class ImageViewerViewModel : ViewModelBase
    {
        #region Fields
        private IFileController _controller;
        #endregion

        #region UI Variable
        [ObservableProperty]
        private string _imagePath;
        #endregion

        public ImageViewerViewModel()
        {
            _controller = Ioc.Default.GetService<IFileController>();
            _controller.Changed += OnControlChanged;
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

            _controller.LoadImages(dropItem);
        }

        #region Public Methods

        #endregion

        #region Private Methods
        private void OnControlChanged(object? sender, FileChangedEventArgs e)
        {
            ImagePath = e.FileName;
        }
        #endregion
    }
}
