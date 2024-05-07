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
        private Dictionary<Key, string> _keyDictionary = new Dictionary<Key, string>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SettingAddingBtn_Click(object sender, RoutedEventArgs e)
        {
            
            // if (_keyDictionary.ContainsKey())
        }
    }
}