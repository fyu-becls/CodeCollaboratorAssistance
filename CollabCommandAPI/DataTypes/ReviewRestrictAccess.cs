using System;
using System.Collections.Generic;
using System.Text;

namespace CollabCommandAPI.DataTypes
{
    public static class ReviewRestrictAccess
    {
        public const string Anyone = "anyone";
        public const string Group = "group";
        public const string Participants = "participants";
        public const string GroupAndParticipants = "group-and-participants";
        public const string GroupOrParticipants = "group-or-participants";
    }
}
