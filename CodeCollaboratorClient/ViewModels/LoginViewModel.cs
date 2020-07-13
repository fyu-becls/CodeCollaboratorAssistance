using CodeCollaboratorClient.Messages;
using CollabCommandAPI;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CodeCollaboratorClient;
using CodeCollaboratorClient.Authentication;

namespace CodeCollaboratorClient.ViewModels
{
    public class LoginViewModel : ObservableObject
    {
        private bool _isLogin;
        private readonly CollabConnectionSettings _collabConnectionSettings;
        private readonly ICollabServerConnection _serverConnection;
        private readonly IAuthenticationService _authenticationService;
        public LoginViewModel(CollabConnectionSettings settings, ICollabServerConnection serverConnection, IAuthenticationService authenticationService)
        {
            _collabConnectionSettings = settings;
            _serverConnection = serverConnection;
            _authenticationService = authenticationService;
        }

        public bool IsLogin => Thread.CurrentPrincipal.Identity.IsAuthenticated;

        public bool IsLogout
        {
            get => !IsLogin;
        }

        public string UserName
        {
            get
            {
                //Get the current principal object
                CustomPrincipal customPrincipal = Thread.CurrentPrincipal as CustomPrincipal;
                if (customPrincipal == null)
                    throw new ArgumentException("The application's default thread principal must be set to a CustomPrincipal object on startup.");
                return customPrincipal.Identity?.Name ?? "Anonymous";
            }
        }
        public string UserPassword
        {
            get => _collabConnectionSettings.UserPassword;
            set => Set(ref _collabConnectionSettings.UserPassword, value);
        }

        public ICommand LoginCommand
        {
            get => new RelayCommand(() =>
            {
                var msg = new OpenLoginDialogMessage(_collabConnectionSettings);
                Messenger.Default.Send<OpenLoginDialogMessage>(msg);
            });
        }

        public ICommand LogoutCommand
        {
            get => new RelayCommand(() =>
            {
                //Get the current principal object
                CustomPrincipal customPrincipal = Thread.CurrentPrincipal as CustomPrincipal;
                if (customPrincipal == null)
                    throw new ArgumentException("The application's default thread principal must be set to a CustomPrincipal object on startup.");

                //Authenticate the user
                customPrincipal.Identity = new AnonymousIdentity();

                _collabConnectionSettings.UserName = null;
                _collabConnectionSettings.UserPassword = null;
                RaisePropertyChanged(nameof(IsLogin));
                RaisePropertyChanged(nameof(IsLogout));
                RaisePropertyChanged(nameof(UserName));
            });
        }

        public async Task<bool> LoginWithParameter(CollabConnectionSettings settings)
        {
            CurrentMainWindow.Instance.IsBusy = true;
            await Task.Delay(200);
            if (string.IsNullOrWhiteSpace(settings.UserName) || string.IsNullOrWhiteSpace(settings.UserPassword))
            {
                Messenger.Default.Send<NotificationMessage>(new NotificationMessage("Username/password is required."));
            }
            else
            {
                try
                {
                    //Validate credentials through the authentication service
                    User user = await _authenticationService.AuthenticateUser(settings.UserName, settings.UserPassword);

                    //Get the current principal object
                    CustomPrincipal customPrincipal = Thread.CurrentPrincipal as CustomPrincipal;
                    if (customPrincipal == null)
                        throw new ArgumentException("The application's default thread principal must be set to a CustomPrincipal object on startup.");

                    //Authenticate the user
                    customPrincipal.Identity = new CustomIdentity(user.Username, user.Roles);
                    RaisePropertyChanged(nameof(UserName));
                    RaisePropertyChanged(nameof(IsLogin));
                    RaisePropertyChanged(nameof(IsLogout));

                    //Update UI
                }
                catch (UnauthorizedAccessException)
                {
                    CurrentMainWindow.Instance.IsBusy = false;
                    Messenger.Default.Send<NotificationMessage>(new NotificationMessage("Login failed! Please provide some valid credentials."));

                    return await Task.FromResult(false);
                }
                catch (Exception ex)
                {
                    CurrentMainWindow.Instance.IsBusy = false;
                    Messenger.Default.Send<NotificationMessage>(new NotificationMessage($"ERROR: {ex.Message}"));
                    return await Task.FromResult(false);
                }
            }

            CurrentMainWindow.Instance.IsBusy = false;
            return await Task.FromResult(true);
        }
    }
}
