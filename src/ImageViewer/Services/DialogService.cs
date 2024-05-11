using CommunityToolkit.Mvvm.DependencyInjection;

using ImageViewer.Bases;

namespace ImageViewer.Services
{
    public class DialogService : IDialogService
    {
        private IDialog _popWindow;

        public DialogService()
        {
            _popWindow = Ioc.Default.GetService(typeof(PopupWindow)) as IDialog;
        }

        public IDialog Dialog => _popWindow;

        public void SetVM(ViewModelBase vm)
        {
            if (_popWindow.DataContext is PopupViewModel viewModel)
            {
                viewModel.PopupVM = vm;
            }
        }
    }
}
