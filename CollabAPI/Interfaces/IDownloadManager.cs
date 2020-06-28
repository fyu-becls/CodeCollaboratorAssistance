using System;
using System.Collections.Generic;
using System.Text;

namespace CollabAPI
{
    public interface IDownloadManager
    {
        string GetContentDirectory();

        void SetContentDirectory(string contentDirectory);

        string GetContent(int reviewId, Review.Version version);

        string Download(string fullUri);

        bool CreateArchive(List<int> reviewIds, string archivePath, Client.ArchiveOption archiveOption);
    }
}
