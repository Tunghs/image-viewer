using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;

using ImageViewer.Bases;
using ImageViewer.PopupWindows;
using ImageViewer.Services;

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

        #endregion Fields

        public MainWindowViewModel()
        {
            _dialogService = Ioc.Default.GetService<IDialogService>();
            _snackbarService = Ioc.Default.GetService<ISnackbarService>();
            _controller = Ioc.Default.GetService<IFileControlService>();
            _controller.Changed += OnControlChanged;
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
            switch (e.Key)
            {
                case Key.Left:
                    _controller.Previous();
                    break;

                case Key.Right:
                    _controller.Next();
                    break;

                default:
                    break;
            }
        }

        #endregion Command

        #region Methods

        private void OpenSettingWindow()
        {
            _dialogService.Show(new PopupViewModel(), "HI", 500, 650, typeof(PopupWindow));
        }

        private void OnControlChanged(object? sender, FileChangedEventArgs e)
        {
            Title = $"{Path.GetFileName(e.FileName)} ( {e.Index + 1} / {e.TotalCount} )";

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