using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using ImageViewer.Bases;

using Microsoft.Win32;

using System.Windows.Input;

namespace ImageViewer.Viewers
{
    public partial class SettingViewerViewModel : PopupDialogViewModelBase
    {
        #region Fields
        private OpenFolderDialog _openFolderDialog = new OpenFolderDialog() { Title = "Select Save Folder" };
        #endregion

        #region UI Variable
        [ObservableProperty]
        private string _saveDirPath;

        [ObservableProperty]
        private string _key;

        [ObservableProperty]
        private Dictionary<string, string> _settings = new Dictionary<string, string>();

        public bool IsFileCopy;
        private bool _isFileCopy
        {
            get => IsFileCopy;
            set 
            { 
                SetProperty(ref IsFileCopy, value);
                FileCopyModeChanged?.Invoke(value);
            }
        }
        #endregion

        #region Event
        public Action<bool> FileCopyModeChanged { get; set; }
        #endregion

        [RelayCommand]
        private void OnButtonClick(string @param)
        {
            switch (@param)
            {
                case "AddSetting":
                    AddSetting();
                    break;
                case "OpenDirDialog":
                    OpenDirDialog();
                    break;
                default:
                    break;
            }
        }

        [RelayCommand]
        private void OnSettingDeleteBtnClick(string @param)
        {
            Settings.Remove(@param);

            UpdateSetting();
        }

        [RelayCommand]
        private void OnKeyTbxKeyDown(KeyEventArgs e)
        {
            Key = string.Empty;
            e.Handled = true;

            Key = e.Key.ToString();
        }

        private void AddSetting()
        {
            if (string.IsNullOrEmpty(SaveDirPath) || string.IsNullOrEmpty(Key))
            {
                return;
            }

            if (Settings.ContainsKey(Key))
            {
                System.Windows.MessageBox.Show("중복된 단축키 존재");
                return;
            }

            Settings.Add(Key, SaveDirPath);
            SaveDirPath = string.Empty;
            Key = string.Empty;

            UpdateSetting();
        }

        private void OpenDirDialog()
        {
            if (_openFolderDialog.ShowDialog() == true)
            {
                SaveDirPath = _openFolderDialog.FolderName;
            }
        }

        private void UpdateSetting()
        {
            var tempDic = new Dictionary<string, string>(Settings);
            Settings = null;
            Settings = tempDic;
        }
    }
}
