using CommunityToolkit.Mvvm.DependencyInjection;

using ImageViewer.Viewers;
using ImageViewer.Viewers.EditorViewers;

namespace ImageViewer.Components
{
    public sealed class ViewModelLocator
    {
        public MainWindowViewModel? MainWindowViewModel
            => Ioc.Default.GetService<MainWindowViewModel>();

        public ImageViewerViewModel? ImageViewerViewModel
            => Ioc.Default.GetService<ImageViewerViewModel>();

        public ImageResizeViewModel? ImageResizeViewerViewModel
            => Ioc.Default.GetService<ImageResizeViewModel>();

        public ImageCropViewModel? ImageCropViewModel
            => Ioc.Default.GetService<ImageCropViewModel>();

        public FileRenameViewModel? FileRenameViewModel
            => Ioc.Default.GetService<FileRenameViewModel>();
    }
}
