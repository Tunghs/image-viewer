using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

using ImageViewer.Viewers;
using ImageViewer.Services;
using ImageViewer.Data;
using Wpf.Ui;
using ImageViewer.Viewers.EditorViewers;

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
            // Snackbar service 등록
            services.AddSingleton<ISnackbarService, SnackbarService>();

            // Viewer ViewModels
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<ImageViewerViewModel>();

            // Setting Viewer viewModels
            services.AddTransient<ImageResizeViewModel>();
            services.AddTransient<ImageCropViewModel>();
            services.AddTransient<FileRenameViewModel>();

            return services.BuildServiceProvider();
        }
    }
}
