using System;
using System.Collections.Generic;
using System.Text;

namespace CollabAPI
{
    public interface IDiffViewerService : IBaseService
    {
        List<DiffViewer.FilesCompareInformation> GetFilesCompareInformation(
          int reviewId,
          int versionId);
    }
}
