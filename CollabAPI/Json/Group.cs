using System;
using System.Collections.Generic;
using System.Text;

namespace CollabAPI
{
    public class Group
    {
        public interface IHasGuid
        {
            string guid { get; set; }
        }

        public class HasGuid : Group.IHasGuid
        {
            public string guid { get; set; }
        }

        public class GroupListResponse
        {
            public List<Group.GroupDescription> groupList { get; set; }
        }

        public class GroupDescription : Group.EditGroupRequest
        {
            public int level { get; set; }

            public bool isAllUsers { get; set; }

            public bool isBuiltIn { get; set; }

            public bool isSync { get; set; }
        }

        public class CreateGroupRequest
        {
            public string description { get; set; }

            public string guid { get; set; }

            public string title { get; set; }

            public bool allowAssociateWithReviews { get; set; }

            public bool enabled { get; set; }

            public bool reviewPool { get; set; }
        }

        public class EditGroupRequest : Group.CreateGroupRequest, Group.IHasGuid
        {
        }

        public class GroupMemberDescription
        {
            public string guid { get; set; }

            public string name { get; set; }

            public Group.GroupMemberDescription.TYPE type { get; set; }

            public enum TYPE
            {
                USER,
                ADMIN,
                GROUP,
            }
        }

        public class GroupMemberListResponse
        {
            public List<Group.GroupMemberDescription> groupMemberList { get; set; }
        }
    }
}
