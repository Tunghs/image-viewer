using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using ImageViewer.Bases;
using ImageViewer.Services;

using System.Windows;

namespace ImageViewer.Viewers
{
    public partial class ImageViewerViewModel : ViewModelBase
    {
        #region Fields

        private IFileControlService _controller;

        #endregion Fields

        #region UI Variable

        [ObservableProperty]
        private string _imagePath;

        #endregion UI Variable

        public ImageViewerViewModel(IFileControlService fileControlService)
        {
            _controller = fileControlService;
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

        #region Private Methods

        private void OnControlChanged(object? sender, FileChangedEventArgs e)
        {
            ImagePath = e.FileName;
        }

        #endregion Private Methods
    }
}