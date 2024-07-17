using ImageViewer.Bases;
using ImageViewer.PopupWindows;

using Wpf.Ui.Controls;

namespace ImageViewer.Windows
{
    /// <summary>
    /// PopupWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PopupWindow : FluentWindow, IDialog
    {
        public PopupWindow()
        {
            this.DataContext = new PopupViewModel();
            this.Owner = App.Current.MainWindow;
            InitializeComponent();
        }

        public Action? CloseCallback { get; set; }
    }
}
