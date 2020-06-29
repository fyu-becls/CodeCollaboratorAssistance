using System.Security.Principal;

namespace CodeCollaboratorClient.Authentication
{
    public class CustomIdentity : IIdentity
    {
        public CustomIdentity(string name, string[] roles)
        {
            Name = name;
            Roles = roles;
        }

        public string Name { get; private set; }
        public string[] Roles { get; private set; }

        #region IIdentity Members
        public string AuthenticationType { get { return "Custom authentication"; } }

        public bool IsAuthenticated { get { return !string.IsNullOrEmpty(Name); } }
        #endregion
    }
}