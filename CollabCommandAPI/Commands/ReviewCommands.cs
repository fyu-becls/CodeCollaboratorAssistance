using System;
using System.Collections.Generic;
using System.Text;

namespace CollabCommandAPI.Commands
{
    public class ReviewCommands
    {
        public string Create(string title, List<ReviewCustomField> customFields, string deadline, List<ReviewCustomField> participantCustomFields, string restrictAccess)
        {
            return $"admin review create --title '{title}' --deadline '{deadline}' --restrict-access '{restrictAccess}'";
        }
    }
}
