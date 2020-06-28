using System;
using System.Collections.Generic;
using System.Text;

namespace CollabAPI
{
    public interface IServer
    {
        bool CanConnect { get; }

        bool Connected { get;  }

        bool IsLoggedIn { get; }

        ICommandExecutor CommandExecutor { get; }

        ISessionInfo SessionInfo { get; }

        IDownloadManager DownloadManager { get; }

        IDiffViewerService DiffViewerService { get; }

        IGroupService GroupService { get; }

        IReviewService ReviewService { get; }

        IServerInfoService ServerInfoService { get; }

        ISessionService SessionService { get; }

        IUserService UserService { get; }
    }
}
