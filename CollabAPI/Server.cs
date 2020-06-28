// Decompiled with JetBrains decompiler
// Type: SmartBear.Collaborator.Api.Server
// Assembly: Collaborator.Api, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9c3e7b13c78c6163
// MVID: 82D41E82-1BF6-4105-8B80-342693CB64ED
// Assembly location: C:\Users\fyu\AppData\Local\Microsoft\VisualStudio\16.0_be238a39\Extensions\gzzm4hy2.wuy\Collaborator.Api.dll

using System;
using System.Collections.Generic;

namespace CollabAPI
{
  public class Server : IServer
  {
    private Logger _log;
    private ServerConnection _serverConnection;
    private IDiffViewerService _diffViewerService;
    private IGroupService _groupService;
    private IReviewService _reviewService;
    private IServerInfoService _serverInfoService;
    private ISessionService _sessionService;
    private IUserService _userService;
    private IDownloadManager _downloadManager;

        public Server() : this("http://svcndalcodeco:8080")
        {

        }
    public Server(string Url)
    {
      this._log = new Logger(typeof (Server));
            this._serverConnection = new ServerConnection((IServer)this, new ConnectionSettings
            {
                ServerURL = Url
            }); ;
    }

    //public static IServer Instance
    //{
    //  get
    //  {
    //    if (Server._server == null)
    //      Server._server = new Server();
    //    return (IServer) Server._server;
    //  }
    //}

    public bool CheckConnectionSettings(
      ConnectionSettings connectionSettings,
      out string errorMessage,
      IClientInformation clientInformation)
    {
      errorMessage = (string) null;
      try
      {
        SessionService.getLoginTicket getLoginTicket = new SessionService.getLoginTicket();
        getLoginTicket.login = connectionSettings.UserName;
        getLoginTicket.password = connectionSettings.UserPassword;
        List<object> requests = new List<object>();
        requests.Add((object) getLoginTicket);
        List<JsonResult> jsonResultList = (List<JsonResult>) null;
        using (ServerConnection serverConnection = new ServerConnection(this, connectionSettings))
        {
          serverConnection.ClientBuild = clientInformation.Build;
          serverConnection.ClientGuid = clientInformation.Guid;
          serverConnection.ClientVersion = clientInformation.Version;
          serverConnection.CheckClientServerCompatible();
          jsonResultList = serverConnection.ExecuteCommands(requests, false);
        }
        if (jsonResultList.Count == 1)
        {
          JsonResult jsonResult = jsonResultList[0];
          if (jsonResult.IsError())
          {
            errorMessage = jsonResult.GetErrorString(true);
            return false;
          }
        }
        else
        {
          errorMessage = "c_error_UnexpectedError";
          return false;
        }
      }
      catch (Exception ex)
      {
        errorMessage = ex.Message;
        return false;
      }
      return true;
    }

    public bool CanConnect
    {
      get
      {
        return this._serverConnection.CanConnect();
      }
    }

    public bool Connected
    {
      get
      {
        return this._serverConnection.IsConnected();
      }
    }

        public bool Connect(string userName, string password)
        {
            _serverConnection.UserName = userName;
            _serverConnection.UserPassword = password;

            return _serverConnection.Connect();
        }


        public bool ConnectByTiket(string userName, string tiket)
        {
            _serverConnection.UserName = userName;
            _serverConnection.UserTicket = tiket;

            return _serverConnection.Connect();
        }

        public bool IsLoggedIn
    {
      get
      {
        return !string.IsNullOrWhiteSpace(this._serverConnection.UserTicket);
      }
    }

    public ICommandExecutor CommandExecutor
    {
      get
      {
        return (ICommandExecutor) this._serverConnection;
      }
    }

    public ISessionInfo SessionInfo
    {
      get
      {
        return (ISessionInfo) this._serverConnection;
      }
    }

    public IDownloadManager DownloadManager
    {
      get
      {
        if (this._downloadManager == null)
          this._downloadManager = (IDownloadManager) new DownloadManager((IServer) this);
        return this._downloadManager;
      }
    }

    public IDiffViewerService DiffViewerService
    {
      get
      {
        if (this._diffViewerService == null)
          this._diffViewerService = (IDiffViewerService) new DiffViewerServiceAdapter((IServer) this);
        return this._diffViewerService;
      }
    }

    public IGroupService GroupService
    {
      get
      {
        if (this._groupService == null)
          this._groupService = (IGroupService) new GroupServiceAdapter((IServer) this);
        return this._groupService;
      }
    }

    public IReviewService ReviewService
    {
      get
      {
        if (this._reviewService == null)
          this._reviewService = (IReviewService) new ReviewServiceAdapter((IServer) this);
        return this._reviewService;
      }
    }

    public IServerInfoService ServerInfoService
    {
      get
      {
        if (this._serverInfoService == null)
          this._serverInfoService = (IServerInfoService) new ServerInfoServiceAdapter((IServer) this);
        return this._serverInfoService;
      }
    }

    public ISessionService SessionService
    {
      get
      {
        if (this._sessionService == null)
          this._sessionService = (ISessionService) new SessionServiceAdapter((IServer) this);
        return this._sessionService;
      }
    }

    public IUserService UserService
    {
      get
      {
        if (this._userService == null)
          this._userService = (IUserService) new UserServiceAdapter((IServer) this);
        return this._userService;
      }
    }
  }
}
