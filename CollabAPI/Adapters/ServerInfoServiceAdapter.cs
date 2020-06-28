// Decompiled with JetBrains decompiler
// Type: SmartBear.Collaborator.Api.Adapters.ServerInfoServiceAdapter
// Assembly: Collaborator.Api, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9c3e7b13c78c6163
// MVID: 82D41E82-1BF6-4105-8B80-342693CB64ED
// Assembly location: C:\Users\fyu\AppData\Local\Microsoft\VisualStudio\16.0_be238a39\Extensions\gzzm4hy2.wuy\Collaborator.Api.dll

using System;
using System.Collections.Generic;

namespace CollabAPI
{
  public class ServerInfoServiceAdapter : BaseServiceAdapter, IServerInfoService, IBaseService
  {
    private const int INCORRECT_BUILD = -1;

    public ServerInfoServiceAdapter(IServer server)
      : base(server)
    {
    }

    public string GetVersion()
    {
      return this.SendRequest((object) new ServerInfoService.getVersion(), true)?.GetResponse<ServerInfoService.getVersionResponse>().version;
    }

    public int GetServerBuild()
    {
      JsonResult jsonResult = this.SendRequest((object) new ServerInfoService.getServerBuild(), false);
      return jsonResult != null ? jsonResult.GetResponse<ServerInfoService.getServerBuildResponse>().serverBuild : -1;
    }

    public int GetMinimumJavaClientBuild()
    {
      JsonResult jsonResult = this.SendRequest((object) new ServerInfoService.getMinimumJavaClientBuild(), false);
      return jsonResult != null ? jsonResult.GetResponse<ServerInfoService.getMinimumJavaClientBuildResponse>().minimumJavaClientBuild : -1;
    }

    public string GetSystemGlobalValue(string settingName)
    {
      SortedDictionary<string, string> sortedDictionary = (SortedDictionary<string, string>) null;
      ServerInfoService.getSystemGlobalValues systemGlobalValues = new ServerInfoService.getSystemGlobalValues();
      systemGlobalValues.systemGlobalValuesNames = new List<string>();
      systemGlobalValues.systemGlobalValuesNames.Add(settingName);
      JsonResult jsonResult = this.SendRequest((object) systemGlobalValues, true);
      if (jsonResult != null)
        sortedDictionary = jsonResult.GetResponse<ServerInfoService.SystemGlobalValuesResponse>().systemGlobalValues;
      return sortedDictionary != null ? sortedDictionary[settingName] : string.Empty;
    }

    public List<Review.ReviewRemoteSystem> GetRemoteSystems()
    {
      List<Review.ReviewRemoteSystem> reviewRemoteSystemList = new List<Review.ReviewRemoteSystem>();
      ServerInfoService.getRemoteSystemIntegrations systemIntegrations = new ServerInfoService.getRemoteSystemIntegrations();
      systemIntegrations.enabledOnly = true;
      JsonResult jsonResult = this.SendRequest((object) systemIntegrations, true);
      if (jsonResult != null)
      {
        ServerInfoService.RemoteSystemIntegrationsResponse response = jsonResult.GetResponse<ServerInfoService.RemoteSystemIntegrationsResponse>();
        if (response != null && response.remoteSystemIntegrations != null)
        {
          foreach (SystemAdmin.RemoteSystem systemIntegration in response.remoteSystemIntegrations)
          {
            Review.ReviewRemoteSystem reviewRemoteSystem = new Review.ReviewRemoteSystem(systemIntegration);
            reviewRemoteSystemList.Add(reviewRemoteSystem);
          }
          reviewRemoteSystemList.Sort((Comparison<Review.ReviewRemoteSystem>) ((a, b) =>
          {
            if (a.title == null && b.title == null)
              return 0;
            if (a.title == null)
              return -1;
            return b.title == null ? 1 : a.title.CompareTo(b.title);
          }));
        }
      }
      return reviewRemoteSystemList;
    }

    public bool GetAllowAdminReviews()
    {
      string systemGlobalValue = this.GetSystemGlobalValue("allow-sysadmin-reviews");
      try
      {
        return bool.Parse(systemGlobalValue);
      }
      catch (Exception ex)
      {
        this.Log.LogException(ex);
        return false;
      }
    }
  }
}
