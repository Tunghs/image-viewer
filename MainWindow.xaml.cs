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
using Microsoft.WindowsAPICodePack.Dialogs;

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
        private static List<string> _ImagePathList = null;

        // ---- 다이얼로그 구현부 ----
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
        // 폴더 불러오기
        private string OpenFolderDialog()
        {
            CommonOpenFileDialog dlg = new CommonOpenFileDialog();
            dlg.InitialDirectory = "c:\\";
            dlg.IsFolderPicker = true;
            string path = null;

            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                path = dlg.FileName;
            }
            return path;
        }

        // 파일 경로를 받아서 동일 디렉토리 내 모든 파일 경로 반환
        private List<string> GetFilePathList(string filePath)
        {
            //리스트뷰 클리어
            List<string> filePathList = new List<string>();
            // 전체 경로에서 디렉토리 Path 추출
            string path = System.IO.Path.GetDirectoryName(filePath);

            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(path);
            foreach (System.IO.FileInfo File in dir.GetFiles())
            {
                filePathList.Add(File.FullName);
            }
            return filePathList;
        }

        private string contralImage(string path, List<string> list, int no)
        {
            int num = list.IndexOf(path) + no;
            

            if (num == -1)
            {
                MessageBox.Show("첫번째 이미지입니다.");
                return path;
            }
            else if(num == list.Count)
            {
                MessageBox.Show("마지막 이미지입니다.");
                return path;
            }
            else
            {
                string resultImagePath = list[num];
                return resultImagePath;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        // ---- 방향키로 이미지 이동 ----
        private void onKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                // 이전 사진으로 이동.
                string prevImage = contralImage(_ImagePath, _ImagePathList, -1);
                _ImagePath = prevImage;
                ViewBox.Source = new BitmapImage(new Uri(_ImagePath));

            }
            if (e.Key == Key.Right)
            {
                //다음 사진으로 이동.
                string nextImage = contralImage(_ImagePath, _ImagePathList, 1);
                _ImagePath = nextImage;
                ViewBox.Source = new BitmapImage(new Uri(_ImagePath));
            }

            //--핫키--
            if (e.Key == Key.Q)
            {
                string fileName = System.IO.Path.GetFileName(_ImagePath);
                string afterfolderPath = hotKeyPath_1.Text + @"\" + fileName;

                // 다음 사진으로 이동
                string nextImage = contralImage(_ImagePath, _ImagePathList, 1);
                ViewBox.Source = new BitmapImage(new Uri(nextImage));

                moveFile(_ImagePath, afterfolderPath);

                _ImagePath = nextImage;

                
            }
        }

        private void moveFile(string aPath, string bPath)
        {
            System.IO.File.Move(aPath, bPath);
        }

        // 메뉴 - 파일 불러오기
        private void fileOpen_Click(object sender, RoutedEventArgs e)
        {
            _ImagePath = OpenFileDialog();
            ViewBox.Source = new BitmapImage(new Uri(_ImagePath));

            th(_ImagePath);
        }

        private void th(string path)
        {
            ViewBox.Source = new BitmapImage(new Uri(path));
            currentLocationTxt.Text = System.IO.Path.GetDirectoryName(path);

            //listViewClear();
            _ImagePathList = GetFilePathList(path);
            AddImageListView(_ImagePathList);

            textDropBlock.Visibility = System.Windows.Visibility.Hidden;
            DropArea.Visibility = System.Windows.Visibility.Hidden;
        }

        // ---- 이미지 드래그 구현부 ----
        // drag 인식 확인
        private void file_DragEnter(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.All;
        }
        // drop 구현
        private void file_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            _ImagePath = files[0];
            th(_ImagePath);
        }
        // 마우스 포인터가 drag 영역에 들어 왔을 때 처리
        private void DropArea_DragOver(object sender, DragEventArgs e)
        {
            DropArea.Background = new SolidColorBrush(Color.FromArgb(255, 189, 189, 189));
        }
        // 마우스 포인터가 drag 영역에 나갔을 때 처리
        private void DropArea_DragLeave(object sender, DragEventArgs e)
        {
            DropArea.Background = new SolidColorBrush(Color.FromArgb(255, 234, 234, 234));
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

        // ---- 사이드 목록 구현부 ---
        private void AddImageListView(List<string> list)
        {
            int i = 1;
            foreach (string filePath in list)
            {
                string fileName = System.IO.Path.GetFileName(filePath);

                MyData.GetInstance().Add(new MyData() { dataNo = i, dataName = fileName });
                i++;
            }
            imageListView.ItemsSource = MyData.GetInstance();
        }
        private void listViewClear()
        {
            foreach (MyData item in imageListView.SelectedItems)
            {
                MyData.GetInstance().Remove(item);
            }
            imageListView.Items.Refresh();
        }

        // ---- 사이드 단축키 구현부 ----
        private void hotKey_1Btn_Click(object sender, RoutedEventArgs e)
        {
            hotKeyPath_1.Text = OpenFolderDialog();
        }
        private void hotKey_2Btn_Click(object sender, RoutedEventArgs e)
        {
            hotKeyPath_2.Text = OpenFolderDialog();
        }

        private void hotKey_3Btn_Click(object sender, RoutedEventArgs e)
        {
            hotKeyPath_3.Text = OpenFolderDialog();
        }

        private void hotKey_4Btn_Click(object sender, RoutedEventArgs e)
        {
            hotKeyPath_4.Text = OpenFolderDialog();
        }

        private void hotKey_5Btn_Click(object sender, RoutedEventArgs e)
        {
            hotKeyPath_5.Text = OpenFolderDialog();
        }

        private void hotKey_6Btn_Click(object sender, RoutedEventArgs e)
        {
            hotKeyPath_6.Text = OpenFolderDialog();
        }

        private void hotKey_7Btn_Click(object sender, RoutedEventArgs e)
        {
            hotKeyPath_7.Text = OpenFolderDialog();
        }

        private void hotKey_8Btn_Click(object sender, RoutedEventArgs e)
        {
            hotKeyPath_8.Text = OpenFolderDialog();
        }

        private void hotKey_9Btn_Click(object sender, RoutedEventArgs e)
        {
            hotKeyPath_9.Text = OpenFolderDialog();
        }

        private void hotKey_10Btn_Click(object sender, RoutedEventArgs e)
        {
            hotKeyPath_10.Text = OpenFolderDialog();
        }

    }
}
