// Decompiled with JetBrains decompiler
// Type: SmartBear.Collaborator.Api.Adapters.SessionServiceAdapter
// Assembly: Collaborator.Api, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9c3e7b13c78c6163
// MVID: 82D41E82-1BF6-4105-8B80-342693CB64ED
// Assembly location: C:\Users\fyu\AppData\Local\Microsoft\VisualStudio\16.0_be238a39\Extensions\gzzm4hy2.wuy\Collaborator.Api.dll


namespace CollabAPI
{
  public class SessionServiceAdapter : BaseServiceAdapter, ISessionService, IBaseService
  {
    public SessionServiceAdapter(IServer server)
      : base(server)
    {
    }

    public string GetLoginTicket(string login, string password)
    {
      JsonResult jsonResult = this.CommandExecutor.ExecuteCommand((object) new SessionService.getLoginTicket()
      {
        login = login,
        password = password
      }, false);
      if (!jsonResult.IsError())
        return jsonResult.GetResponse<SessionService.getLoginTicketResponse>().loginTicket;
      this.Log.LogError(jsonResult.GetErrorString(true));
      return (string) null;
    }
  }
}
