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

using Wpf.Ui.Controls;

namespace ImageViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : FluentWindow
    {
        private Key _tempKey;
        public Dictionary<Key, string> KeyDictionary { get; set; } = new Dictionary<Key, string>();

        public MainWindow()
        {
            InitializeComponent();

            //SettingItems.ItemsSource = KeyDictionary;
        }

        //private void SettingAddingBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    if (string.IsNullOrEmpty(KeyTbx.Text) || string.IsNullOrEmpty(SaveDirPathTbx.Text))
        //        return;

        //    if (KeyDictionary.ContainsKey(_tempKey))
        //    {
        //        System.Windows.MessageBox.Show("중복된 단축키 존재");
        //        return;
        //    }

        //    KeyDictionary.Add(_tempKey, SaveDirPathTbx.Text);
        //    SaveDirPathTbx.Text = string.Empty;
        //    KeyTbx.Text = string.Empty;

        //    // SettingItems.Items.Refresh();
        //    SettingItems.ItemsSource = KeyDictionary;
        //    SettingItems.Items.Refresh();
        //}

        //private void KeyTbx_KeyDown(object sender, KeyEventArgs e)
        //{
        //    KeyTbx.Text = string.Empty;
        //    e.Handled = true;

        //    _tempKey = e.Key;
        //    KeyTbx.Text = _tempKey.ToString();
        //}

        //private void SettingDeleteBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    Wpf.Ui.Controls.Button? btn = sender as Wpf.Ui.Controls.Button;
        //    SettingItems.Items.Refresh();
        //}

        private void SettingssDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Delete");
        }
    }
}