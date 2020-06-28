using CodeCollaboratorClient.Messages;
using CollabCommandAPI;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CodeCollaboratorClient.ViewModels
{
    public class LoginViewModel : ObservableObject
    {
        private bool _isLogin;
        private readonly CollabConnectionSettings _collabConnectionSettings;
        private readonly ICollabServerConnection _serverConnection;
        public LoginViewModel(CollabConnectionSettings settings, ICollabServerConnection serverConnection)
        {
            _collabConnectionSettings = settings;
            _serverConnection = serverConnection;
        }

        public bool IsLogin
        {
            get => _isLogin;
            set
            {
                if (Set(ref _isLogin, value))
                {
                    RaisePropertyChanged(nameof(IsLogout));
                }
            }
        }

        public bool IsLogout
        {
            get => !IsLogin;
        }

        public string UserName
        {
            get => _collabConnectionSettings.UserName;
            set
            {
                if (Set(ref _collabConnectionSettings.UserName, value))
                {
                    UserSettings.Default.UserName = _collabConnectionSettings.UserName;
                }
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
                _collabConnectionSettings.UserName = null;
                _collabConnectionSettings.UserPassword = null;
                IsLogin = false;
                RaisePropertyChanged(nameof(UserName));
            });
        }

        public async Task<bool> LoginWithParameter(CollabConnectionSettings settings)
        {
            CurrentMainWindow.Instance.IsBusy = true;
            if (string.IsNullOrWhiteSpace(settings.UserName) || string.IsNullOrWhiteSpace(settings.UserPassword))
            {
                Messenger.Default.Send<NotificationMessage>(new NotificationMessage("Username/password is required."));
            }
            else
            {
                if (! (await _serverConnection.Connect()))
                {
                    IsLogin = false;
                    Messenger.Default.Send<NotificationMessage>(new NotificationMessage("Login failed."));
                }
                else
                {
                    IsLogin = true;
                    RaisePropertyChanged(nameof(UserName));
                }
            }

            CurrentMainWindow.Instance.IsBusy = false;
            return await Task.FromResult(true);
        }
    }
}
