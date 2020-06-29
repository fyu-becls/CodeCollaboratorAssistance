using ExecuteCommandLineProgram;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace CollabCommandAPI
{
    public class CollabServerConnection : ICollabServerConnection, ICollabCommandExecutor
    {
        private readonly CollabConnectionSettings _collabConnectionSettings;
        private readonly string _workspaceLocation;
        private readonly AuthenticationCommands _authenticationCommands;
        public CollabServerConnection(CollabConnectionSettings settings, AuthenticationCommands authenticationCommands)
        {
            _collabConnectionSettings = settings;
            _authenticationCommands = authenticationCommands;
            _workspaceLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        public bool CanConnect()
        {
            return !string.IsNullOrWhiteSpace(_collabConnectionSettings.ServerURL) && Uri.IsWellFormedUriString(_collabConnectionSettings.ServerURL, UriKind.Absolute);
        }

        public async Task<bool> Connect()
        {
            var result = await ExecuteCommand(_authenticationCommands.Login(_collabConnectionSettings), 5000);
            return result.ExitCode == 0;
        }

        public async Task<bool> Connect(string username, string password)
        {
            var result = await ExecuteCommand(_authenticationCommands.Login(_collabConnectionSettings.ServerURL, username, password), 5000);
            return result.ExitCode == 0;
        }

        public async Task<bool> Disconnect()
        {
            var result = await ExecuteCommand(_authenticationCommands.Logout(_collabConnectionSettings), 5000);
            return result.ExitCode == 0;
        }

        public event EventHandler<string> NewOutputEvent;

        public async Task<CommandLineProgramProcessResult> ExecuteCommand(string args, int timeout)
        {
            var result = await Task.Run(() =>
            {
                return CommandLineProgramProcess.RunProgram(_collabConnectionSettings.CollabExeLocation,
                    _workspaceLocation, args, timeout);
            });

            //if (!string.IsNullOrWhiteSpace(result.StandardOutput))
            //{
            //    NewOutputEvent?.Invoke(this, result.StandardOutput);
            //}
            if (!string.IsNullOrWhiteSpace(result.StandardError))
            {
                NewOutputEvent?.Invoke(this, result.StandardError);
            }

            return result;
        }

        public bool IsConnected()
        {
            throw new NotImplementedException();
        }
    }
}
