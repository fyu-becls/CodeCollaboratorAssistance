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

        public string Logout(CollabConnectionSettings settings)
        {
            return $"logout";
        }
    }
}
