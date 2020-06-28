using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
#pragma warning disable 0649 // Expected warnings in JSON classes
namespace CollabAPI
{
    public class SessionService
    {
        public class getLoginTicket
        {
            public string login;
            public string password;
        }

        public class getLoginTicketResponse
        {
            public string loginTicket;
        }

        public class authenticate
        {
            public string login;
            public string ticket;
        }

        public class authenticateResponse
        {
            // void response return type
        }

        public class setMetadata
        {
            public string clientName;
            public string expectedServerVersion;
            public string clientStringId;
        }

        public class setMetadataResponse
        {
            // void response return type
        }

        public static JsonCommand GetSetMetadataCommand(
            string clientName,
            string expectedServerVersion)
        {
            return new JsonCommand((object)new SessionService.setMetadata()
            {
                clientName = clientName,
                expectedServerVersion = expectedServerVersion,
                clientStringId = "6"
            });
        }

        public static JsonCommand GetAuthenticateCommand(string login, string ticket)
        {
            return new JsonCommand((object)new SessionService.authenticate()
            {
                login = login,
                ticket = ticket
            });
        }
    }
}
#pragma warning restore 0649 // Expected warnings in JSON classes