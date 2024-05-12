using CommunityToolkit.Mvvm.ComponentModel;

using ImageViewer.Bases;

namespace ImageViewer
{
    public partial class PopupViewModel : ViewModelBase
    {
        #region UI Properties
        [ObservableProperty]
        private ViewModelBase _popupVM;
        #endregion
    }
}
