using CommunityToolkit.Mvvm.DependencyInjection;

using ImageViewer.Services;
using ImageViewer.Viewers;
using ImageViewer.Windows;

using Microsoft.Extensions.DependencyInjection;

using Wpf.Ui;

namespace ImageViewer
{
    public class Bootstrapper
    {
        public Bootstrapper()
        {
            var services = ConfigureServices();
            Ioc.Default.ConfigureServices(services);
        }

        /// <summary>
        /// Configures the services for the application.
        /// </summary>
        /// <returns></returns>
        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Services
            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<IFileControlService, FileControlService>();
            services.AddSingleton<IImageProcessingService, ImageProcessingService>();

            // WPF-UI Snackbar service 등록
            services.AddSingleton<ISnackbarService, SnackbarService>();

            // Viewer ViewModels
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<ImageViewerViewModel>();

            return services.BuildServiceProvider();
        }
    }
}