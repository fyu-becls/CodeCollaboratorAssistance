namespace Fcs.Core
{
    /// <summary>
    /// 放大类型参数 PnE
    /// v=10^（PowerNumber * xc /（PnR））* ZeroValue
    /// </summary>
    public struct FcsAmplificationType
    {
        /// <summary>
        /// 10的次方数
        /// </summary>
        public double PowerNumber { get; set; }
        /// <summary>
        /// 0对应的转换值
        /// </summary>
        public double ZeroValue { get; set; }

        public FcsAmplificationType(double powerNumber, double zeroValue)
        {
            this.PowerNumber = powerNumber;
            this.ZeroValue = zeroValue;
        }

        public FcsAmplificationType(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                this.PowerNumber = this.ZeroValue = 0d;
                return;
            }
            var spirts = str.Split(',');
            if (spirts.Length != 2)
            {
                this.PowerNumber = this.ZeroValue = 0d;
                return;
            }
            if (double.TryParse(spirts[0], out var powerNumber)) this.PowerNumber = powerNumber;
            else this.PowerNumber = 0d;
            if (double.TryParse(spirts[1], out var zeroValue)) this.ZeroValue = zeroValue;
            else this.ZeroValue = 0d;
        }

        public override string ToString()
        {
            return string.Concat(PowerNumber, ",", ZeroValue);
        }

    }
}
