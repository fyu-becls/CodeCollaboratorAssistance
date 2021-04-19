namespace Fcs.Core
{
    /// <summary>
    /// 数据字节排序方式
    /// </summary>
    public enum FcsByteEndianness
    {
        /// <summary>
        /// 未知的
        /// </summary>
        Unknown,
        LittleEndian,
        BigEndian
    }
    public class FcsByteEndiannessConvert
    {
        #region endian
        public const string BigEndian = "4,3,2,1";
        public const string LittleEndian = "1,2,3,4";
        #endregion
        public static FcsByteEndianness ConvertToEnum(string endian)
        {
            switch (endian)
            {
                case BigEndian:
                    return FcsByteEndianness.BigEndian;
                case LittleEndian:
                    return FcsByteEndianness.LittleEndian;
                default:
                    return FcsByteEndianness.LittleEndian;
            }
        }

        public static string ConvertToString(FcsByteEndianness fCSByteOrd)
        {
            switch (fCSByteOrd)
            {
                case FcsByteEndianness.LittleEndian:
                    return LittleEndian;
                case FcsByteEndianness.BigEndian:
                    return BigEndian;
                default:
                    return string.Empty;
            }
        }
    }
}
