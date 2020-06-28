using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Net;

namespace CollabAPI
{
    public class ServerConnection : IServerConnection, ICommandExecutor, ISessionInfo, IDisposable
    {
        private const string CLIENT_VERSION_NOT_COMPATIBLE_WITH_SERVER_VERSION_FORMAT = "The client version {0} is incompatible with the server version {1}.";
        private const string CLIENT_VERSION_NOT_HIGHER_MINIMAL_REQUIRED_VERSION_FORMAT = "The client build version {0} is lower than the minimum client build {1} required by the server.";
        private ILogger _log;
        private IServer _server;
        private HttpClient _httpClient;
        private ConnectionSettings _connectionSettings;
        private string _serverVersion;
        private string _userTicket;
        private int _userId;

        public ServerConnection(IServer server)
          : this(server, new ConnectionSettings())
        {
        }

        public ServerConnection(IServer server, ConnectionSettings connectionSettings)
        {
            ClientBuild = 12500;
            ClientGuid = "70fddf50-5742-47f2-8f1d-d951236a9d5a";
            this._server = server;
            this._log = (ILogger)new Logger(typeof(ServerConnection));
            this._connectionSettings = connectionSettings;
            this.NeedToUpdateHttpClient = true;
        }

        ~ServerConnection()
        {
            this.Dispose();
        }

        public void UpdateUserId()
        {
            if (this.HasUserTicket())
            {
                User.UserInfo userInfo = this.Server.UserService.GetUserInfo();
                if (userInfo != null)
                {
                    this.UserId = userInfo.id;
                    this._log.LogDebugMessage("CONNECTED as " + this.UserName + ". UserLoginOrId ticket: *************" + this.UserTicket.Substring(this.UserTicket.Length - 5, 5));
                    return;
                }
            }
            this._log.LogDebugMessage("DISCONNECTED FROM SERVER.");
            this.UserId = -1;
        }

        private bool HasUserTicket()
        {
            return !string.IsNullOrWhiteSpace(this.UserTicket);
        }

        private IServer Server
        {
            get
            {
                return this._server;
            }
        }

        private bool CheckExecuteCommandArguments(
          List<object> requests,
          bool sendAuthTicket,
          bool throwException)
        {
            string serverUrl = this.ServerURL;
            if (string.IsNullOrWhiteSpace(serverUrl))
            {
                if (throwException)
                    throw new ArgumentException("Server URL cannot be empty");
                return false;
            }
            if (!Uri.IsWellFormedUriString(serverUrl, UriKind.Absolute))
            {
                if (throwException)
                    throw new ArgumentException("Server URL must be a well formed URI");
                return false;
            }
            if (string.IsNullOrWhiteSpace(this.UserName))
            {
                if (throwException)
                    throw new ArgumentException("User name cannot be empty");
                return false;
            }
            if (((requests == null ? 0 : (requests.Count > 0 ? 1 : 0)) | (sendAuthTicket ? 1 : 0)) == 0)
            {
                if (throwException)
                    throw new ArgumentException("No requests to send");
                return false;
            }
            bool flag = !string.IsNullOrWhiteSpace(this.UserTicket);
            if (!sendAuthTicket || flag)
                return true;
            if (throwException)
                throw new ArgumentException("You don't have a ticket! Call getUserTicket() first.");
            return false;
        }

        private List<JsonCommand> PrepareJSonCommands(
          List<object> requests,
          bool sendAuthTicket)
        {
            List<JsonCommand> jsonCommandList = new List<JsonCommand>();
            jsonCommandList.Add(SessionService.GetSetMetadataCommand(this.ClientGuid, this.ClientVersion));
            if (sendAuthTicket)
                jsonCommandList.Add(SessionService.GetAuthenticateCommand(this.UserName, this.UserTicket));
            if (jsonCommandList != null)
            {
                foreach (object request in requests)
                    jsonCommandList.Add(new JsonCommand(request));
            }
            return jsonCommandList;
        }

        private List<JsonResult> ProcessJSonResults(
          List<JsonResult> allResults,
          bool sendAuthTicket)
        {
            List<JsonResult> jsonResultList = new List<JsonResult>();
            if (allResults != null && allResults.Count<JsonResult>() > 0)
            {
                List<JsonResult>.Enumerator enumerator = allResults.GetEnumerator();
                enumerator.MoveNext();
                if (enumerator.Current != null && enumerator.Current.IsError())
                    throw new Exception(enumerator.Current.GetErrorString(true));
                if (sendAuthTicket)
                {
                    enumerator.MoveNext();
                    if (enumerator.Current != null && enumerator.Current.IsError())
                        throw new Exception(enumerator.Current.GetErrorString(true));
                }
                while (enumerator.MoveNext())
                    jsonResultList.Add(enumerator.Current);
            }
            return jsonResultList;
        }

