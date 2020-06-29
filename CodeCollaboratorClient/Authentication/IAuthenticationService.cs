using System.Threading.Tasks;

namespace CodeCollaboratorClient.Authentication
{
    public interface IAuthenticationService
    {
        Task<User> AuthenticateUser(string username, string password);
    }
}