using System;
using System.Collections.Generic;
using System.Text;

namespace CollabAPI
{
    public interface ILogger
    {
        void LogException(Exception exception);

        void LogError(string message);

        void LogWarningMessage(string message);

        void LogInfoMessage(string message);

        void LogDebugMessage(string message);

        void LogDebugMessage(string message, Exception exception);

        bool IsShowErrorMessage { get; set; }

        void ShowError(string error);
    }
}
