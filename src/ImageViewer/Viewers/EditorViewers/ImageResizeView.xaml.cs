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

namespace ImageViewer.Viewers.EditorViewers
{
    /// <summary>
    /// ImageResizeView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ImageResizeView : UserControl
    {
        public ImageResizeView()
        {
            InitializeComponent();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton? btn = e.Source as RadioButton;
            int a = 0;
            MessageBox.Show(btn.Content.ToString());
        }
    }
}
