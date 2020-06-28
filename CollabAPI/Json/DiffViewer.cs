using System;
using System.Collections.Generic;
using System.Text;

namespace CollabAPI
{
    public class DiffViewer
    {
        public enum DiffViewerDisplayMode
        {
            MOST_RECENT,
            ALL,
            LATEST_ACCEPTED,
            FIRST_UPLOADED_LAST_UPLOADED,
            ONLY_CURRENT_BRANCH_CHANGES,
            COMMITS,
        }

        public class FilesCompareInformation
        {
            public int compareType { get; set; }

            public string compareTypeDescription { get; set; }

            public int currentVersionId { get; set; }

            public int previousVersionId { get; set; }

            public string changeType { get; set; }
        }

        public class FilesCompareInformationRequest : User.ClientBuildRequest, Review.IHasReviewId, Review.IHasVersionId
        {
            public int reviewId { get; set; }

            public int versionId { get; set; }
        }

        public class FilesCompareInformationResponse
        {
            public List<DiffViewer.FilesCompareInformation> filesCompareInformationList { get; set; }
        }
    }
}
