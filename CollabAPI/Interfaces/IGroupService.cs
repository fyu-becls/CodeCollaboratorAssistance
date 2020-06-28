using System;
using System.Collections.Generic;
using System.Text;

namespace CollabAPI
{
    public interface IGroupService : IBaseService
    {
        List<Group.GroupDescription> GetGroupList(int reviewId);

        List<Group.GroupMemberDescription> GetMemberList(string groupGuid);
    }
}
