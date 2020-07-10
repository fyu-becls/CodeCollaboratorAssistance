using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Navigation;

namespace CodeCollaboratorClient
{
    public interface IMainWindow
    {
        bool IsBusy { get; set; }
        NavigationService NavService { get; }
        void NavigateTo(Uri page, object parameter = null);
    }
}
