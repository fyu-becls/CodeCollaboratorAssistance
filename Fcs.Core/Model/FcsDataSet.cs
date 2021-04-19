using System.Collections.Generic;

namespace Fcs.Core
{
    /// <summary>
    /// 输出对象
    /// </summary>
    public class FcsDataSet
    {
        public Dictionary<string, string> TextSegment { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> AnalysisSegment { get; set; } = new Dictionary<string, string>();
        public IList<FcsSignalMeasurement> Measurements { get; set; }
    }
}
