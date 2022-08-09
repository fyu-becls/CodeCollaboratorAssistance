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

    public class Logger : ILogger
    {
        public bool IsShowErrorMessage { get; set; }

        public Logger(Type t)
        { }

        public void LogDebugMessage(string message)
        {
        }

        public void LogDebugMessage(string message, Exception exception)
        {
        }

        public void LogError(string message)
        {
        }

        public void LogException(Exception exception)
        {
        }

        public void LogInfoMessage(string message)
        {
        }

        public void LogWarningMessage(string message)
        {
        }

        public void ShowError(string error)
        {
        }
    }
}
