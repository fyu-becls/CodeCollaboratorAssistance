using System.IO;

namespace Fcs.Core
{
    interface IFcsFileParser
    {
        FcsDataSet ReadDataSet(Stream stream, out long nextData, long fileBeginOffset = 0);
    }
}
