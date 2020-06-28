using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabAPI
{
    public class CodeCollabAPI
    {
        public string serverURL
        {
            get;
            set;
        }
        public string userName
        {
            get;
            set;
        }
        public string userTicket
        {
            get;
            private set;
        }

        public CodeCollabAPI(string serverURL, string userName, string loginTicket)
        {
            if (String.IsNullOrWhiteSpace(serverURL))
                throw new ArgumentException("serverURL cannot be empty");
            else
                this.serverURL = serverURL;

            if (String.IsNullOrWhiteSpace(userName))
                throw new ArgumentException("userName cannot be empty");
            else
                this.userName = userName;

            this.userTicket = loginTicket;
        }

        public CodeCollabAPI(string serverURL, string userName) : this(serverURL, userName, "") { }

        // Returns API Meta Data command that tells the Collaborator server about this client
        private JsonCommand getMetaDataCommand()
        {
            return new JsonCommand(new SessionService.setMetadata()
            {
                expectedServerVersion = "9.0.9001",
                clientName = "Rick's Collab API client"
            });
        }

        private JsonResult executeSingleCommand(Object request, bool sendAuthTicket = true)
        {
            if (request == null && !sendAuthTicket)
                throw new ArgumentException("Invalid arguments: No request object, and no authentication request.");

            if (String.IsNullOrWhiteSpace(serverURL))
                throw new ArgumentException("serverURL cannot be empty");

            if (String.IsNullOrWhiteSpace(userName))
                throw new ArgumentException("userName cannot be empty");

            // If we're sending an auth ticket, then make sure we have a ticket
            if (sendAuthTicket && !hasTicket())
                throw new ArgumentException("You don't have a ticket! Call getLoginTicket() first.");

            // The lists that store our request and response batches
            List<JsonCommand> requestList = new List<JsonCommand>();
            List<JsonResult> responseList;

            // Add the meta data command
            requestList.Add(getMetaDataCommand());

            if (sendAuthTicket)
            {
                // Add the API authenticate command
                requestList.Add(new JsonCommand(new SessionService.authenticate()
                {
                    login = this.userName,
                    ticket = this.userTicket
                }));
            }

            // Add the callers API command
            if (request != null)
                requestList.Add(new JsonCommand(request));

            try
            {
                // Send the request, it's our responsibility to trap errors here, they should be friendly
                responseList = CollabServerRequest.Execute(this.serverURL, requestList);
            }
            catch (Exception ex)
            {
                // Tell the user about the low level communication error. e.g. connection refused
                throw new Exception("Error sending request " + ex.Message, ex);
            }

            if (responseList != null && responseList.Count() > 0) // Execute block if we didn't error above
            {
                List<JsonResult>.Enumerator responses = responseList.GetEnumerator();
                responses.MoveNext(); // skip the meta data response

                if (sendAuthTicket)
                {
                    responses.MoveNext();
                    if (responses.Current.IsError())
                    { // authentication error
                        throw new Exception(responses.Current.GetErrorString());
                    }
                }

                if (request != null) // If this is null, this is just an auth check
                    responses.MoveNext();

                return responses.Current;
            }

            // Should never get here.
            throw new Exception("API failed to return a result");
        }

        // Gets a login ticket from the server, throws an exception on communication exception,
        // Returns a JsonResult object when the API returns, whether auth fails or not.
        // If JsonResult.isError() is false, then the login succeeded.
        public JsonResult getLoginTicket(string userPassword)
        {
            // Create the API get login ticket request object
            SessionService.getLoginTicket request = new SessionService.getLoginTicket();
            // Populate the request fields with our form data
            request.login = this.userName;
            request.password = userPassword;

            JsonResult response;

            try
            {
                // Send the request, it's our responsibility to trap errors here, they should be friendly
                response = executeSingleCommand(request, false);
            }
            catch (Exception ex)
            {
                // Tell the user about the low level communication error. e.g. connection refused
                throw new Exception(ex.Message, ex);
            }

            // If there's no error, then save the ticket
            if (!response.IsError())
                this.userTicket = response.GetResponse<SessionService.getLoginTicketResponse>().loginTicket;

            // Hand the whole response to the caller. If there's an error, they can do what they want
            return response;
        }
        // True if ticket is still valid, false otherwise
        public bool checkTicketValidity()
        {
            try
            {
                executeSingleCommand(null, true); // Just try to auth
            }
            catch
            {
                return false; // No bueno
            }

            // If we got there, then we're good
            return true;
        }
        // True if we have a ticket stored locally, it may or may not still be valid
        public bool hasTicket()
        {
            return !String.IsNullOrWhiteSpace(this.userTicket);
        }
        // Clear our locally stored login ticket
        public void clearLoginTicket()
        {
            this.userTicket = "";
        }
        // Get the remote server version string
        public string getServerVersion()
        {
            ServerInfoService.getVersion request = new ServerInfoService.getVersion();
            JsonResult response;
            try
            {
                response = executeSingleCommand(request, false);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

            if (!response.IsError())
                return response.GetResponse<ServerInfoService.getVersionResponse>().version;
            else
                return null;
        }
        // Get the remote server build number
        public int getServerBuild()
        {
            ServerInfoService.getServerBuild request = new ServerInfoService.getServerBuild();
            JsonResult response;
            try
            {
                response = executeSingleCommand(request, false);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

            if (!response.IsError())
                return response.GetResponse<ServerInfoService.getServerBuildResponse>().serverBuild;
            else
                return -1;
        }
        // Get the minimum client build number for XML RPC clients
        public int getMinimumJavaClientBuild()
        {
            ServerInfoService.getMinimumJavaClientBuild request = new ServerInfoService.getMinimumJavaClientBuild();
            JsonResult response;
            try
            {
                response = executeSingleCommand(request, false);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

            if (!response.IsError())
                return response.GetResponse<ServerInfoService.getMinimumJavaClientBuildResponse>().minimumJavaClientBuild;
            else
                return -1;
        }
        // Create a review with very few parameters
        public JsonResult createReview(string title = "")
        {
            ReviewService.createReview request = new ReviewService.createReview();
            JsonResult response;

            // Set the creater to the logged in user
            request.creator = this.userName;
            request.title = title;

            try
            {
                response = executeSingleCommand(request);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

            return response;
        }
        // Get review suggestions for the current user
        public JsonResult getSuggestedReviews(User.SuggestionType suggestionType = User.SuggestionType.UPLOAD,
            string changelistId = "",
            int maxResults = 100)
        {
            UserService.getSuggestedReviews request = new UserService.getSuggestedReviews();
            JsonResult response;

            request.changelistId = changelistId;
            request.suggestionType = suggestionType;
            request.maxResults = maxResults;

            try
            {
                response = executeSingleCommand(request);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

            return response;
        }

        // Get review suggestions for the current user
        public JsonResult getReviewSummary(int id)
        {
            ReviewService.getReviewSummary request = new ReviewService.getReviewSummary();
            JsonResult response;

            request.reviewId=id;
            request.active = true;
            request.clientBuild = 12500;
            request.clientGuid = "70fddf50-5742-47f2-8f1d-d951236a9d5a";

            try
            {
                response = executeSingleCommand(request);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

            return response;
        }
    }
}
