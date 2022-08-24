namespace Fcs.Core
{
    /// <summary>
    /// 数据格式
    /// </summary>
    public enum FcsDataType
    {
        /// <summary>
        /// 未知的
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// 二进制整型
        /// </summary>
        I = 1,
        /// <summary>
        /// 单精度浮点
        /// </summary>
        F = 2,
        /// <summary>
        /// 双精度浮点
        /// </summary>
        D = 3
    }

    public class FcsDataTypeConvert
    {
        #region endian
        public const char Integers = 'I';
        public const char Float = 'F';
        public const char Double = 'D';
        #endregion
        public static FcsDataType ConvertToEnum(char type)
        {
            switch (type)
            {
                case Integers:
                    return FcsDataType.I;
                case Float:
                    return FcsDataType.F;
                case Double:
                    return FcsDataType.D;
                default:
                    return FcsDataType.Unknown;
            }
        }

        public static FcsDataType ConvertToEnum(string type)
        {
            if (string.IsNullOrEmpty(type) || type.Length <= 0) return FcsDataType.Unknown;
            switch (type[0])
            {
                case Integers:
                    return FcsDataType.I;
                case Float:
                    return FcsDataType.F;
                case Double:
                    return FcsDataType.D;
                default:
                    return FcsDataType.Unknown;
            }
        }

        public static char ConvertToChar(FcsDataType fCSDataType)
        {
            switch (fCSDataType)
            {
                case FcsDataType.I:
                    return Integers;
                case FcsDataType.F:
                    return Float;
                case FcsDataType.D:
                    return Double;
                default:
                    return default;
            }
        }
        public static string ConvertToString(FcsDataType fCSDataType)
        {
            switch (fCSDataType)
            {
                case FcsDataType.I:
                    return "I";
                case FcsDataType.F:
                    return "F";
                case FcsDataType.D:
                    return "D";
                default:
                    return default;
            }
        }
    }
}
