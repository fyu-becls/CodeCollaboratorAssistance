using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CollabCommandAPI
{
    public interface ICollabServerConnection
    {
        bool IsConnected();

        bool CanConnect();

        Task<bool> Connect();

        Task<bool> Connect(string username, string password);

        Task<bool> Disconnect();

        event EventHandler<string> NewOutputEvent;
    }
}
