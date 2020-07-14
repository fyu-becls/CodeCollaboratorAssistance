using System.Windows;
using System.Windows.Controls;
using CollabAPI;

namespace CodeCollaboratorClient.Pages
{
    public class CustomFieldTemplateSelector : DataTemplateSelector
    {
        public CustomFieldTemplateSelector()
        {

        }

        public DataTemplate SelectionTemplate { get; set; }

        public DataTemplate StringTemplate { get; set; }

        public DataTemplate StringBigTemplate { get; set; }


        public DataTemplate UnSupportedTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var fieldSettings = item as SystemAdmin.CustomFieldSettings;
            if (fieldSettings == null)
                return null;

            switch (fieldSettings.customFieldType)
            {
                case SystemAdmin.CustomFieldType.SELECTION:
                    return SelectionTemplate;
                case SystemAdmin.CustomFieldType.STRING:
                    return StringTemplate;
                case SystemAdmin.CustomFieldType.STRINGBIG:
                    return StringBigTemplate;
                default:
                    return UnSupportedTemplate;
            }
        }
    }
}