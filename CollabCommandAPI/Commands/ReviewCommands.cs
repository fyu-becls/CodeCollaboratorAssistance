using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace CollabCommandAPI.Commands
{
    public class ReviewCommands
    {
        private string ReviewSummaryXslPath
        {
            get { return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "review_summary.xsl"); }
        }
        public string Create(string title, List<ReviewCustomField> customFields, string deadline, List<ReviewCustomField> participantCustomFields, string restrictAccess)
        {
            return $"admin review create --title '{title}' --deadline '{deadline}' --restrict-access '{restrictAccess}'";
        }

        public string GetReviewsList(string keyword, bool disablePrettyPrint = false)
        {
            var tempPath = Path.GetTempFileName();
            return $"admin find review '{keyword}'" + (disablePrettyPrint ? " --disable-pretty-print" : string.Empty);
        }
    }
}
