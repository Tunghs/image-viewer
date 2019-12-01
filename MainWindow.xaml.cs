using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace Imgae_Viewer
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string _ImagePath = null;

        private string OpenFileDialog()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            Nullable<bool> result = dlg.ShowDialog();

            return dlg.FileName;
        }


        public MainWindow()
        {
            InitializeComponent();
            

        }

        private void fileOpen_Click(object sender, RoutedEventArgs e)
        {
            _ImagePath = OpenFileDialog();
            ViewBox.Source = new BitmapImage(new Uri(_ImagePath));
        }


        // ---- 사이드 애니메이션 구현부 ---
        // 목록 메뉴
        private void btnFileListMenuShow_Click(object sender, RoutedEventArgs e)
        {
            ShowHideMenu("sbShowFileListMenu", btnFileListMenuHide, btnFileListMenuShow, fileListMenu);
            btnSaveKeyMenuShow.Visibility = System.Windows.Visibility.Hidden;
        }

        private void btnFileListMenuHide_Click(object sender, RoutedEventArgs e)
        {
            ShowHideMenu("sbHideFileListMenu", btnFileListMenuHide, btnFileListMenuShow, fileListMenu);
            btnSaveKeyMenuShow.Visibility = System.Windows.Visibility.Visible;
        }
        // 단축키 메뉴
        private void btnSaveKeyMenuShow_Click(object sender, RoutedEventArgs e)
        {
            ShowHideMenu("sbShowSaveKeyMenu", btnSaveKeyMenuHide, btnSaveKeyMenuShow, saveKeyMenu);
            btnFileListMenuShow.Visibility = System.Windows.Visibility.Hidden;
        }

        private void btnSaveKeyMenuHide_Click(object sender, RoutedEventArgs e)
        {
            ShowHideMenu("sbHideSaveKeyMenu", btnSaveKeyMenuHide, btnSaveKeyMenuShow, saveKeyMenu);
            btnFileListMenuShow.Visibility = System.Windows.Visibility.Visible;
        }

        // 애니메이션 작업
        private void ShowHideMenu(string Storyboard, Button btnHide, Button btnShow, StackPanel pnl)
        {
            Storyboard sb = Resources[Storyboard] as Storyboard;
            sb.Begin(pnl);

            if (Storyboard.Contains("Show"))
            {
                btnHide.Visibility = System.Windows.Visibility.Visible;
                btnShow.Visibility = System.Windows.Visibility.Hidden;
            }
            else if (Storyboard.Contains("Hide"))
            {
                btnHide.Visibility = System.Windows.Visibility.Hidden;
                btnShow.Visibility = System.Windows.Visibility.Visible;
            }
        }
    }
}