        public JsonResult ExecuteCommand(object request, bool sendAuthTicket = true)
        {
            List<object> requests = new List<object>();
            if (request != null)
                requests.Add(request);
            List<JsonResult> source = this.ExecuteCommands(requests, sendAuthTicket);
            return source != null && source.Count > 0 ? source.First<JsonResult>() : (JsonResult)null;
        }

        public List<JsonResult> ExecuteCommands(
          List<object> requests,
          bool sendAuthTicket = true)
        {
            this.CheckExecuteCommandArguments(requests, sendAuthTicket, true);
            return this.ProcessJSonResults(ServerRequestSender.Execute((ISessionInfo)this, this.PrepareJSonCommands(requests, sendAuthTicket)), sendAuthTicket);
        }

        public async Task<List<JsonResult>> PostMultipartRequest(
          object request,
          string fileToAttach,
          IProgressMonitor progressMonitor)
        {
            ServerConnection serverConnection = this;
            List<object> requests = new List<object>();
            requests.Add(request);
            serverConnection.CheckExecuteCommandArguments(requests, true, false);
            List<JsonCommand> commands = serverConnection.PrepareJSonCommands(requests, true);
            List<JsonResult> allResults = await ServerRequestSender.ExecuteAsync((ISessionInfo)serverConnection, commands, fileToAttach, progressMonitor).ConfigureAwait(false);
            return serverConnection.ProcessJSonResults(allResults, true);
        }

        public bool CanConnect()
        {
            return !string.IsNullOrWhiteSpace(this.ServerURL) && Uri.IsWellFormedUriString(this.ServerURL, UriKind.Absolute) && !string.IsNullOrWhiteSpace(this.UserName);
        }

        public bool Connect()
        {
            if (this.HasUserTicket())
                return true;
            try
            {
                this.CheckClientServerCompatible();
                this.UserTicket = this.Server.SessionService.GetLoginTicket(this.UserName, this.UserPassword);
            }
            catch (Exception ex)
            {
                this.Disconnect();
                this._log.LogError(ex.Message);
            }
            return this.HasUserTicket();
        }

        public void CheckClientServerCompatible()
        {
            string message = string.Empty;
            int clientBuild = this.ClientBuild;
            int minimumClientBuild = this.MinimumClientBuild;
            try
            {
                if (clientBuild < minimumClientBuild)
                {
                    message = string.Format("The client build version {0} is lower than the minimum client build {1} required by the server.", (object)clientBuild, (object)minimumClientBuild);
                }
                else
                {
                    int serverBuild = this.ServerBuild;
                    if (serverBuild / 100 >= clientBuild / 100)
                        return;
                    message = string.Format("The client version {0} is incompatible with the server version {1}.", (object)clientBuild, (object)serverBuild);
                }
            }
            finally
            {
                if (!string.IsNullOrEmpty(message))
                    throw new Exception(message);
            }
        }

        public bool Disconnect()
        {
            this.UserTicket = "";
            this._serverVersion = (string)null;
            return true;
        }

