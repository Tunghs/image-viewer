using CommunityToolkit.Mvvm.DependencyInjection;

using System.Windows.Input;

using Wpf.Ui;
using Wpf.Ui.Controls;

namespace ImageViewer.Windows
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : FluentWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += MainWindow_MouseDown;

            var snackbarService = Ioc.Default.GetService<ISnackbarService>();
            snackbarService.SetSnackbarPresenter(this.SnackbarPresenter);
        }

        private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
