// Decompiled with JetBrains decompiler
// Type: SmartBear.Collaborator.Api.Adapters.UserServiceAdapter
// Assembly: Collaborator.Api, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9c3e7b13c78c6163
// MVID: 82D41E82-1BF6-4105-8B80-342693CB64ED
// Assembly location: C:\Users\fyu\AppData\Local\Microsoft\VisualStudio\16.0_be238a39\Extensions\gzzm4hy2.wuy\Collaborator.Api.dll

using System.Collections.Generic;

namespace CollabAPI
{
  public class UserServiceAdapter : BaseServiceAdapter, IUserService, IBaseService
  {
    private int _lastActionItemsUpdateTime = int.MinValue;
    private int _lastUserInfoUpdateTime = int.MinValue;
    private int _lastAllUsersUpdateTime = int.MinValue;
    private List<User.ActionItem> _actionItems;
    private User.UserInfo _userInfo;
    private const uint _updateIntervalInMsecAllUsers = 120000;
    private List<User.UserInfo> _allUsers;

    public UserServiceAdapter(IServer server)
      : base(server)
    {
    }

    public override void InvalidateCachedData()
    {
      lock (this.UpdateDataLock)
      {
        this._lastActionItemsUpdateTime = int.MinValue;
        this._actionItems = (List<User.ActionItem>) null;
        if (this.Server.Connected)
          return;
        this._lastUserInfoUpdateTime = int.MinValue;
        this._userInfo = (User.UserInfo) null;
        this._lastAllUsersUpdateTime = int.MinValue;
        this._allUsers = (List<User.UserInfo>) null;
      }
    }

    public void DismissCommitTodo(int reviewId)
    {
      UserService.dismissCommitTodo dismissCommitTodo = new UserService.dismissCommitTodo();
      dismissCommitTodo.reviewId = reviewId;
      this.SendRequest((object) dismissCommitTodo, true);
      this.InvalidateCachedData();
    }

    public User.UserInfo GetUserInfo()
    {
      lock (this.UpdateDataLock)
      {
        if (this._userInfo == null || this.NeedToUpdateData(this._lastUserInfoUpdateTime))
        {
          JsonResult jsonResult = this.SendRequest((object) new UserService.getSelfUser(), true);
          if (jsonResult != null)
          {
            this._userInfo = jsonResult.GetResponse<User.UserInfo>();
            this._lastUserInfoUpdateTime = this.GetUpdateTime();
          }
        }
        return this._userInfo;
      }
    }

    public User.SuggestedReviewsResponse GetSuggestedReviews(
      User.SuggestionType suggestionType = User.SuggestionType.UPLOAD,
      string changelistId = "",
      int maxResults = 100)
    {
      UserService.getSuggestedReviews suggestedReviews = new UserService.getSuggestedReviews();
      suggestedReviews.changelistId = changelistId;
      suggestedReviews.suggestionType = suggestionType;
      suggestedReviews.maxResults = maxResults;
      return this.SendRequest((object) suggestedReviews, true)?.GetResponse<User.SuggestedReviewsResponse>();
    }

    public List<User.ActionItem> GetActionItems()
    {
      lock (this.UpdateDataLock)
      {
        if (this._actionItems == null || this.NeedToUpdateData(this._lastActionItemsUpdateTime))
        {
          JsonResult jsonResult = this.SendRequest((object) new UserService.getActionItems(), true);
          if (jsonResult != null)
          {
            this._actionItems = jsonResult.GetResponse<User.GetActionItemsResponse>().actionItems;
            this._lastActionItemsUpdateTime = this.GetUpdateTime();
          }
        }
        return this._actionItems;
      }
    }

    public List<User.UserInfo> GetUserList(bool forceUpdate = false)
    {
      lock (this.UpdateDataLock)
      {
        if (((this._allUsers == null ? 1 : (this.NeedToUpdateData(this._lastAllUsersUpdateTime, 120000U) ? 1 : 0)) | (forceUpdate ? 1 : 0)) != 0)
        {
          JsonResult jsonResult = this.SendRequest((object) new UserService.getUserList(), true);
          if (jsonResult != null)
          {
            this._allUsers = jsonResult.GetResponse<User.UserListResponse>().userList;
            this._lastAllUsersUpdateTime = this.GetUpdateTime();
          }
        }
        return this._allUsers;
      }
    }
  }
}
