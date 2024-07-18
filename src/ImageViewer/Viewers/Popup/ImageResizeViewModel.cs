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
using System.IO;

namespace ImageViewer.Viewers.Popup
{
    public partial class ImageResizeViewModel : PopupDialogViewModelBase
    {
        #region UI Properties
        [ObservableProperty]
        private Visibility _percentageSettingVisibility = Visibility.Collapsed;

        [ObservableProperty]
        private List<string> _images = new List<string>();
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
                case "AddImage":

                    break;
                case "AddFolder":

                    break;
                default:
                    break;
            }
        }

        [RelayCommand]
        public void OnFileDrop(DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                return;
            }

            var dropItems = (string[])e.Data.GetData(DataFormats.FileDrop);
            for (int index = 0;  index < dropItems.Length; index++)
            {
                if (IsDirectory(dropItems[index]))
                {

                }
                else
                {
                    Images.Add(dropItems[index]);
                }
            }
        }
        #endregion

        #region Methods
        private bool IsDirectory(string path)
        {
            if (path == null)
            {
                return false;
            }

            FileAttributes attr = File.GetAttributes(path);

            if (attr.HasFlag(FileAttributes.Directory))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
