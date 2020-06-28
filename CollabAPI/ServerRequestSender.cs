using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CollabAPI
{
    internal static class ServerRequestSender
    {
        private static string ERROR_CERTIFICATE_MESSAGE = "You are connecting to Collaborator server '{0}'." + Environment.NewLine + "The server's certificate is invalid." + Environment.NewLine + "Do you want to use this certificate anyway?";
        private static string TRUSTED_CERTIFICATE_NAME = "trustedcertificate";
        private static ILogger _log = (ILogger)new Logger(typeof(ServerRequestSender));
        private static HashSet<string> incorrectCertificatesSet = new HashSet<string>();
        private static Dictionary<SslPolicyErrors, string> _sertificateErrors = new Dictionary<SslPolicyErrors, string>();

        private static string ServerUrl { get; set; }

        public static List<JsonResult> Execute(
          ISessionInfo sessionInfo,
          List<JsonCommand> commands)
        {
            ServerRequestSender.ServerUrl = sessionInfo.ServerURL;
            string requestUri = ServerRequestSender.ServerUrl.TrimEnd('/') + "/services/json/v1";
            //ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ServerRequestSender.ValidateRemoteCertificate);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            JsonConvert.DefaultSettings = (Func<JsonSerializerSettings>)(() => new JsonSerializerSettings()
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            });
            StringContent stringContent = new StringContent(JsonConvert.SerializeObject((object)commands), Encoding.UTF8);
            stringContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "json"
            };
            Task<HttpResponseMessage> task1 = sessionInfo.Client.PostAsync(requestUri, (HttpContent)stringContent);
            task1.Wait();
            HttpResponseMessage result = task1.Result;
            result.EnsureSuccessStatusCode();
            string str = string.Empty;
            if (result != null)
            {
                Task<string> task2 = result.Content.ReadAsStringAsync();
                task2.Wait();
                str = task2.Result;
            }
            return JsonConvert.DeserializeObject<List<JsonResult>>(str);
        }

        private static bool SkipCertificate(X509Certificate certificate, string localCertificatePath)
        {
            try
            {
                return System.IO.File.Exists(localCertificatePath) && new X509Certificate(localCertificatePath).Equals(certificate);
            }
            catch (Exception ex)
            {
                ServerRequestSender._log.LogError(ex.Message == null ? "" : ex.Message);
            }
            return false;
        }


        public static async Task<List<JsonResult>> ExecuteAsync(
          ISessionInfo sessionInfo,
          List<JsonCommand> commands,
          string fileToAttach,
          IProgressMonitor progressMonitor)
        {
            string serverUrl = sessionInfo.ServerURL;
            HttpResponseMessage httpResponseMessage = (HttpResponseMessage)null;
            char[] chArray = new char[1] { '/' };
            string requestUri = serverUrl.TrimEnd(chArray) + "/services/json/v1";
            HttpClient client = sessionInfo.Client;
            JsonConvert.DefaultSettings = (Func<JsonSerializerSettings>)(() => new JsonSerializerSettings()
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            });
            StringContent stringContent = new StringContent(JsonConvert.SerializeObject((object)commands), Encoding.UTF8);
            stringContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "json"
            };
            HttpContent content;
            if (!string.IsNullOrEmpty(fileToAttach) && System.IO.File.Exists(fileToAttach))
            {
                StreamContent streamContent = new StreamContent((Stream)System.IO.File.Open(fileToAttach, FileMode.Open));
                streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = Path.GetFileName(fileToAttach),
                    FileName = Path.GetFileName(fileToAttach)
                };
                streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream")
                {
                    CharSet = "ISO-8859-1"
                };
                streamContent.Headers.Add("Content-Transfer-Encoding", "binary");
                MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();
                multipartFormDataContent.Add((HttpContent)stringContent);
                multipartFormDataContent.Add((HttpContent)streamContent);
                content = (HttpContent)multipartFormDataContent;
            }
            else
                content = (HttpContent)stringContent;
            if (content != null)
            {
                try
                {
                    if (progressMonitor != null)
                        httpResponseMessage = await client.PostAsync(requestUri, content, progressMonitor.CancellationTokenSource.Token).ConfigureAwait(false);
                    else
                        httpResponseMessage = await client.PostAsync(requestUri, content).ConfigureAwait(false);
                    httpResponseMessage.EnsureSuccessStatusCode();
                }
                catch (TaskCanceledException ex)
                {
                    ServerRequestSender._log.LogDebugMessage("Files upload canceled");
                    return (List<JsonResult>)null;
                }
            }
            string str = string.Empty;
            if (httpResponseMessage != null)
                str = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<JsonResult>>(str);
        }
    }
}
