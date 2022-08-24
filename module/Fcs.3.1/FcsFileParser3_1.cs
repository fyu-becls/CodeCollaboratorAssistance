
using Fcs.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fcs.File
{
    public class FcsFileParser3_1 : FcsFileBaseParser
    {
        #region 解析
        /// <summary>
        /// 从文本段填充数据集参数
        /// </summary>
        /// <param name="keyValues">文本段字典</param>
        /// <param name="parameter">需要完善的参数</param>
        protected override void ParseParameterFromTextSegment(Dictionary<string, string> keyValues, FcsDataSetParameter parameter)
        {
            base.ParseParameterFromTextSegment(keyValues, parameter);
            if (keyValues.ContainsKey(FcsKewywords.ModeKey) && ModeConvert.ConvertToEnum(keyValues[FcsKewywords.ModeKey]) != FcsDataMode.L) throw new Exception("Can't analyse,mode must be L");
        }

        /// <summary>
        /// 解析数据集的通道参数
        /// </summary>
        /// <param name="textSegment">文本段key-value集合</param>
        /// <param name="par">通道数量</param>
        /// <param name="defaultDataType">默认数据类型</param>
        protected override IList<FcsSignalMeasurement> ParseSignalMeasurements(Dictionary<string, string> textSegment, uint par, FcsDataType defaultDataType)
        {
            var measurements = base.ParseSignalMeasurements(textSegment, par, defaultDataType);
            foreach (var item in measurements) item.PnDATATYPE = defaultDataType;
            return measurements;
        }
        #endregion

        #region 保存
        /// <summary>
        /// 创建一个空的没有数据的head段,只有版本号
        /// </summary>
        /// <returns></returns>
        protected override byte[] CreateEmptyHeadBytes()
        {
            return new byte[] { 0x46, 0x43, 0x53, 0x33, 0x2e, 0x31, 0x20, 0x20, 0x20, 0x20
                    , 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x30
                    , 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x30
                    , 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x30
                    , 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x30
                    , 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x30
                    , 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x30};
        }
        /// <summary>
        /// 重置文本段中的一些参数，根据通道参数
        /// </summary>
        /// <param name="measurements">通道集合</param>
        /// <param name="textSegment">文本段字典</param>
        protected override void ResetTextSegment(Dictionary<string, string> textSegment, IList<FcsSignalMeasurement> measurements)
        {
            if (textSegment == null) throw new Exception("Dictionary can't be null");
            if (measurements == null || measurements.Count <= 0 || measurements.FirstOrDefault(p => p.Values == null || p.Values.Count <= 0) != null) throw new Exception("Measurement array and every measurement's values can't be null or empty");
            var counts = measurements.Select(p => p.Values.Count).Distinct().ToArray();
            if (counts.Length > 1) throw new Exception("Error,every measurement's values count mast same");
            else textSegment[FcsKewywords.TOTKey] = counts[0].ToString();//tot数量
            textSegment[FcsKewywords.ByteOrdKey] = BitConverter.IsLittleEndian ? FcsByteEndiannessConvert.LittleEndian : FcsByteEndiannessConvert.BigEndian;//windows系统默认
            textSegment[FcsKewywords.ModeKey] = "L";//保留字段，3.2版本已经取消，都是List类型
            textSegment[FcsKewywords.PARKey] = measurements.Count.ToString();//通道数量
            var datatypes = measurements.Select(p => p.PnDATATYPE).Distinct().ToArray();
            FcsDataType defaultDataType = FcsDataType.Unknown;//取出默认数据类型
            if (datatypes.Contains(FcsDataType.D)) defaultDataType = FcsDataType.D;
            else if (datatypes.Contains(FcsDataType.F)) defaultDataType = FcsDataType.F;
            else defaultDataType = FcsDataType.I;
            textSegment[FcsKewywords.DataTypeKey] = FcsDataTypeConvert.ConvertToString(defaultDataType);
            for (int i = 1; i <= measurements.Count; i++)
            {
                var measurement = measurements[i - 1];
                if (defaultDataType != FcsDataType.I)//如果是浮点型
                {
                    textSegment[string.Format(FcsKewywords.PnBKey, i)] = defaultDataType == FcsDataType.D ? "64" : "32";
                    textSegment[string.Format(FcsKewywords.PnEKey, i)] = "0,0";
                }
                else//如果是整数型
                {
                    if (measurement.Values[0] is ulong ul) textSegment[string.Format(FcsKewywords.PnBKey, i)] = "64";
                    else if (measurement.Values[0] is uint ui) textSegment[string.Format(FcsKewywords.PnBKey, i)] = "32";
                    else if (measurement.Values[0] is ushort us) textSegment[string.Format(FcsKewywords.PnBKey, i)] = "16";
                    else if (measurement.Values[0] is byte ub) textSegment[string.Format(FcsKewywords.PnBKey, i)] = "8";
                    else throw new Exception("Measurement value's data type not supported");
                    textSegment[string.Format(FcsKewywords.PnEKey, i)] = measurement.PnE.ToString();
                }
                string pngkey = string.Format(FcsKewywords.PnGKey, i);
                if (!double.IsNaN(measurement.PnG) && measurement.PnG > 0) textSegment[pngkey] = measurement.PnG.ToString();
                else textSegment[pngkey] = "1";

                string pndatatypekey = string.Format(FcsKewywords.PnDataTypeKey, i);
                if (defaultDataType != measurement.PnDATATYPE) textSegment[pndatatypekey] = FcsDataTypeConvert.ConvertToString(measurement.PnDATATYPE);//PnDataType不是默认值
                else if (textSegment.ContainsKey(pndatatypekey)) textSegment.Remove(pndatatypekey);//PnDataType是默认值
                if (string.IsNullOrEmpty(measurement.PnN)) throw new Exception("Measurment name can't be null or empty");
                textSegment[string.Format(FcsKewywords.PnNKey, i)] = measurement.PnN;

                textSegment[string.Format(FcsKewywords.PnRKey, i)] = measurement.PnR.ToString();

                string pndkey = string.Format(FcsKewywords.PnDKey, i);
                if (measurement.PnD.Type != FcsSuggestedVisualizationScaleType.Unknown) textSegment[pndkey] = measurement.PnD.ToString();
                else if (textSegment.ContainsKey(pndkey)) textSegment.Remove(pndkey);

                string pnfkey = string.Format(FcsKewywords.PnFKey, i);
                if (!string.IsNullOrEmpty(measurement.PnF)) textSegment[pnfkey] = measurement.PnF;
                else if (textSegment.ContainsKey(pnfkey)) textSegment.Remove(pnfkey);

                string pnlkey = string.Format(FcsKewywords.PnLKey, i);
                if (!string.IsNullOrEmpty(measurement.PnL)) textSegment[pnlkey] = measurement.PnL;
                else if (textSegment.ContainsKey(pnlkey)) textSegment.Remove(pnlkey);

                string pnokey = string.Format(FcsKewywords.PnOKey, i);
                if (measurement.PnO != 0) textSegment[pnokey] = measurement.PnO.ToString();
                else if (textSegment.ContainsKey(pnokey)) textSegment.Remove(pnokey);

                string pnskey = string.Format(FcsKewywords.PnSKey, i);
                if (!string.IsNullOrEmpty(measurement.PnS)) textSegment[pnskey] = measurement.PnS;
                else if (textSegment.ContainsKey(pnskey)) textSegment.Remove(pnskey);

                string pntkey = string.Format(FcsKewywords.PnTKey, i);
                if (!string.IsNullOrEmpty(measurement.PnT)) textSegment[pntkey] = measurement.PnT;
                else if (textSegment.ContainsKey(pntkey)) textSegment.Remove(pntkey);

                string pnvkey = string.Format(FcsKewywords.PnVKey, i);
                if (!double.IsNaN(measurement.PnV) && measurement.PnV > 0) textSegment[pnvkey] = measurement.PnV.ToString();
                else if (textSegment.ContainsKey(pnvkey)) textSegment.Remove(pnvkey);
            }
        }
        #endregion
    }
}
