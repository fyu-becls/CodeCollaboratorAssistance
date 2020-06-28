using CollabCommandAPI;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeCollaboratorClient.Messages
{
    public class OpenLoginDialogMessage : MessageBase
    {
        public CollabConnectionSettings Settings { get; set; }

        public OpenLoginDialogMessage(CollabConnectionSettings settings)
        {
            Settings = settings;
        }
    }
}
