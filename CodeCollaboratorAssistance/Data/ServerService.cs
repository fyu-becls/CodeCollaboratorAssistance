using Blazored.SessionStorage;
using CollabAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeCollaboratorAssistance.Data
{
    public class ServerService
    {
        private readonly ISessionStorageService _sessionService;
        public ServerService(ISessionStorageService sessionService)
        {
            _sessionService = sessionService;
        }

        public string ServerUrl { get; } = "http://svcndalcodeco:8080";

        private Server server;

        public async Task<Server> GetCollaboratorServer(string user = null, string password = null)
        {
            if (server != null)
            {
                return server;
            }

            server = new Server(ServerUrl);

            if (user != null && password != null)
            {
                server.Connect(user, password);
            }
            else
            {
                user = user ?? await _sessionService.GetItemAsync<string>("username");
                var ticket = await _sessionService.GetItemAsync<string>("ticket");

                if (ticket != null)
                {
                    server.ConnectByTiket(user, ticket);
                }
            }

            if (server.Connected)
            {
                await _sessionService.SetItemAsync("username", server.SessionInfo.UserName);
                await _sessionService.SetItemAsync("ticket", server.SessionInfo.UserTicket);
            }
            else
            {
                server = null;
            }

            return server;
        }
    }
}
