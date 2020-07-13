using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Authoring.Infrastructure.ServiceLocator;
using CodeCollaboratorClient.ViewModels;

namespace CodeCollaboratorClient.Pages
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
            DataContext = ServiceLocatorManager.Instance.GlobalServiceLocator.GetService<HomeViewModel>();
        }

        private void Hyperlink_OnClick(object sender, RoutedEventArgs e)
        {
            CurrentMainWindow.Instance.NavigateTo(new Uri("Pages/Review.xaml", UriKind.Relative));
        }
    }
}
