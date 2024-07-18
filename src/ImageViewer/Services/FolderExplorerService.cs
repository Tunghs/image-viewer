using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Services
{
    public interface IFolderExplorerService
    {
        void LoadFolder(string path);

        void Cancel();

        void Next();
    }

    public class FolderExplorerService : IFolderExplorerService
    {

    }
}
