using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using ImageViewer.Bases;
using ImageViewer.Services;
using ImageViewer.Viewers;

using System.IO;
using System.Windows.Input;

using Wpf.Ui;
using Wpf.Ui.Controls;

namespace ImageViewer
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        #region Fields

        private readonly IFileControlService _controller;
        private readonly ISnackbarService _snackbarService;
        private readonly IDialogService _dialogService;
        private SettingViewerViewModel _settingViewerVm;
        private EditorViewerViewModel _editorViewerVm;

        #endregion Fields

        public MainWindowViewModel(
            IDialogService dialogService,
            ISnackbarService snackbarService,
            IFileControlService fileControlService)
        {
            _dialogService = dialogService;
            _snackbarService = snackbarService;
            _controller = fileControlService;
            _controller.Changed += OnControlChanged;
            _settingViewerVm = new SettingViewerViewModel();
            _editorViewerVm = new EditorViewerViewModel();
        }

        #region UI Properties

        [ObservableProperty]
        private string _title;

        #endregion UI Properties

        #region Command

        [RelayCommand]
        private void OnButtonClick(string @param)
        {
            switch (@param)
            {
                case "OpenSetting":
                    OpenSettingWindow();
                    break;
                case "OpenEditor":
                    OpenEditorWindow();
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

            if (_settingViewerVm.Settings.ContainsKey(e.Key.ToString()))
            {
                _controller.Move(_settingViewerVm.Settings[e.Key.ToString()]);
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

        private void OpenSettingWindow()
        {
            _dialogService.Show(_settingViewerVm, "Settings", 500, 650, typeof(PopupWindow));
        }

        private void OpenEditorWindow()
        {
            _dialogService.Show(_editorViewerVm, "Editor", 960, 720, typeof(PopupWindow));
        }

        private void OnControlChanged(object? sender, FileChangedEventArgs e)
        {
            Title = $"[{e.Index + 1}/{e.TotalCount}] {Path.GetFileName(e.FileName)}";

            if (e.Index == 0)
            {
                _snackbarService.Show("Information", "This is the first image.", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Info28), TimeSpan.FromSeconds(2));
            }
            else if (e.Index == e.TotalCount - 1)
            {
                _snackbarService.Show("Information", "This is the last image.", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Info28), TimeSpan.FromSeconds(2));
            }
        }

        #endregion Methods
    }
}