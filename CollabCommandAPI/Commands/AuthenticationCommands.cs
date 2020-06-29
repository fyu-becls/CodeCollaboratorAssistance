using System;
using System.Collections.Generic;
using System.Text;

namespace CollabCommandAPI
{
    public class AuthenticationCommands
    {
        public string Login(CollabConnectionSettings settings)
        {
            return $"login {settings.ServerURL} {settings.UserName} {settings.UserPassword}";
        }

        public string Login(string url, string username, string password)
        {
            return $"login {url} {username} {password}";
        }

        public string Logout(CollabConnectionSettings settings)
        {
            return $"logout";
        }
    }
}
