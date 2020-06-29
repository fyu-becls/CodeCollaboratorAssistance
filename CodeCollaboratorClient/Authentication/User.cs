namespace CodeCollaboratorClient.Authentication
{
    public class User
    {
        public User(string username, string[] roles)
        {
            Username = username;
            Roles = roles;
        }
        public string Username
        {
            get;
            set;
        }

        public string[] Roles
        {
            get;
            set;
        }
    }
}