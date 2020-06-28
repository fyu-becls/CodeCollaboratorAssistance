using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace CollabAPI
{
    public class JsonCommand
    {
        public string command;
        public object args;

        public JsonCommand(object commandObject)
        {
            if (commandObject == null)
                throw new Exception("commandObject can't be null!");

            // A bit of a hack, but this translates the nested class name into the command name
            command = commandObject.GetType().ToString(); // CollabAddIn.SessionService+getLoginTicket
            command = command.Substring(command.LastIndexOf('.') + 1); // SessionService+getLoginTicket
            command = command.Replace('+', '.'); // SessionService.getLoginTicket

            // Our API request object that will later get serialized into the args JSON array
            args = commandObject;
        }
    }
}
