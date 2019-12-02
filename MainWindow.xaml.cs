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
using System.IO;

namespace Imgae_Viewer
{

    class MyData
    {
        public int dataNo { get; set; }
        public string dataName { get; set; }

        private static List<MyData> instance;

        public static List<MyData> GetInstance()
        {
            if (instance == null)
                instance = new List<MyData>();

            return instance;
        }
    }
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string _ImagePath = null;

        // 데이터 바인딩 방법.
        private void data()
        {
            MyData.GetInstance().Add(new MyData() { dataNo = 1, dataName = "fileA" });

            imageListView.ItemsSource = MyData.GetInstance();
        }

        private string OpenFileDialog()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.InitialDirectory = "c:\\";

            if(dlg.ShowDialog() == true)
            {
                return dlg.FileName;
            }
            else
            {
                return null;
            }
        }

        
        public MainWindow()
        {
            InitializeComponent();
        }

        // 파일 열기
        private void fileOpen_Click(object sender, RoutedEventArgs e)
        {
            _ImagePath = OpenFileDialog();
            ViewBox.Source = new BitmapImage(new Uri(_ImagePath));
            // 가져온 이미지 경로 내 모든 이미지들을 리스트에 추가해줌.
        }

        private void file_DragEnter(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
        }

        private void file_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
                ViewBox.Source = new BitmapImage(new Uri(file));
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
