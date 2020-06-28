using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using Newtonsoft.Json;
 
namespace CollabAPI
{
    public static class CollabServerRequest
    {
        // Uploads a zip file using auth data passed using a SessionService.authenticate object
        public static void UploadZipFile(string serverURL, string localFilePath, SessionService.authenticate authData)
        {
            byte[] response;

            // We're going to need these
            if (String.IsNullOrWhiteSpace(serverURL))
                throw new Exception("Server URL cannot be empty!");
            if (String.IsNullOrWhiteSpace(localFilePath))
                throw new Exception("Local Zip File path cannot be empty");

            // Append the content upload service URL
            serverURL = serverURL.TrimEnd('/') + @"/contentupload";

            // Get a new HTTP client
            using (WebClient client = new WebClient())
            {
               // TODO: set request headers
                // send the request synchronously, and block until it returns
                try
                {
                    // Add auth headers
                    // Create credentials, base64 encode of username:password
                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(authData.login + ":"));
                    // Inject this string as the Authorization header
                    client.Headers.Add(HttpRequestHeader.Authorization, string.Format("Basic {0}", credentials));
                    client.Headers.Add("WWW-authenticate-CodeCollabTicket", authData.ticket);
                    try
                    {
                        response = client.UploadFile(serverURL, localFilePath);
                    }
                    catch (Exception ex)
                    {
                        // HACK: Silly Servlet is not returning linefeeds the way that Microsoft likes them.
                        if (ex.Message.StartsWith("The server committed a protocol violation."))
                        {
                            // Ignore this. Actual error message is:
                            // The server committed a protocol violation. Section=ResponseHeader Detail=CR must be followed by LF
                            // Luckily, we've already uploaded the file, and don't care much about the response.
                            // We need to fix this in the servlet, or more C# client classess will complain.

                            // Update: The application calling this API should add this to the app.config:
                            // <system.net>
                            //  <settings>
                            //      <httpWebRequest useUnsafeHeaderParsing="true" />
                            //  </settings>
                            // </system.net>

                            // We'll leave this block here, just in case.
                        }
                        else
                            throw ex;
                    }                    
                }
                catch (Exception ex)
                {
                    throw new Exception("Connection Error: " + ex.Message);
                }
            }
        }

        // Executes a batch of requests
        public static List<JsonResult> Execute(string serverURL, List<JsonCommand> commands)
        {
            string response = "";
            string data = "";

            // We're going to need these
            if (String.IsNullOrWhiteSpace(serverURL))
                throw new Exception("Server URL cannot be empty!");
            if (commands == null || commands.Count < 1)
                throw new Exception("No commands were specified!");

            // Append the JSON service URL
            serverURL = serverURL.TrimEnd('/') + @"/services/json/v1";

            // Get a new HTTP client
            using (WebClient client = new WebClient())
            {
                // convert the request object into a Json string
                data = JsonConvert.SerializeObject(commands);
                //MessageBox.Show(data);

                // TODO: set request headers

                // send the request synchronously, and block until it returns
                try
                {
                    response = client.UploadString(serverURL, data);
                }
                catch (Exception ex)
                {
                    throw new Exception("Connection Error: " + ex.Message);
                }
            }

            // Deserialize the list of JsonResult objects
            List<JsonResult> resp = JsonConvert.DeserializeObject<List<JsonResult>>(response);

            // Send the data back to the caller
            return resp;
        }
    }
}
