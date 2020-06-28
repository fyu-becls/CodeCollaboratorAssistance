using System;
using System.Collections.Generic;
using System.Text;

namespace CollabCommandAPI
{
    public class CollabConnectionSettings
    {
        public CollabConnectionSettings()
        {
            CollabExeLocation = "ccollab";
        }

        public string CollabExeLocation { get; set; }

        public string ServerURL;

        public string UserName;

        public string UserPassword;
    }
}
