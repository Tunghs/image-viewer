using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using ImageViewer.Bases;
using ImageViewer.PopupWindows;
using ImageViewer.Services;

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
        #endregion

        #region Methods
        private void OpenSettingWindow()
        {
            // Popup1View 팝업 띄우기
            dialogService.Show(new PopupViewModel(), "HI", 500, 650, typeof(PopupWindow));
            //dialogService.Dialog.ShowDialog();
        }
        #endregion
    }
}
