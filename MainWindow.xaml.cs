using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using OpenCvSharp;

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
    public partial class MainWindow : System.Windows.Window
    {
        private static string _ImagePath = null;
        private static List<string> _ImagePathList = null;
        private static int _InvertCheck = 1;

        // ---- 다이얼로그 구현부 ----
        private string OpenFileDialog()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.InitialDirectory = "c:\\";

            if (dlg.ShowDialog() == true)
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

        private string controlImage(string path, List<string> list, int no)
        {
            int num = list.IndexOf(path) + no;


            if (num == -1)
            {
                MessageBox.Show("첫번째 이미지입니다.");
                return path;
            }
            else if (num == list.Count)
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
            try
            {
                if (e.Key == Key.Left)
                {
                    // 이전 사진으로 이동.
                    string prevImage = controlImage(_ImagePath, _ImagePathList, -1);
                    _ImagePath = prevImage;
                    showView(prevImage);

                }
                if (e.Key == Key.Right)
                {
                    //다음 사진으로 이동.
                    string nextImage = controlImage(_ImagePath, _ImagePathList, 1);
                    _ImagePath = nextImage;
                    showView(nextImage);
                }

                //--핫키--
                if (e.Key == Key.Q)
                {
                    string fileName = System.IO.Path.GetFileName(_ImagePath);
                    string afterfolderPath = hotKeyPath_1.Text + @"\" + fileName;

                    afterMoveWork(_ImagePath, afterfolderPath, _ImagePathList);
                }

                if (e.Key == Key.W)
                {
                    string fileName = System.IO.Path.GetFileName(_ImagePath);
                    string afterfolderPath = hotKeyPath_2.Text + @"\" + fileName;

                    afterMoveWork(_ImagePath, afterfolderPath, _ImagePathList);
                }

                if (e.Key == Key.E)
                {
                    string fileName = System.IO.Path.GetFileName(_ImagePath);
                    string afterfolderPath = hotKeyPath_3.Text + @"\" + fileName;

                    afterMoveWork(_ImagePath, afterfolderPath, _ImagePathList);
                }
            }
            catch
            {
                MessageBox.Show("error");
            }
        }

        // 파일 이동 후 처리
        private void afterMoveWork(string source, string target, List<string> list)
        {
            // 다음 사진으로 이동
            string nextImage = controlImage(source, list, 1);
            showView(nextImage);

            int num = _ImagePathList.IndexOf(source);
            _ImagePathList.Remove(_ImagePathList[num]);

            moveFile(source, target);
            _ImagePath = nextImage;
        }

        // 파일 이동
        private void moveFile(string aPath, string bPath)
        {
            if (File.Exists(aPath))
            {
                FileInfo oldFile = new FileInfo(aPath);
                oldFile.MoveTo(bPath);
            }
        }

        // 메뉴 - 파일 불러오기
        private void fileOpen_Click(object sender, RoutedEventArgs e)
        {
            _ImagePath = OpenFileDialog();
            showView(_ImagePath);

            Chores(_ImagePath);
        }

        // 이미지 뷰
        private void showView(string path)
        {
            int num = 0;
            int allnum = 0;

            if (_ImagePathList != null)
            {
                num = _ImagePathList.IndexOf(path) + 1;
                allnum = _ImagePathList.Count;
            }   
            Title = Path.GetFileName(path) + " (" + num.ToString() + "/" + allnum.ToString() + ")";

            if (_InvertCheck == 1)
            {
                Bitmap bitmap = FileController.GetBitmapWithImagePath(path);
                ViewBox.Source = FileController.GetBitmapSource(bitmap);
            }
            else if(_InvertCheck == -1)
            {
                ViewBox.Source = FileController.GetBitmapSource(InvertImage(path));
            }
        }

        //잡일 처리 -- 나중에 수정--
        private void Chores(string path)
        {
            showView(path);
            currentLocationTxt.Text = System.IO.Path.GetDirectoryName(path);

            //listViewClear();
            _ImagePathList = GetFilePathList(path);
            AddImageListView(_ImagePathList);

            textDropBlock.Visibility = System.Windows.Visibility.Hidden;
            DropArea.Visibility = System.Windows.Visibility.Hidden;
        }

        // ---- 이미지 역상 활성화 ----
        private void runInvertView_Click(object sender, RoutedEventArgs e)
        {
            _InvertCheck = _InvertCheck * -1;
        }   

        // ---- 이미지 역상 -----
        private Bitmap InvertImage(string path)
        {
            Mat src = new Mat(path, ImreadModes.Unchanged);
            Mat dst = new Mat();

            Cv2.BitwiseNot(src, dst);
            Bitmap bitDst = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(dst);

            return bitDst;
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
            Chores(_ImagePath);
        }
        // 마우스 포인터가 drag 영역에 들어 왔을 때 처리
        private void DropArea_DragOver(object sender, DragEventArgs e)
        {
            DropArea.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 189, 189, 189));
        }
        // 마우스 포인터가 drag 영역에 나갔을 때 처리
        private void DropArea_DragLeave(object sender, DragEventArgs e)
        {
            DropArea.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 234, 234, 234));
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

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
