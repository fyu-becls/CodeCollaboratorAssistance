namespace Fcs.Core
{
    /// <summary>
    /// 推荐的可视化比例 PnD
    /// </summary>
    public struct FcsSuggestedVisualizationScale
    {
        public FcsSuggestedVisualizationScaleType Type { get; set; }
        public double F1 { get; set; }
        public double F2 { get; set; }

        public FcsSuggestedVisualizationScale(FcsSuggestedVisualizationScaleType type, double f1, double f2)
        {
            this.Type = type;
            this.F1 = f1;
            this.F2 = f2;
        }

        public FcsSuggestedVisualizationScale(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                this.Type = FcsSuggestedVisualizationScaleType.Unknown;
                this.F1 = this.F2 = 0d;
                return;
            }
            var spirts = str.Split(',');
            if (spirts.Length != 3)
            {
                this.Type = FcsSuggestedVisualizationScaleType.Unknown;
                this.F1 = this.F2 = 0d;
                return;
            }
            switch (spirts[0].ToUpper())
            {
                case "LINEAR":
                    this.Type = FcsSuggestedVisualizationScaleType.Linear;
                    break;
                case "LOGARITHMIC":
                    this.Type = FcsSuggestedVisualizationScaleType.Logarithmic;
                    break;
                default:
                    this.Type = FcsSuggestedVisualizationScaleType.Unknown;
                    break;
            }
            if (double.TryParse(spirts[1], out var f1)) this.F1 = f1;
            else this.F1 = 0d;
            if (double.TryParse(spirts[2], out var f2)) this.F2 = f2;
            else this.F2 = 0d;
        }

        public override string ToString()
        {
            string typestring;
            switch (Type)
            {
                case FcsSuggestedVisualizationScaleType.Linear:
                    typestring = "LINEAR";
                    break;
                case FcsSuggestedVisualizationScaleType.Logarithmic:
                    typestring = "LOGARITHMIC";
                    break;
                default:
                    return string.Empty;
            }
            return string.Concat(typestring, ",", F1, ",", F2);
        }

    }

    public enum FcsSuggestedVisualizationScaleType
    {
        Unknown = 0,
        Linear = 1,
        Logarithmic = 2
    }
}
