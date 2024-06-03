using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace ImageViewer.Bases
{
    public abstract class ViewModelBase : ObservableValidator
    {
        public ViewModelBase() { }

        public virtual void Cleanup()
        {
            WeakReferenceMessenger.Default.Cleanup();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Cleanup();
            }
        }
    }

    public abstract partial class PopupDialogViewModelBase : ViewModelBase
    {
        [ObservableProperty]
        private ObservableObject? popupVM;

        [RelayCommand]
        private void Closing()
        {
            PopupVM = null;
        }
    }
}
