namespace CodeCollaboratorClient.Authentication
{
    public class AnonymousIdentity : CustomIdentity
    {
        public AnonymousIdentity()
            : base(string.Empty, new string[] { })
        { }
    }
}