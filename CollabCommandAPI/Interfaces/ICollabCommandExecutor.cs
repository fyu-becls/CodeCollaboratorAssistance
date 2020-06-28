using ExecuteCommandLineProgram;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CollabCommandAPI
{
    public interface ICollabCommandExecutor
    {
        Task<CommandLineProgramProcessResult> ExecuteCommand(string args, int timeout);
    }
}