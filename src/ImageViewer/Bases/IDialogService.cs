using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Bases
{
    public interface IDialogService
    {
        IDialog Dialog { get; }

        void SetVM(ViewModelBase vm);
    }
}
