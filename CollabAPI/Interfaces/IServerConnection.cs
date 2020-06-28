using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace CollabAPI
{
    public interface IServerConnection
    {
        bool IsConnected();

        bool CanConnect();

        bool Connect();

        bool Disconnect();

        HttpClient Client { get; }
    }
}
