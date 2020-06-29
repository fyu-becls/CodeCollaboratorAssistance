using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CollabCommandAPI;

namespace CodeCollaboratorClient.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private class InternalUserData
        {
            public InternalUserData(string username, string hashedPassword, string[] roles)
            {
                Username = username;
                HashedPassword = hashedPassword;
                Roles = roles;
            }
            public string Username
            {
                get;
                private set;
            }

            public string HashedPassword
            {
                get;
                private set;
            }

            public string[] Roles
            {
                get;
                private set;
            }
        }

        private readonly ICollabServerConnection _serverConnection;

        public AuthenticationService(ICollabServerConnection connection)
        {
            _serverConnection = connection;
        }

        public async Task<User> AuthenticateUser(string username, string clearTextPassword)
        {
            //InternalUserData userData = _users.FirstOrDefault(u => u.Username.Equals(username)
            //    && u.HashedPassword.Equals(CalculateHash(clearTextPassword, u.Username)));
            //if (userData == null)
            //    throw new UnauthorizedAccessException("Access denied. Please provide some valid credentials.");

            var isConnect = await _serverConnection.Connect(username, clearTextPassword);
            if (!isConnect)
            {
                throw new UnauthorizedAccessException("Access denied. Please provide some valid credentials.");
            }

            return new User(username, null);
        }

        private string CalculateHash(string clearTextPassword, string salt)
        {
            // Convert the salted password to a byte array
            byte[] saltedHashBytes = Encoding.UTF8.GetBytes(clearTextPassword + salt);
            // Use the hash algorithm to calculate the hash
            HashAlgorithm algorithm = new SHA256Managed();
            byte[] hash = algorithm.ComputeHash(saltedHashBytes);
            // Return the hash as a base64 encoded string to be compared to the stored password
            return Convert.ToBase64String(hash);
        }
    }
}