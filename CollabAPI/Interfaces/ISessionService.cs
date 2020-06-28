using System;
using System.Collections.Generic;
using System.Text;

namespace CollabAPI
{
    public interface ISessionService : IBaseService
    {
        string GetLoginTicket(string login, string password);
    }
}