namespace Fcs.Core
{
    /// <summary>
    /// 存储模式
    /// </summary>
    public enum FcsDataMode
    {
        Unknown,
        /// <summary>
        /// 列表模式
        /// </summary>
        L
    }

    public class ModeConvert
    {
        #region endian
        public const char List = 'L';
        #endregion
        public static FcsDataMode ConvertToEnum(char mode)
        {
            switch (mode)
            {
                case List:
                    return FcsDataMode.L;
                default:
                    return FcsDataMode.Unknown;
            }
        }

        public static FcsDataMode ConvertToEnum(string mode)
        {
            if (string.IsNullOrEmpty(mode) || mode.Length <= 0) return FcsDataMode.Unknown;
            switch (mode[0])
            {
                case List:
                    return FcsDataMode.L;
                default:
                    return FcsDataMode.Unknown;
            }
        }

        public static char ConvertToString(FcsDataMode fCSMode)
        {
            switch (fCSMode)
            {
                case FcsDataMode.L:
                    return List;
                default:
                    return default;
            }
        }
    }
}
