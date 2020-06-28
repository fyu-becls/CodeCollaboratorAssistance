using System;
using System.Collections.Generic;
using System.Text;

namespace CollabCommandAPI
{
    public class CommandsService
    {
        public AuthenticationCommands AuthenticationCommands { get; }
        public CommandsService(AuthenticationCommands authentication)
        {
            AuthenticationCommands = authentication;
        }
    }
}
