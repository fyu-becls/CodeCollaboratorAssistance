using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace CollabAPI
{
    public interface ISessionInfo
    {
        string UserTicket { get; }

        string ServerURL { get; set; }

        string UserName { get; set; }

        string UserPassword { get; set; }

        bool UseProxy { get; set; }

        string ProxyHost { get; set; }

        int ProxyPort { get; set; }

        string ProxyLogin { get; set; }

        string ProxyPassword { get; set; }

        int UserId { get; }

        string ServerVersion { get; }

        int ServerBuild { get; }

        int ClientBuild { get; set; }

        string ClientVersion { get; set; }

        string ClientGuid { get; set; }

        HttpClient Client { get; }
    }
}
