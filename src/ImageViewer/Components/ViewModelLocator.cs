﻿using CommunityToolkit.Mvvm.DependencyInjection;

using ImageViewer.Viewers;
using ImageViewer.Windows;

namespace ImageViewer.Components
{
    public sealed class ViewModelLocator
    {
        public MainWindowViewModel? MainWindowViewModel
            => Ioc.Default.GetService<MainWindowViewModel>();

        public ImageViewerViewModel? ImageViewerViewModel
            => Ioc.Default.GetService<ImageViewerViewModel>();
    }
}
