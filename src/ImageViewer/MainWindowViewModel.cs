using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;

using ImageViewer.Bases;
using ImageViewer.Data;
using ImageViewer.PopupWindows;
using ImageViewer.Services;

using System.IO;
using System.Windows.Input;

namespace ImageViewer
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        #region Fields
        private IFileController _controller;
        private readonly IDialogService dialogService;
        #endregion

        public MainWindowViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;
            _controller = Ioc.Default.GetService<IFileController>();
            _controller.Changed += OnControlChanged;
        }

        #region UI Properties
        [ObservableProperty]
        private string _title;
        #endregion

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

        private void OnControlChanged(object? sender, FileChangedEventArgs e)
        {
            Title = $"{Path.GetFileName(e.FileName)} ( {e.Index} / {e.TotalCount} )";
        }
        #endregion
    }
}
