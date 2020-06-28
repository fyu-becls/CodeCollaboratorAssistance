using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CollabAPI
{
    public interface ICommandExecutor
    {
        JsonResult ExecuteCommand(object request, bool sendAuthTicket = true);

        List<JsonResult> ExecuteCommands(List<object> requests, bool sendAuthTicket = true);

        Task<List<JsonResult>> PostMultipartRequest(
          object request,
          string fileToAttach,
          IProgressMonitor progressMonitor);
    }
}
