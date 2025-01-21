﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using ImageViewer.Bases;

using System.Windows;
using System.IO;
using System.Collections.ObjectModel;
using ImageViewer.Services;
using ImageViewer.Data;
using Microsoft.Win32;

namespace ImageViewer.Viewers.Popup
{
    public partial class ImageResizeViewModel : PopupDialogViewModelBase
    {
        #region Fields
        private List<string> _supportedExtentions = new List<string>() { ".jpg", ".png", ".bmp", ".jpeg" };
        private IImageProcessingService _imageProcessingService;
        #endregion

        #region UI Properties
        [ObservableProperty]
        private Visibility _percentageSettingVisibility = Visibility.Collapsed;

        [ObservableProperty]
        ObservableCollection<string> _images = new ObservableCollection<string>();

        [ObservableProperty]
        private ImageInfo _selectedImageInfo;

        private string _selectedImage;
        public string SelectedImage
        {
            get => _selectedImage;
            set 
            { 
                SetProperty(ref _selectedImage, value);
                UpdateImageInfo(value);
            }
        }

        [ObservableProperty]
        private bool _isNewSizeEnabled = true;

        [ObservableProperty]
        private string _displayResizeWidth;

        [ObservableProperty]
        private string _displayResizeHeight;

        [ObservableProperty]
        private int _resizeWidth = 256;

        [ObservableProperty]
        private int _resizeHeight = 256;

        [ObservableProperty]
        private int _resizePercentage = 100;
        #endregion

        public ImageResizeViewModel(IImageProcessingService imageProcessingService)
        {
            _imageProcessingService = imageProcessingService;
        }

        #region Command
        [RelayCommand]
        private void OnButtonClick(string @param)
        {
            switch (@param)
            {
                case "AddImage":
                    OpenFileDialog();
                    break;
                case "AddFolder":
                    OpenFolderDialog();
                    break;
                case "Run":
                    Resize(ResizeWidth, ResizeHeight);
                    break;
                default:
                    break;
            }
        }

        [RelayCommand]
        private void OnRadioButtonClick(string @param)
        {
            if (param == "Pixel")
            {
                IsNewSizeEnabled = true;
                PercentageSettingVisibility = Visibility.Collapsed;

                if (SelectedImage != null)
                {
                    DisplayResizeWidth = ResizeWidth.ToString();
                    DisplayResizeHeight = ResizeHeight.ToString();
                }
            }
            else if (param == "Percentage")
            {
                IsNewSizeEnabled = true;
                PercentageSettingVisibility = Visibility.Visible;

                if (SelectedImage != null)
                {
                    UpdateResizePercentage();
                }
            }
            else
            {
                IsNewSizeEnabled = false;
                string[] spParams = @param.Split(' ');
                DisplayResizeWidth = spParams[0];
                DisplayResizeHeight = spParams[2];
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
                    var images = Directory.EnumerateFiles(dropItems[index], "*.*", SearchOption.TopDirectoryOnly)
                        .Where(s => _supportedExtentions.Any(x => s.ToLower().EndsWith(x))).ToList();

                    for (int imgIdx = 0; imgIdx < images.Count; imgIdx++)
                    {
                        Images.Add(images[imgIdx]);
                    }
                }
                else
                {
                    if (_supportedExtentions.Any(x => dropItems[index].ToLower().EndsWith(x)))
                        Images.Add(dropItems[index]);
                }
            }
        }

        [RelayCommand]
        public void OnSelectionChanged(string selectedItem)
        {
            MessageBox.Show(selectedItem);
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

        private void UpdateImageInfo(string filePath)
        {
            SelectedImageInfo = new ImageInfo(filePath);
            UpdateResizePercentage();
        }

        private void UpdateResizePercentage()
        {
            DisplayResizeWidth = (SelectedImageInfo.Width * (ResizePercentage / 100)).ToString();
            DisplayResizeHeight = (SelectedImageInfo.Height * (ResizePercentage / 100)).ToString();
        }

        private void OpenFileDialog()
        {
            var fileDialog = new OpenFileDialog
            {
                Title = "Select Image",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                Filter = "image files (*.png, *.jpg, *jpeg, *.bmp)|*.png;*.jpg;*jpeg;*.bmp"
            };

            if (fileDialog.ShowDialog() == true)
            {
                var files = fileDialog.FileNames;
                for (int imgIdx = 0; imgIdx < files.Length; imgIdx++)
                {
                    Images.Add(files[imgIdx]);
                }
            }
        }

        private void OpenFolderDialog()
        {
            var folderDialog = new OpenFolderDialog
            {
                Title = "Select Folder",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)
            };

            if (folderDialog.ShowDialog() == true)
            {
                var folders = folderDialog.FolderNames;
                for (int index = 0; index < folders.Length; index++)
                {
                    if (IsDirectory(folders[index]))
                    {
                        var images = Directory.EnumerateFiles(folders[index], "*.*", SearchOption.TopDirectoryOnly)
                            .Where(s => _supportedExtentions.Any(x => s.ToLower().EndsWith(x))).ToList();

                        for (int imgIdx = 0; imgIdx < images.Count; imgIdx++)
                        {
                            Images.Add(images[imgIdx]);
                        }
                    }
                }
            }
        }

        private void Resize(int width, int height)
        {
            string time = DateTime.Now.ToString("yyyyMMddHHmmss");
            foreach (var image in Images)
            {
                string saveDir = Path.GetDirectoryName(image) + $@"\{time}_Resize";
                _imageProcessingService.Resize(image, saveDir, width, height);
            }
        }
        #endregion
    }
}