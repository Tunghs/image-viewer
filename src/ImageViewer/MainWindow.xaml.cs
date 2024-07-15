using CommunityToolkit.Mvvm.DependencyInjection;

using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Wpf.Ui;
using Wpf.Ui.Controls;

namespace ImageViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
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