        public bool IsConnected()
        {
            if (!this.HasUserTicket())
                return false;
            try
            {
                this.ExecuteCommand((object)null, true);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public HttpClient Client
        {
            get
            {
                if (this.NeedToUpdateHttpClient)
                {
                    if (this._httpClient != null)
                        this._httpClient.Dispose();
                    this._httpClient = this.CreateHttpClient();
                    this.NeedToUpdateHttpClient = false;
                }
                return this._httpClient;
            }
        }

        private bool NeedToUpdateHttpClient { get; set; }

        private HttpClient CreateHttpClient()
        {
            HttpClient httpClient;
            if (this.UseProxy)
                httpClient = new HttpClient((HttpMessageHandler)new HttpClientHandler()
                {
                    Proxy = this.GetProxy(),
                    UseProxy = true
                });
            else
                httpClient = new HttpClient();
            return httpClient;
        }

        private IWebProxy GetProxy()
        {
            IWebProxy webProxy1 = WebRequest.DefaultWebProxy;
            if (this.UseProxy)
            {
                WebProxy webProxy2 = new WebProxy(this.ProxyHost, this.ProxyPort);
                webProxy2.BypassProxyOnLocal = false;
                if (!string.IsNullOrWhiteSpace(this.ProxyLogin))
                    webProxy2.Credentials = (ICredentials)new NetworkCredential(this.ProxyLogin, this.ProxyPassword);
                webProxy1 = (IWebProxy)webProxy2;
            }
            return webProxy1;
        }

        public string UserTicket
        {
            get
            {
                return this._userTicket;
            }
            set
            {
                if (string.Equals(this._userTicket, value))
                    return;
                this._userTicket = value;
                this.UpdateUserId();
            }
        }

        public string ServerURL
        {
            get
            {
                return this._connectionSettings.ServerURL;
            }
            set
            {
                if (!(value != this._connectionSettings.ServerURL))
                    return;
                this._connectionSettings.ServerURL = value;
                this._log.LogInfoMessage("Server URL set to: " + value);
                this.UserTicket = (string)null;
            }
        }

        public int UserId
        {
            get
            {
                return this._userId;
            }
            private set
            {
                if (object.Equals((object)this._userId, (object)value))
                    return;
                this._userId = value;
                this._log.LogDebugMessage("UserId = " + (object)this._userId);
            }
        }

        public string UserName
        {
            get
            {
                return this._connectionSettings.UserName;
            }
            set
            {
                if (!(value != this._connectionSettings.UserName))
                    return;
                this._connectionSettings.UserName = value;
                this._log.LogInfoMessage("User name set to: " + value);
                this.UserTicket = (string)null;
            }
        }

        public string UserPassword
        {
            get
            {
                return this._connectionSettings.UserPassword;
            }
            set
            {
                string str = string.IsNullOrEmpty(value) ? string.Empty : value;
                if (!(str != this._connectionSettings.UserPassword))
                    return;
                this._connectionSettings.UserPassword = str;
                this._log.LogInfoMessage("User password was changed");
                this.UserTicket = (string)null;
            }
        }

        public bool UseProxy
        {
            get
            {
                return this._connectionSettings.UseProxy;
            }
            set
            {
                if (value == this._connectionSettings.UseProxy)
                    return;
                this._connectionSettings.UseProxy = value;
                this.UserTicket = (string)null;
                this.NeedToUpdateHttpClient = true;
            }
        }

        public string ProxyHost
        {
            get
            {
                return this._connectionSettings.ProxyHost;
            }
            set
            {
                if (!(value != this._connectionSettings.ProxyHost))
                    return;
                this._connectionSettings.ProxyHost = value;
                this.UserTicket = (string)null;
                this.NeedToUpdateHttpClient = true;
            }
        }

        public int ProxyPort
        {
            get
            {
                return this._connectionSettings.ProxyPort;
            }
            set
            {
                if (value == this._connectionSettings.ProxyPort)
                    return;
                this._connectionSettings.ProxyPort = value;
                this.UserTicket = (string)null;
                this.NeedToUpdateHttpClient = true;
            }
        }

        public string ProxyLogin
        {
            get
            {
                return this._connectionSettings.ProxyLogin;
            }
            set
            {
                if (!(value != this._connectionSettings.ProxyLogin))
                    return;
                this._connectionSettings.ProxyLogin = value;
                this.UserTicket = (string)null;
                this.NeedToUpdateHttpClient = true;
            }
        }

        public string ProxyPassword
        {
            get
            {
                return this._connectionSettings.ProxyPassword;
            }
            set
            {
                if (!(value != this._connectionSettings.ProxyPassword))
                    return;
                this._connectionSettings.ProxyPassword = value;
                this.UserTicket = (string)null;
                this.NeedToUpdateHttpClient = true;
            }
        }

        public string ServerVersion
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this._serverVersion))
                    this._serverVersion = this.Server.ServerInfoService.GetVersion();
                return this._serverVersion;
            }
        }

        public int ServerBuild
        {
            get
            {
                return this.Server.ServerInfoService.GetServerBuild();
            }
        }

        public int ClientBuild { get; set; }

        public string ClientGuid { get; set; }

        public string ClientVersion { get; set; }

        public int MinimumClientBuild
        {
            get
            {
                return this.Server.ServerInfoService.GetMinimumJavaClientBuild();
            }
        }

        public string CollaboratorEndpoint { private get; set; }

        public void Dispose()
        {
            if (this._httpClient == null)
                return;
            this._httpClient.Dispose();
        }
    }
}
