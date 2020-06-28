// Decompiled with JetBrains decompiler
// Type: SmartBear.Collaborator.Api.DownloadManager
// Assembly: Collaborator.Api, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9c3e7b13c78c6163
// MVID: 82D41E82-1BF6-4105-8B80-342693CB64ED
// Assembly location: C:\Users\fyu\AppData\Local\Microsoft\VisualStudio\16.0_be238a39\Extensions\gzzm4hy2.wuy\Collaborator.Api.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace CollabAPI
{
    public class DownloadManager : IDownloadManager
    {
        private const string DATA_SERVLET_CONTENT_FORMAT = "/data/server?versionid={0}&reviewid={1}";
        private const string DATA_SERVLET_ARCHIVE_FORMAT = "/data/server?archive=true&option={0}&reviewid={1}";
        private const string ARCHIVE_FILE_NAME_FORMAT = "review-{0}-archive";
        private const string LOG_ARCHIVE_DID_NOT_CREATED_FORMAT = "Archive for review with id {0} was not created.";
        private const string ARCHIVE_EXTENSION = ".zip";
        private const string REPORT_EXTENSION = ".pdf";
        private const string CONTENT_DIRECTORY_NAME = "content";
        private string _contentDirectory;
        private IServer _server;
        private ILogger _log;

        public string DEFAULT_CONTENT_DIRECTORY
        {
            get
            {
                return null;
            }
        }

        public DownloadManager(IServer server)
        {
            this._server = server;
            this._log = (ILogger)new Logger(this.GetType());
            this._contentDirectory = this.DEFAULT_CONTENT_DIRECTORY;
        }

        private string GetContentUrl(int reviewId, int versionId)
        {
            return string.Format("/data/server?versionid={0}&reviewid={1}", (object)versionId, (object)reviewId);
        }

        private void CreateContentDirectory()
        {
            string contentDirectory = this.GetContentDirectory();
            if (Directory.Exists(contentDirectory))
                return;
            Directory.CreateDirectory(contentDirectory);
        }

        public string GetContent(int reviewId, Review.Version version)
        {
            int id = version.id;
            string md5 = version.md5;
            string fileName = Path.GetFileName(version.path);
            ISessionInfo sessionInfo = this._server.SessionInfo;
            sessionInfo.ServerURL.TrimEnd('/');
            string contentUrl = this.GetContentUrl(reviewId, id);
            string fullUri = sessionInfo.ServerURL + contentUrl;
            string withoutExtension = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);
            //this.CreateContentDirectory();
            //string str = string.Format("{0}\\{1}_{2}{3}", (object)PathHelper.RemoveTrailingPathDelimiter(this.GetContentDirectory()), (object)withoutExtension, (object)id, (object)extension);
            //if (System.IO.File.Exists(str))
            //    return str;
            return this.Download(fullUri);
            //if (!System.IO.File.Exists(str))
            //  str = string.Empty;
            //return str;
        }

        public string Download(string fullUri)
        {
            string content = null;
            ISessionInfo sessionInfo = this._server.SessionInfo;
            ApiUtils.ToggleAllowUnsafeHeaderParsing(true);
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    string base64String = Convert.ToBase64String(Encoding.ASCII.GetBytes(sessionInfo.UserName + ":"));
                    webClient.Headers.Add(HttpRequestHeader.Authorization, string.Format("Basic {0}", (object)base64String));
                    webClient.Headers.Add("WWW-authenticate-CodeCollabTicket", sessionInfo.UserTicket);
                    ILogger logger = (ILogger)new Logger(typeof(DownloadManager));
                    try
                    {
                        IProgressMonitor progressMonitor = (IProgressMonitor)new ProgressMonitor("c_title_Generic");
                        progressMonitor.WaitMessage = "c_progress_task_DownloadingFile";
                        progressMonitor.Show();
                        try
                        {
                            content = webClient.DownloadString(fullUri);
                        }
                        finally
                        {
                            progressMonitor.Hide();
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogException(ex);
                    }
                }
            }
            finally
            {
                ApiUtils.ToggleAllowUnsafeHeaderParsing(false);
            }
            return content;
        }


        public void DownloadToFile(string fullUri, string targetFileFullName)
        {
            ISessionInfo sessionInfo = this._server.SessionInfo;
            ApiUtils.ToggleAllowUnsafeHeaderParsing(true);
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    string base64String = Convert.ToBase64String(Encoding.ASCII.GetBytes(sessionInfo.UserName + ":"));
                    webClient.Headers.Add(HttpRequestHeader.Authorization, string.Format("Basic {0}", (object)base64String));
                    webClient.Headers.Add("WWW-authenticate-CodeCollabTicket", sessionInfo.UserTicket);
                    ILogger logger = (ILogger)new Logger(typeof(DownloadManager));
                    try
                    {
                        IProgressMonitor progressMonitor = (IProgressMonitor)new ProgressMonitor("c_title_Generic");
                        progressMonitor.WaitMessage = "c_progress_task_DownloadingFile";
                        progressMonitor.Show();
                        try
                        {
                            webClient.DownloadFile(fullUri,targetFileFullName);
                        }
                        finally
                        {
                            progressMonitor.Hide();
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogException(ex);
                    }
                }
            }
            finally
            {
                ApiUtils.ToggleAllowUnsafeHeaderParsing(false);
            }
        }

        public bool CreateArchive(
          List<int> reviewIds,
          string archivePath,
          Client.ArchiveOption archiveOption)
        {
            ISessionInfo sessionInfo = this._server.SessionInfo;
            sessionInfo.ServerURL.TrimEnd('/');
            archivePath = PathHelper.AddTrailingPathDelimiter(archivePath);
            string str1 = archiveOption == Client.ArchiveOption.ONLY_REPORT ? ".pdf" : ".zip";
            foreach (int reviewId in reviewIds)
            {
                string str2 = string.Format("review-{0}-archive", (object)reviewId);
                string archiveUrl = this.GetArchiveUrl(reviewId, archiveOption);
                string fullUri = sessionInfo.ServerURL + archiveUrl;
                string str3 = archivePath + str2 + str1;
                this.DownloadToFile(fullUri, str3);
                if (!System.IO.File.Exists(str3))
                {
                    this._log.LogError(string.Format("Archive for review with id {0} was not created.", (object)reviewId));
                    return false;
                }
            }
            return true;
        }

        private string GetArchiveUrl(int reviewId, Client.ArchiveOption archiveOption)
        {
            return string.Format("/data/server?archive=true&option={0}&reviewid={1}", (object)ApiUtils.GetArchiveOptionValue(archiveOption), (object)reviewId);
        }

        public void SetContentDirectory(string contentDirectory)
        {
            if (string.IsNullOrEmpty(contentDirectory))
                this.SetContentDirectoryToDefault();
            else
                this._contentDirectory = contentDirectory;
        }

        private void SetContentDirectoryToDefault()
        {
            this._contentDirectory = this.DEFAULT_CONTENT_DIRECTORY;
        }

        public string GetContentDirectory()
        {
            return this._contentDirectory;
        }
    }
}
