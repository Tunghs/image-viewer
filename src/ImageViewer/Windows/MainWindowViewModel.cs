using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using ImageViewer.Bases;
using ImageViewer.Services;
using ImageViewer.Viewers.Popup;

using System.IO;
using System.Windows.Input;

using Wpf.Ui;

namespace ImageViewer.Windows
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        #region Fields

        private readonly IFileControlService _controller;
        private readonly ISnackbarService _snackbarService;
        private readonly IDialogService _dialogService;
        private ShortcutKeySettingViewModel _shortcutKeySettingVm;
        private ImageResizeViewModel _resizeViewModel;

        #endregion Fields

        #region UI Properties

        [ObservableProperty]
        private string _title = "Image Viewer";

        #endregion UI Properties

        public MainWindowViewModel(
            IDialogService dialogService,
            ISnackbarService snackbarService,
            IFileControlService fileControlService,
            IImageProcessingService imageProcessingService)
        {
            _dialogService = dialogService;
            _snackbarService = snackbarService;
            _controller = fileControlService;
            _controller.Changed += OnControlChanged;
            _shortcutKeySettingVm = new ShortcutKeySettingViewModel();
            _resizeViewModel = new ImageResizeViewModel(imageProcessingService);
        }

        #region Command

        [RelayCommand]
        private void OnButtonClick(string @param)
        {
            switch (@param)
            {
                case "Hotkey":
                    _dialogService.Show(_shortcutKeySettingVm, "Shortcut Key Setting", 500, 650, typeof(PopupWindow));
                    break;

                case "Resize":
                    _dialogService.Show(_resizeViewModel, "Image Resize", 960, 520, typeof(PopupWindow));
                    break;

                default:
                    break;
            }
        }

        [RelayCommand]
        private void OnKeyDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    _controller.Previous();
                    break;

                case Key.Right:
                    _controller.Next();
                    break;

                case Key.Back:
                    _controller.Cancel();
                    break;
            }

            if (_shortcutKeySettingVm.Settings.ContainsKey(e.Key.ToString()))
            {
                _controller.Move(_shortcutKeySettingVm.Settings[e.Key.ToString()]);
            }
        }

        [RelayCommand]
        private void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                _controller.Previous();
            }
            else
            {
                _controller.Next();
            }
        }

        #endregion Command

        #region Methods

        private void OnControlChanged(object? sender, FileChangedEventArgs e)
        {
            Title = $"[{e.Index + 1}/{e.TotalCount}] {Path.GetFileName(e.FileName)}";
        }

        #endregion Methods
    }
}