using System;
using System.Collections.Generic;
using System.Text;

namespace CollabAPI
{
    public class DiffViewerServiceAdapter : BaseServiceAdapter, IDiffViewerService, IBaseService
    {
        public DiffViewerServiceAdapter(IServer server)
          : base(server)
        {
        }

        public List<DiffViewer.FilesCompareInformation> GetFilesCompareInformation(
          int reviewId,
          int versionId)
        {
            DiffViewerService.getFilesCompareInformation compareInformation = new DiffViewerService.getFilesCompareInformation();
            compareInformation.reviewId = reviewId;
            compareInformation.versionId = versionId;
            compareInformation.clientBuild = this.SessionInfo.ClientBuild;
            JsonResult jsonResult = this.SendRequest((object)compareInformation, true);
            return jsonResult != null ? jsonResult.GetResponse<DiffViewer.FilesCompareInformationResponse>().filesCompareInformationList : new List<DiffViewer.FilesCompareInformation>();
        }
    }
}
