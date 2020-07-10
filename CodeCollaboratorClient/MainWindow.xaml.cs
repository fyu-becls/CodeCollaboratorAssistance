using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using Authoring.Infrastructure.ServiceLocator;
using CodeCollaboratorClient.Messages;
using CodeCollaboratorClient.ViewModels;
using CollabCommandAPI;
using CollabCommandAPI.Commands;
using CollabCommandAPI.DataTypes;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls.Dialogs;

namespace CodeCollaboratorClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged, IMainWindow
    {
        private bool _isBusy;

        public MainWindow()
        {
            InitializeComponent();
            CurrentMainWindow.Instance = this;

            Messenger.Default.Register<OpenLoginDialogMessage>(this, OpenLoginDialogMethod);

            Messenger.Default.Register<NotificationMessage>(this, ShowNotificationMethod);
            LoginViewModel = ServiceLocatorManager.Instance.GlobalServiceLocator.GetService<LoginViewModel>();
            DataContext = this;
            Loaded += MainWindow_Loaded;

            var connection = ServiceLocatorManager.Instance.GlobalServiceLocator.GetService<ICollabServerConnection>();
            connection.NewOutputEvent += (sender, s) => { Output.Add(s); };

            NavigationFrame.Navigated += (sender, args) =>
            {
                if (args.Uri == new Uri("Pages/HomePage.xaml", UriKind.Relative))
                {
                    Hamburger.SelectedIndex = 0;
                }
                else if (args.Uri == new Uri("Pages/Review.xaml", UriKind.Relative))
                {
                    Hamburger.SelectedIndex = 1;
                }
            };
        }

        public ObservableCollection<string> Output { get; set; } = new ObservableCollection<string>();

        public LoginViewModel LoginViewModel { get; set; }

        public ICommand ShowHamburgerHomeCommand =>
            new RelayCommand(() => { NavigationFrame.Navigate(new Uri("Pages/HomePage.xaml", UriKind.Relative)); });

        public ICommand ShowHamburgerReviewCommand =>
            new RelayCommand(() => { NavigationFrame.Navigate(new Uri("Pages/Review.xaml", UriKind.Relative)); });

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (Set(ref _isBusy, value))
                {
                    if (_isBusy)
                        IsEnabled = false;
                    else
                        IsEnabled = true;
                }
            }
        }

        public NavigationService NavService
        {
            get => NavigationFrame.NavigationService;
        }

        public void NavigateTo(Uri page, object parameter)
        {
            var result = NavigationFrame.Navigate(page, parameter);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Hamburger.SelectedIndex = 0;
        }

        private void RaisePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private bool Set<T>(ref T field, T value, [CallerMemberName] string name = null)
        {
            if (Equals(field, value))
                return false;
            field = value;

            RaisePropertyChanged(name);
            return true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var reviewCommands = ServiceLocatorManager.Instance.GlobalServiceLocator.GetService<ReviewCommands>();
            var command = reviewCommands.Create("Test", null, "2020-06-20", null, ReviewRestrictAccess.Participants);
            var executor = ServiceLocatorManager.Instance.GlobalServiceLocator.GetService<ICollabCommandExecutor>();
            var result = executor.ExecuteCommand(command, 5000);
        }

        private void OpenSiteClick(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo(UserSettings.Default.ServerUrl) {UseShellExecute = true});
        }

        private void OpenSettingsDialog(object sender, RoutedEventArgs e)
        {
        }

        private void RibbonGalleryItem_Selected(object sender, RoutedEventArgs e)
        {
        }

        private async void OpenLoginDialogMethod(OpenLoginDialogMessage msg)
        {
            var settings = new LoginDialogSettings
            {
                InitialUsername = msg.Settings.UserName,
                NegativeButtonVisibility = Visibility.Visible,
                EnablePasswordPreview = true
            };

            var data = await this.ShowLoginAsync("Login",
                "Please use your Code Collaborator account and password to login.", settings);

            if (data != null)
            {
                msg.Settings.UserName = data.Username;
                msg.Settings.UserPassword = data.Password;

                await LoginViewModel.LoginWithParameter(msg.Settings);
            }
        }

        private async void ShowNotificationMethod(NotificationMessage msg)
        {
            await this.ShowMessageAsync("Notification", msg.Notification);
        }
    }
}