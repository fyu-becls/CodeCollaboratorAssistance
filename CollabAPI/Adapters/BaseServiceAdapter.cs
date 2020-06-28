using System;
using System.Collections.Generic;
using System.Text;

namespace CollabAPI
{
    public class BaseServiceAdapter : IBaseService
    {
        private uint _updateIntervalInMsec = 15000;
        private object _updateDataLock = new object();
        private IServer _server;
        private ILogger _log;
        private string _lastError;

        public BaseServiceAdapter(IServer server)
        {
            this._server = server;
            this._log = (ILogger)new Logger(this.GetType());
        }

        protected IServer Server
        {
            get
            {
                return this._server;
            }
        }

        protected ICommandExecutor CommandExecutor
        {
            get
            {
                return this.Server.CommandExecutor;
            }
        }

        protected ISessionInfo SessionInfo
        {
            get
            {
                return this.Server.SessionInfo;
            }
        }

        protected object UpdateDataLock
        {
            get
            {
                return this._updateDataLock;
            }
        }

        public ILogger Log
        {
            get
            {
                return this._log;
            }
        }

        public string LastError
        {
            get
            {
                return this._lastError;
            }
        }

        protected JsonResult SendRequest(object request, bool sendAuthTicket = true)
        {
            if (!this.Server.CanConnect)
                return (JsonResult)null;
            JsonResult jsonResult;
            try
            {
                jsonResult = this.CommandExecutor.ExecuteCommand(request, sendAuthTicket);
                if (jsonResult.IsError())
                {
                    this._lastError = jsonResult.GetErrorString(false);
                    this.Log.LogError(jsonResult.GetErrorString(true));
                    jsonResult = (JsonResult)null;
                }
            }
            catch (Exception ex)
            {
                this.Log.LogException(ex);
                jsonResult = (JsonResult)null;
            }
            return jsonResult;
        }

        protected int GetUpdateTime()
        {
            return Environment.TickCount & int.MaxValue;
        }

        protected bool NeedToUpdateData(int dataLastUpdate)
        {
            return this.NeedToUpdateData(dataLastUpdate, this._updateIntervalInMsec);
        }

        protected bool NeedToUpdateData(int dataLastUpdate, uint updateIntervalInMsec)
        {
            return (long)this.GetUpdateTime() - ((long)dataLastUpdate + (long)updateIntervalInMsec) >= (long)updateIntervalInMsec;
        }

        public virtual void InvalidateCachedData()
        {
        }
    }
}
