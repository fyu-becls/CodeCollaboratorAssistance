using CollabAPI;
using System.Threading.Tasks;

namespace CodeCollaboratorClient.CollabAPIManager
{
    public class APIManager
    {
        public string ServerUrl { get; } = "http://svcndalcodeco:8080";

        private Server server;

        public async Task<Server> GetCollaboratorServer(string user, string password)
        {
            server = new Server(ServerUrl);

            if (user != null && password != null)
            {
                server.Connect(user, password);
            }

            if (server.Connected)
            {
            }
            else
            {
                server = null;
            }

            return server;
        }
    }
}
