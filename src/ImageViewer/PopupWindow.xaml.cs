using ImageViewer.Bases;
using ImageViewer.PopupWindows;

using Wpf.Ui.Controls;

namespace ImageViewer
{
    /// <summary>
    /// PopupWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PopupWindow : FluentWindow, IDialog
    {
        public PopupWindow()
        {
            this.DataContext = new PopupViewModel();
            InitializeComponent();
        }

        public Action? CloseCallback { get; set; }


    }
}
