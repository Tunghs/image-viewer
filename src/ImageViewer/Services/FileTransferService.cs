using ImageViewer.Data;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Services
{
    public interface IFileTransferService
    {
        void Cancel();

        void Copy(string srcFilePath, string dstFilePath);

        void Move(string srcFilePath, string dstFilePath);

    }

    public class FileTransferService : IFileTransferService
    {
        #region Fields
        private FixedSizeStack<string> _cancleStack = new FixedSizeStack<string>(5);
        #endregion

        public void Cancel()
        {
            string cancelFilePath = _cancleStack.Pop();
            if (cancelFilePath == null)
            {
                return;
            }

            if (!File.Exists(cancelFilePath))
            {
                return;
            }
        }

        public void Copy(string srcFilePath, string dstFilePath)
        {
            throw new NotImplementedException();
        }

        public void Move(string srcFilePath, string dstFilePath)
        {
            throw new NotImplementedException();
        }
    }
}
