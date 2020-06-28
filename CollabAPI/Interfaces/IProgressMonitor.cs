using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CollabAPI
{
    public interface IProgressMonitor
    {
        string WaitMessage { get; set; }

        string ProgressText { get; set; }

        string StatusBarText { get; set; }

        bool IsHidden { get; }

        bool HasCanceled { get; }

        void Hide();

        void Show();

        int TotalStepsCount { get; set; }

        int CompletedStepsCount { get; set; }

        CancellationTokenSource CancellationTokenSource { get; }
    }
}
