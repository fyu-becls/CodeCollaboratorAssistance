using System;
using System.Collections.Generic;
using System.Text;

namespace CollabAPI
{
    public struct ConnectionSettings
    {
        public string ServerURL { get; set; }

        public string UserName { get; set; }

        public string UserPassword { get; set; }

        public bool UseProxy { get; set; }

        public string ProxyHost { get; set; }

        public int ProxyPort { get; set; }

        public string ProxyLogin { get; set; }

        public string ProxyPassword { get; set; }
    }
}
