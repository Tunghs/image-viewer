using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using ImageViewer.Bases;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace ImageViewer.Viewers.EditorViewers
{
    public partial class ImageResizeViewModel : ViewModelBase
    {
        #region UI Properties
        [ObservableProperty]
        private Visibility _percentageSettingVisibility = Visibility.Collapsed;
        #endregion

        #region Command
        [RelayCommand]
        private void OnSizeRbtnClick(RoutedEventArgs e)
        {
            RadioButton? btn = e.Source as RadioButton;
            int a = 0;
            MessageBox.Show(btn.Content.ToString());
        }

        [RelayCommand]
        private void OnButtonClick(string @param)
        {
            switch (@param)
            {
                case "Run":
                    MessageBox.Show("Click");
                    break;
                default:
                    break;
            }
        }
        #endregion

    }
}
