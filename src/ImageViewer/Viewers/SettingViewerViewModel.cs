using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using ImageViewer.Bases;

using Microsoft.Win32;

using System.Windows.Input;

namespace ImageViewer.Viewers
{
    public partial class SettingViewerViewModel : PopupDialogViewModelBase
    {
        #region UI Variable
        [ObservableProperty]
        private string _saveDirPath;

        [ObservableProperty]
        private string _key;

        [ObservableProperty]
        private Dictionary<string, string> _settings = new Dictionary<string, string>() { {"ddddd", "eeeee"}};
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

            if (Settings.Any(pair => pair.Value == Key))
            {
                System.Windows.MessageBox.Show("중복된 단축키 존재");
                return;
            }

            if (Settings.ContainsKey(SaveDirPath))
            {
                System.Windows.MessageBox.Show("중복된 저장 폴더 존재");
                return;
            }

            Settings.Add(SaveDirPath, Key);
            SaveDirPath = string.Empty;
            Key = string.Empty;

            UpdateSetting();
        }

        private void OpenDirDialog()
        {
            var folderDialog = new OpenFolderDialog
            {
                Title = "Select Folder",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };

            if (folderDialog.ShowDialog() == true)
            {
                SaveDirPath = folderDialog.FolderName;
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
