using ImageViewer.Data;

using System.IO;

namespace ImageViewer.Services
{
    public interface IFileControlService
    {
        event EventHandler<FileChangedEventArgs> Changed;
        void LoadImages(string path);
        void Cancel();
        bool Next();
        bool Previous();
        void Move(string dstDirPath);
    }

    public class FileChangedEventArgs : EventArgs
    {
        public readonly int TotalCount;
        public readonly int Index;
        public readonly string FileName;
        public FileChangedEventArgs(int totalCount, int index, string fileName)
        {
            TotalCount = totalCount;
            Index = index;
            FileName = fileName;
        }
    }

    public class FileControlService : IFileControlService
    {
        #region Fields
        private List<string> _supportedExtentions = new List<string>() { ".jpg", ".png", ".bmp", ".jpeg" };
        private FixedSizeStack<string> _cancleStack = new FixedSizeStack<string>(5);
        private string _currentDirectory;
        private List<string> _imageList = [];
        private int _index;
        private int _totalCount;
        #endregion

        #region Event
        public event EventHandler<FileChangedEventArgs> Changed;
        #endregion

        #region Public Methods
        public void LoadImages(string path)
        {
            string currentImage = null;
            string directoryPath = path;
            if (!IsDirectory(path))
            {
                currentImage = path;
                directoryPath = Path.GetDirectoryName(path);
            }

            _imageList = Directory.EnumerateFiles(directoryPath, "*.*", SearchOption.TopDirectoryOnly)
                .Where(s => _supportedExtentions.Any(x => s.ToLower().EndsWith(x))).ToList();

            _totalCount = _imageList.Count;
            _index = 0;
            if (currentImage != null)
            {
                _index = _imageList.IndexOf(currentImage);
            }
            OnFileChanged(new FileChangedEventArgs(_totalCount, _index, _imageList[_index]));
        }

        public void Cancel()
        {
            string cancelFilePath = _cancleStack.Pop();
            if (cancelFilePath == null)
            {
                return;
            }

            if (File.Exists(cancelFilePath))
            {
                FileInfo info = new FileInfo(cancelFilePath);
                info.MoveTo($@"{_currentDirectory}\{info.Name}");

                _imageList.Add(cancelFilePath);
                _imageList.Sort();

                _index = _imageList.IndexOf(_imageList[_index]);

                OnFileChanged(new FileChangedEventArgs(_totalCount++, _index, _imageList[_index]));
            }
        }

        public bool Next()
        {
            _index++;
            if (_imageList.Count < _index)
            {
                _index--;
                return false;
            }

            OnFileChanged(new FileChangedEventArgs(_totalCount, _index, _imageList[_index]));
            return true;
        }

        public bool Previous()
        {
            _index--;
            if (_index < 0)
            {
                _index++;
                return false;
            }

            OnFileChanged(new FileChangedEventArgs(_totalCount, _index, _imageList[_index]));
            return true;
        }

        public void Move(string dstDirPath)
        {
            if (File.Exists(_imageList[_index]))
            {
                FileInfo info = new FileInfo(_imageList[_index]);
                info.MoveTo($@"{dstDirPath}\{info.Name}");

                _cancleStack.Push(_imageList[_index]);
                _imageList.RemoveAt(_index);

                OnFileChanged(new FileChangedEventArgs(_totalCount--, _index, _imageList[_index]));
            }
        }
        #endregion

        protected virtual void OnFileChanged(FileChangedEventArgs e)
        {
            Changed?.Invoke(this, e);
        }

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
    }
}
