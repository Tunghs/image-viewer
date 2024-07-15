using ImageViewer.Viewers;

using System.Windows;
using System.Windows.Controls;

namespace ImageViewer.DataTemplateSelectors
{
    public class PopupTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SettingsViewerTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is SettingViewerViewModel)
            {
                return SettingsViewerTemplate;
            }
            return base.SelectTemplate(item, container);
        }
    }
}
