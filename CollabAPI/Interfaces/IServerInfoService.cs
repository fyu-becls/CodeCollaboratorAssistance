using System;
using System.Collections.Generic;
using System.Text;

namespace CollabAPI
{
    public interface IServerInfoService : IBaseService
    {
        bool GetAllowAdminReviews();

        int GetMinimumJavaClientBuild();

        List<Review.ReviewRemoteSystem> GetRemoteSystems();

        int GetServerBuild();

        string GetSystemGlobalValue(string settingName);

        string GetVersion();
    }
}
