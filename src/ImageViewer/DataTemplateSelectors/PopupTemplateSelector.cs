using ImageViewer.Viewers.Popup;

using System.Windows;
using System.Windows.Controls;

namespace ImageViewer.DataTemplateSelectors
{
    public class PopupTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ShortcutKeySettingTemplate { get; set; }
        public DataTemplate ImageResizeTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is ShortcutKeySettingViewModel)
            {
                return ShortcutKeySettingTemplate;
            }
            else if (item is ImageResizeViewModel)
            {
                return ImageResizeTemplate;
            }
            return base.SelectTemplate(item, container);
        }
    }
}
