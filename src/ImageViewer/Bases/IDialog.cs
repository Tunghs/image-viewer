namespace ImageViewer.Bases
{
    public interface IDialog
    {
        object DataContext { get; set; }

        void Show();

        bool? ShowDialog();

        void Close();
    }
}
