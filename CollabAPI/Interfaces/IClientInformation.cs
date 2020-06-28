using System;
using System.Collections.Generic;
using System.Text;

namespace CollabAPI
{
    public interface IClientInformation
    {
        int Build { get; }

        string Guid { get; }

        string Version { get; }
    }
}
