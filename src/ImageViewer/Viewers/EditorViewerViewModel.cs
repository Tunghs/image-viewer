using CommunityToolkit.Mvvm.ComponentModel;

using ImageViewer.Bases;
using ImageViewer.Viewers.EditorViewers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Viewers
{
    public partial class EditorViewerViewModel : PopupDialogViewModelBase
    {
        #region Fields
        [ObservableProperty]
        private ImageResizeViewModel _resizeVm;
        #endregion

        public EditorViewerViewModel()
        {
            _resizeVm = new ImageResizeViewModel();
        }
    }
}
