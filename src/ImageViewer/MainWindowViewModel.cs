using CommunityToolkit.Mvvm.Input;

using ImageViewer.Bases;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ImageViewer
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly IDialogService _dialogService;
        public MainWindowViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }

        #region Command
        [RelayCommand]
        private void OnButtonClick(string @param)
        {
            switch (@param)
            {
                case "OpenSetting":
                    OpenSettingWindow();
                    break;
                case "Crop":
                    // Crop();
                    break;
                default:
                    break;
            }
        }

        [RelayCommand]
        private void OnFileDrop(DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                return;
            }

            var dropItems = (string[])e.Data.GetData(DataFormats.FileDrop);
            var dropItem = dropItems[0];

            // 파일
            if (File.Exists(dropItem))
            {

            }
            // 폴더
            else if (Directory.Exists(dropItem))
            {

            }
        }
        #endregion

        #region Methods
        private void OpenSettingWindow()
        {
            // Popup1View 팝업 띄우기
            _dialogService.SetVM(new PopupViewModel());
            _dialogService.Dialog.ShowDialog();
        }
        #endregion
    }
}
