using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Fcs.Core
{
    public static class StreamExtensions
    {
        public static void WriteAll(this Stream stream, params byte[] bytes)
        {
            stream.Write(bytes, 0, bytes.Length);
        }

        public static void WriteAll(this Stream stream, IEnumerable<byte> bytes)
        {
            stream.Write(bytes.ToArray(), 0, bytes.Count());
        }
    }
}
