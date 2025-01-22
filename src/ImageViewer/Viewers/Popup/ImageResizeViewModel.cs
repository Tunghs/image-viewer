using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using ImageViewer.Bases;
using ImageViewer.Data;
using ImageViewer.Enums;
using ImageViewer.Services;

using Microsoft.Win32;

using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace ImageViewer.Viewers.Popup
{
    public partial class ImageResizeViewModel : PopupDialogViewModelBase
    {
        #region Fields

        private List<string> _supportedExtentions = new List<string>() { ".jpg", ".png", ".bmp", ".jpeg" };
        private IImageProcessingService _imageProcessingService;
        private ImageResizeMode _resizeMode = ImageResizeMode.Pixel;

        #endregion Fields

        #region UI Properties

        [ObservableProperty]
        private Visibility _percentageSettingVisibility = Visibility.Collapsed;

        [ObservableProperty]
        private ObservableCollection<string> _images = new ObservableCollection<string>();

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

        [ObservableProperty]
        private Visibility _progressGridVisibility = Visibility.Collapsed;

        [ObservableProperty]
        private int _progressValue = 0;

        [ObservableProperty]
        private int _progressMaximum = 0;

        #endregion UI Properties

        public ImageResizeViewModel(IImageProcessingService imageProcessingService)
        {
            _imageProcessingService = imageProcessingService;
        }

        #region Command

        [RelayCommand]
        private void OnAddImageBtnClick()
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

        [RelayCommand]
        private void OnAddFolderBtnClick()
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

        [RelayCommand]
        private void OnDeleteElementBtnClick()
        {

        }

        [RelayCommand]
        private void OnRadioButtonClick(string @param)
        {
            if (param == "Pixel")
            {
                _resizeMode = ImageResizeMode.Pixel;
                IsNewSizeEnabled = true;
                PercentageSettingVisibility = Visibility.Collapsed;

                UpdateResizePixel();
            }
            else if (param == "Percentage")
            {
                _resizeMode = ImageResizeMode.Percentage;
                IsNewSizeEnabled = true;
                PercentageSettingVisibility = Visibility.Visible;

                if (SelectedImage != null)
                {
                    UpdateResizePercentage(ResizePercentage);
                }
            }
            else
            {
                _resizeMode = ImageResizeMode.Pixed;
                IsNewSizeEnabled = false;
                string[] spParams = @param.Split(' ');
                DisplayResizeWidth = spParams[0];
                DisplayResizeHeight = spParams[2];
            }
        }

        [RelayCommand]
        private void OnResizeApplyBtnClick()
        {
            if (_resizeMode == ImageResizeMode.Pixel)
            {
                UpdateResizePixel();
            }
            else if (_resizeMode == ImageResizeMode.Percentage)
            {
                UpdateResizePercentage(ResizePercentage);
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
            for (int index = 0; index < dropItems.Length; index++)
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

        [RelayCommand]
        private void OnRunResizeBtnClick()
        {
            var folderDialog = new OpenFolderDialog
            {
                Title = "Select Save Folder",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                Multiselect = false
            };

            ProgressGridVisibility = Visibility.Visible;
            if (folderDialog.ShowDialog() == true)
            {
                var folderPath = folderDialog.FolderName;

                if (_resizeMode == ImageResizeMode.Percentage)
                {
                    RunResizeByPercentage(ResizePercentage / 100.0, folderPath);
                }
                else
                {
                    RunResize(ResizeWidth, ResizeHeight, folderPath);
                }
            }

            MessageBox.Show("Complete");
            ProgressGridVisibility = Visibility.Collapsed;
        }

        #endregion Command

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

            if (_resizeMode == ImageResizeMode.Percentage)
            {
                UpdateResizePercentage(ResizePercentage);
            }
        }

        private void UpdateResizePixel()
        {
            DisplayResizeWidth = ResizeWidth.ToString();
            DisplayResizeHeight = ResizeHeight.ToString();
        }

        private void UpdateResizePercentage(int value)
        {
            if (SelectedImage == null)
            {
                return;
            }

            double width = Math.Round(((double)(SelectedImageInfo.Width * (double)(value / 100.0))));
            double height = Math.Round((double)(SelectedImageInfo.Height * (double)(value / 100.0)));

            DisplayResizeWidth = width.ToString();
            DisplayResizeHeight = height.ToString();
        }

        private async void RunResizeByPercentage(double ratio, string saveDirPath)
        {
            if (ratio <= 0)
            {
                return;
            }

            await Task.Factory.StartNew(() =>
            {
                for (int index = 0; index < Images.Count; index++)
                {
                    string saveFilePath = saveDirPath + @"\" + Path.GetFileName(Images[index]);
                    _imageProcessingService.Resize(Images[index], saveFilePath, ratio);
                }
            });
        }

        private async void RunResize(int width, int height, string saveDirPath)
        {
            if (width <= 0 || height <= 0)
            {
                return;
            }

            ProgressMaximum = Images.Count;
            await Task.Run(() =>
            {
                for (int index = 0; index < Images.Count; index++)
                {
                    ProgressValue = index;
                    string saveFilePath = saveDirPath + @"\" + Path.GetFileName(Images[index]);
                    _imageProcessingService.Resize(Images[index], saveFilePath, width, height);
                }
            });
        }

        #endregion Methods
    }
}