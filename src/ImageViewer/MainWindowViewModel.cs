using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using ImageViewer.Bases;
using ImageViewer.PopupWindows;
using ImageViewer.Services;

using System.Windows.Input;

namespace ImageViewer
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly IDialogService dialogService;
        public MainWindowViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;
        }

        [ObservableProperty]
        private string _title;

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
        private void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {

            }
        }
        #endregion

        #region Methods
        private void OpenSettingWindow()
        {
            // Popup1View 팝업 띄우기
            dialogService.Show(new PopupViewModel(), "HI", 500, 650, typeof(PopupWindow));
            //dialogService.Dialog.ShowDialog();
        }

        private void MoveImage()
        {
            // 이전 사진으로 이동.
            //string prevImage = controlImage(_currentImagePath, ImageList, -1);
            //_currentImagePath = prevImage;
            //showView(prevImage);
        }
        #endregion
    }
}
