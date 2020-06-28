using Blazored.SessionStorage;
using CollabAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeCollaboratorAssistance.Data
{
    public class AuthenticationService
    {
        private readonly ISessionStorageService _sessionStorage;
        private readonly ServerService _serverService;

        public AuthenticationService(ISessionStorageService sessionStorage, ServerService serverService)
        {
            _sessionStorage = sessionStorage;
            _serverService = serverService;
        }

        private string ServerUrl { get => _serverService.ServerUrl; }

        public async Task<bool> IsAuthenticated()
        {
            //var username = await _sessionStorage.GetItemAsync<string>("username");
            //var ticket = await _sessionStorage.GetItemAsync<string>("ticket");
            //if (ticket != null)
            //{
            //    //var api = new CodeCollabAPI(ServerUrl, username, ticket);
            //    //return api.checkTicketValidity();
            //    var server = new Server(ServerUrl);
            //    return server.ConnectByTiket(username, ticket);
            //}

            return (await _serverService.GetCollaboratorServer()) != null;
        }

        public async Task<bool> AuthenticateAsync(string username, string password)
        {
            var server = await _serverService.GetCollaboratorServer(username, password);
            return server != null;

            //try
            //{
            //    var server = new Server(ServerUrl);
            //    if (server.Connect(username, password))
            //    {
            //        isAuthenticated = true;
            //        await _sessionStorage.SetItemAsync("ticket", server.SessionInfo.UserTicket);
            //        await _sessionStorage.SetItemAsync("server", server);
            //    }

            //    //var api = new CodeCollabAPI(ServerUrl, username);
            //    //if (!api.getLoginTicket(password).IsError())
            //    //{
            //    //    isAuthenticated = true;
            //    //    await _sessionStorage.SetItemAsync("ticket", api.userTicket);
            //    //}
            //}
            //catch (Exception e)
            //{
            //    isAuthenticated = false;
            //}

            //if (!isAuthenticated)
            //{
            //    await _sessionStorage.RemoveItemAsync("ticket");
            //}

            //return isAuthenticated;
        }
    }
}
