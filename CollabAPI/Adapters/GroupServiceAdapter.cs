// Decompiled with JetBrains decompiler
// Type: SmartBear.Collaborator.Api.Adapters.GroupServiceAdapter
// Assembly: Collaborator.Api, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9c3e7b13c78c6163
// MVID: 82D41E82-1BF6-4105-8B80-342693CB64ED
// Assembly location: C:\Users\fyu\AppData\Local\Microsoft\VisualStudio\16.0_be238a39\Extensions\gzzm4hy2.wuy\Collaborator.Api.dll

using System.Collections.Generic;

namespace CollabAPI
{
  public class GroupServiceAdapter : BaseServiceAdapter, IGroupService, IBaseService
  {
    public GroupServiceAdapter(IServer server)
      : base(server)
    {
    }

    public List<Group.GroupDescription> GetGroupList(int reviewId)
    {
      GroupService.getGroupList getGroupList = new GroupService.getGroupList();
      getGroupList.reviewId = reviewId;
      return this.SendRequest((object) getGroupList, true)?.GetResponse<Group.GroupListResponse>().groupList;
    }

    public List<Group.GroupMemberDescription> GetMemberList(string groupGuid)
    {
      GroupService.getMembers getMembers = new GroupService.getMembers();
      getMembers.guid = groupGuid;
      return this.SendRequest((object) getMembers, true)?.GetResponse<Group.GroupMemberListResponse>().groupMemberList;
    }
  }
}
