using System;
using System.Collections.Generic;
using System.Text;

namespace CollabAPI
{
    public interface IBaseService
    {
        string LastError { get; }

        ILogger Log { get; }

        void InvalidateCachedData();
    }
}
