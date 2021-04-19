using Microsoft.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Fcs.Core
{
    public abstract class FcsFileBaseParser : IFcsFileParser
    {
        /// <summary>
        /// Read bytes from a stream.
        /// </summary>
        /// <param name="stream">Readable stream.</param>
        /// <param name="dataSetOffset">The starting point of the data set, relative to the starting point of the stream.</param>
        /// <param name="begin">The starting point of the data to be read, relative to the starting point of the data set.</param>
        /// <param name="end">The end point of the data to be read, relative to the start point of the data set</param>
        /// <returns></returns>
        protected virtual byte[] ReadBytes(Stream stream, long dataSetOffset, long begin, long end)
        {
            if (stream.Length < dataSetOffset + end)
            {
                throw new Exception("Stream length is not enough.");
            }

            var length = end - begin + 1;
            var bytes = new byte[length]; // TODO: Check if the length exceed 2G.
            stream.Seek(dataSetOffset + begin, SeekOrigin.Begin);
            if (length > int.MaxValue)
            {
                var tempBytesLength = length;
                while (tempBytesLength > 0)
                {
                    var tempBytes = new byte[(tempBytesLength > int.MaxValue ? int.MaxValue : Convert.ToInt32(tempBytesLength))];
                    if (stream.Read(tempBytes, 0, tempBytes.Length) != tempBytes.Length)
                    {
                        throw new Exception("Stream read failed");
                    }

                    tempBytes.CopyTo(bytes, length - tempBytesLength);
                    tempBytesLength -= tempBytes.Length;
                }
            }
            else if (stream.Read(bytes, 0, Convert.ToInt32(length)) != length)
            {
                throw new Exception("Stream read failed");
            }

            return bytes;
        }

        /// <summary>
        /// Read key-value data.
        /// </summary>
        /// <param name="bytes">Byte array, the first byte is delimiter.</param>
        /// <param name="keyValues">The result dictionary.</param>
        /// <param name="delimiterByte">delimiter</param>
        /// <param name="encoding">Encoding.</param>
        protected virtual void ReadKeyValues(byte[] bytes, Dictionary<string, string> keyValues, byte delimiterByte, Encoding encoding)
        {
            if (bytes == null || bytes.Length <= 0)
            {
                throw new Exception("Data is null or empty");
            }

            if (keyValues == null)
            {
                throw new Exception("Segment is null");
            }

            var key = new List<byte>();
            var value = new List<byte>();
            var keyByte = false;
            byte lastByte = 0xff;//The last byte of the previous key-value buffer.
            for (long i = 0; i < bytes.LongLength; i++)
            {
                var b = bytes[i];
                if (i == 0L)
                {
                    if (b != delimiterByte) throw new Exception("Delimiter byte error");
                    keyByte = true;
                    continue;
                }
                if (b == delimiterByte)
                {
                    keyByte = !keyByte;
                    if (lastByte == delimiterByte)//double delimiter
                    {
                        if (keyByte) key.Add(delimiterByte);
                        else value.Add(delimiterByte);
                        lastByte = 0xff;
                        continue;
                    }
                    lastByte = b;
                    continue;
                }
                else if (lastByte == delimiterByte && keyByte)
                {
                    keyValues.Add(Encoding.ASCII.GetString(key.ToArray()).ToUpper(), encoding.GetString(value.ToArray()));
                    key.Clear();
                    value.Clear();
                }
                lastByte = b;
                if (keyByte) key.Add(b);
                else value.Add(b);
            }
            keyValues.Add(Encoding.ASCII.GetString(key.ToArray()).ToUpper(), encoding.GetString(value.ToArray()));
        }

        /// <summary>
        /// Read key-value data.
        /// </summary>
        /// <param name="bytes">Byte array, the first byte is delimiter.</param>
        /// <param name="keyValues">The result dictionary.</param>
        /// <param name="delimiterByte">delimiter</param>
        protected virtual void ReadUtf8KeyValues(byte[] bytes, Dictionary<string, string> keyValues, byte delimiterByte)
        {
            ReadKeyValues(bytes, keyValues, delimiterByte, Encoding.UTF8);
        }

        /// <summary>
        /// Read header segment.
        /// </summary>
        /// <param name="stream">File stream.</param>
        /// <param name="fileBeginOffset">The start point of data set.</param>
        /// <param name="parameter">The parameters.</param>
        protected virtual void ReadHeadSegment(Stream stream, long fileBeginOffset, FcsDataSetParameter parameter)
        {
            var headerBytes = new byte[58];
            stream.Seek(fileBeginOffset, SeekOrigin.Begin);
            if (stream.Read(headerBytes, 0, 58) == 58)
            {
                var headerString = Encoding.ASCII.GetString(headerBytes);
                parameter.Version = headerString.Substring(0, 6).Trim();
                parameter.TextBegin = Convert.ToInt64(headerString.Substring(10, 8));
                parameter.TextEnd = Convert.ToInt64(headerString.Substring(18, 8));
                parameter.DataBegin = Convert.ToInt64(headerString.Substring(26, 8));
                parameter.DataEnd = Convert.ToInt64(headerString.Substring(34, 8));
                parameter.AnalysisBegin = Convert.ToInt64(headerString.Substring(42, 8));
                parameter.AnalysisEnd = Convert.ToInt64(headerString.Substring(50, 8));
                if (stream.Length >= fileBeginOffset + parameter.TextBegin + 1)
                {
                    var delimiterByte = ReadBytes(stream, fileBeginOffset, parameter.TextBegin, parameter.TextBegin);
                    if (delimiterByte != null && delimiterByte.Length == 1) parameter.DelimiterByte = delimiterByte[0];
                }
                else
                {
                    throw new Exception("Read Delimiter byte failed, stream length is not enough");
                }
            }
            else
            {
                throw new Exception("Read head failed, stream length is not enough");
            }
        }

        /// <summary>
        /// Parse parameters from text segment.
        /// </summary>
        /// <param name="keyValues">The dictionary of text segment.</param>
        /// <param name="parameter">The parameters</param>
        protected virtual void ParseParameterFromTextSegment(Dictionary<string, string> keyValues, FcsDataSetParameter parameter)
        {
            if (keyValues == null)
            {
                throw new Exception("Text segment is null");
            }

            if (parameter == null)
            {
                throw new Exception("FCS parameter is null");
            }

            if (keyValues.ContainsKey(FcsKewywords.BeginAnalysisKey) &&
                long.TryParse(keyValues[FcsKewywords.BeginAnalysisKey], out var beginAnalysis))
            {
                parameter.AnalysisBegin = beginAnalysis;
            }

            if (keyValues.ContainsKey(FcsKewywords.EndAnalysisKey) &&
                long.TryParse(keyValues[FcsKewywords.EndAnalysisKey], out var endAnalysis))
            {
                parameter.AnalysisEnd = endAnalysis;
            }

            if (keyValues.ContainsKey(FcsKewywords.BeginDataKey) &&
                long.TryParse(keyValues[FcsKewywords.BeginDataKey], out var beginData))
            {
                parameter.DataBegin = beginData;
            }

            if (keyValues.ContainsKey(FcsKewywords.EndDataKey) &&
                long.TryParse(keyValues[FcsKewywords.EndDataKey], out var endData))
            {
                parameter.DataEnd = endData;
            }
            if (keyValues.ContainsKey(FcsKewywords.BeginSTextKey) && long.TryParse(keyValues[FcsKewywords.BeginSTextKey], out var beginSText))
            {
                parameter.STextBegin = beginSText;
            }
            if (keyValues.ContainsKey(FcsKewywords.EndSTextKey) && long.TryParse(keyValues[FcsKewywords.EndSTextKey], out var endSText))
            {
                parameter.STextEnd = endSText;
            }

            if (keyValues.ContainsKey(FcsKewywords.ByteOrdKey))
            {
                parameter.ByteOrd = FcsByteEndiannessConvert.ConvertToEnum(keyValues[FcsKewywords.ByteOrdKey]);
            }

            if (keyValues.ContainsKey(FcsKewywords.DataTypeKey))
            {
                parameter.DataType = FcsDataTypeConvert.ConvertToEnum(keyValues[FcsKewywords.DataTypeKey]);
            }

            if (keyValues.ContainsKey(FcsKewywords.NextDataKey) &&
                long.TryParse(keyValues[FcsKewywords.NextDataKey], out var nextData))
            {
                parameter.NextData = nextData;
            }

            if (keyValues.ContainsKey(FcsKewywords.PARKey) &&
                uint.TryParse(keyValues[FcsKewywords.PARKey], out var par))
            {
                parameter.PAR = par;
            }

            if (keyValues.ContainsKey(FcsKewywords.TOTKey) &&
                uint.TryParse(keyValues[FcsKewywords.TOTKey], out var tot))
            {
                parameter.TOT = tot;
            }
        }

        /// <summary>
        /// Parse signal measurements of dataset.
        /// </summary>
        /// <param name="textSegment">The key-value dictionary of text segment.</param>
        /// <param name="par">The channel account.</param>
        /// <param name="defaultDataType">The default data type.</param>
        protected virtual IList<FcsSignalMeasurement> ParseSignalMeasurements(Dictionary<string, string> textSegment, uint par, FcsDataType defaultDataType)
        {
            if (textSegment == null)
            {
                throw new Exception("Text segment is null");
            }

            if (par <= 0)
            {
                throw new Exception("PAR can't be zero");
            }

            uint pnbTemp = 0;
            switch (defaultDataType)
            {
                case FcsDataType.F:
                    pnbTemp = 32;
                    break;
                case FcsDataType.D:
                    pnbTemp = 64;
                    break;
            }
            var measurements = new List<FcsSignalMeasurement>();
            for (uint i = 1; i <= par; i++)
            {
                var param = new FcsSignalMeasurement();
                var pnn = string.Format(FcsKewywords.PnNKey, i);
                if (textSegment.ContainsKey(pnn))
                {
                    param.PnN = textSegment[pnn];
                }
                if (pnbTemp == 0)
                {
                    var pnb = string.Format(FcsKewywords.PnBKey, i);
                    if (textSegment.ContainsKey(pnb) && uint.TryParse(textSegment[pnb], out var pnbValue))
                    {
                        if (pnbValue % 8 != 0)
                        {
                            throw new Exception("PnB value that are not divisible by 8 are not support");
                        }
                        param.PnB = pnbValue;
                    }
                }
                else
                {
                    param.PnB = pnbTemp;
                }

                var pne = string.Format(FcsKewywords.PnEKey, i);
                if (textSegment.ContainsKey(pne))
                {
                    param.PnE = new FcsAmplificationType(textSegment[pne]);
                }

                var pnr = string.Format(FcsKewywords.PnRKey, i);
                if (textSegment.ContainsKey(pnr) && ulong.TryParse(textSegment[pnr], out var pnro))
                {
                    param.PnR = pnro;
                }

                var pnd = string.Format(FcsKewywords.PnDKey, i);
                if (textSegment.ContainsKey(pnd))
                {
                    param.PnD = new FcsSuggestedVisualizationScale(textSegment[pnd]);
                }

                var pnf = string.Format(FcsKewywords.PnFKey, i);
                if (textSegment.ContainsKey(pnf))
                {
                    param.PnF = textSegment[pnf];
                }

                var png = string.Format(FcsKewywords.PnGKey, i);
                if (textSegment.ContainsKey(png) && double.TryParse(textSegment[png], out var pngo))
                {
                    param.PnG = pngo;
                }

                var pnl = string.Format(FcsKewywords.PnLKey, i);
                if (textSegment.ContainsKey(pnl))
                {
                    param.PnL = textSegment[pnl];
                }

                var pno = string.Format(FcsKewywords.PnOKey, i);
                if (textSegment.ContainsKey(pno) && uint.TryParse(textSegment[pno], out var pnoo))
                {
                    param.PnO = pnoo;
                }

                var pns = string.Format(FcsKewywords.PnSKey, i);
                if (textSegment.ContainsKey(pns))
                {
                    param.PnS = textSegment[pns];
                }

                var pnt = string.Format(FcsKewywords.PnTKey, i);
                if (textSegment.ContainsKey(pnt))
                {
                    param.PnT = textSegment[pnt];
                }

                var pnv = string.Format(FcsKewywords.PnVKey, i);
                if (textSegment.ContainsKey(pnv) && double.TryParse(textSegment[pnv], out var pnvo))
                {
                    param.PnV = pnvo;
                }

                var pndatatype = string.Format(FcsKewywords.PnDataTypeKey, i);
                if (textSegment.ContainsKey(pndatatype) && textSegment[pndatatype].Length == 1)
                {
                    param.PnDATATYPE = FcsDataTypeConvert.ConvertToEnum(textSegment[pndatatype][0]);
                }
                else
                {
                    param.PnDATATYPE = defaultDataType;
                }
                measurements.Add(param);
            }

            return measurements;
        }

        /// <summary>
        /// Parse Data segment.
        /// </summary>
        /// <param name="stream">Readable stream.</param>
        /// <param name="fileBeginOffset">Dataset begin offset, based on the file.</param>
        /// <param name="dataBegin">Data segment begin offset, based on the dataset.</param>
        /// <param name="dataEnd">Data segment end offset, based on the dataset.</param>
        /// <param name="measurements">Measurements.</param>
        /// <param name="tot">The total number of events.</param>
        /// <param name="dataType">The default datatype of dataset.</param>
        /// <param name="byteOrd">Byte order.</param>
        protected virtual void ParseData(Stream stream, long fileBeginOffset, long dataBegin, long dataEnd, IList<FcsSignalMeasurement> measurements, uint tot, FcsDataType dataType, FcsByteEndianness byteOrd)
        {
            if (measurements == null)
            {
                throw new Exception("Measurement list object is null");
            }

            if (dataType == FcsDataType.Unknown)
            {
                throw new Exception("FCS datatype is not supported");
            }

            if (stream.Length < (fileBeginOffset + dataEnd))
            {
                throw new Exception("Stream length is not enough");
            }

            stream.Seek(fileBeginOffset + dataBegin, SeekOrigin.Begin);
            for (uint i = 0; i < tot; i++)
            {
                foreach (var item in measurements)
                {
                    var bytes = new byte[item.PnBByteLength];
                    if (item.PnBByteLength != stream.Read(bytes, 0, item.PnBByteLength))
                    {
                        throw new Exception("Read data segment failed,stream length is not enough");
                    }
                    item.AddOneValue(bytes, byteOrd);
                }
            }
        }

        /// <summary>
        /// Read a data set.
        /// </summary>
        /// <param name="stream">The file stream.</param>
        /// <param name="nextData">The begin position of next data set.</param>
        /// <param name="fileBeginOffset">The begin position of the dataset, based on the stream.</param>
        /// <returns></returns>
        public virtual FcsDataSet ReadDataSet(Stream stream, out long nextData, long fileBeginOffset = 0)
        {
            if (fileBeginOffset > stream.Length)
            {
                throw new Exception("Offset is too big");
            }
            var fcs = new FcsDataSet();
            var parameter = new FcsDataSetParameter();
            // 1. Read head segment.
            // 2. Read text segment key-values.
            // 3. Parse parameter from text segment.
            // 4. Read supplemental text segment key-values.
            // 5. Parse signal measurements.
            // 6. Parse data.
            ReadHeadSegment(stream, fileBeginOffset, parameter);
            ReadUtf8KeyValues(ReadBytes(stream, fileBeginOffset, parameter.TextBegin, parameter.TextEnd), fcs.TextSegment, parameter.DelimiterByte);
            ParseParameterFromTextSegment(fcs.TextSegment, parameter);

            if (parameter.STextBegin != 0 && parameter.STextEnd != 0)
            {
                ReadUtf8KeyValues(ReadBytes(stream, fileBeginOffset, parameter.STextBegin, parameter.STextEnd), fcs.TextSegment, parameter.DelimiterByte);
            }

            if (parameter.AnalysisBegin != 0 && parameter.AnalysisEnd != 0)
            {
                ReadUtf8KeyValues(ReadBytes(stream, fileBeginOffset, parameter.AnalysisBegin, parameter.AnalysisEnd), fcs.AnalysisSegment, parameter.DelimiterByte);
            }

            fcs.Measurements = ParseSignalMeasurements(fcs.TextSegment, parameter.PAR, parameter.DataType);
            ParseData(stream, fileBeginOffset, parameter.DataBegin, parameter.DataEnd, fcs.Measurements, parameter.TOT, parameter.DataType, parameter.ByteOrd);
            nextData = parameter.NextData;
            return fcs;
        }

        /// <summary>
        /// Write to the stream.
        /// </summary>
        /// <param name="stream">Writable stream.</param>
        /// <param name="fileBeginOffset">The begin offset of the dataset, based on the file.</param>
        /// <param name="writeBegin">The begin offset of the position to write byte, based on the data set.</param>
        /// <param name="data">The data to write.</param>
        protected virtual void Write(Stream stream, long fileBeginOffset, long writeBegin, byte[] data)
        {
            if (!stream.CanWrite || !stream.CanSeek)
            {
                throw new Exception("Stream can't write or seek");
            }

            if ((fileBeginOffset + writeBegin) < 0)
            {
                throw new Exception("Can't write,offset must be greater than zero");
            }

            if (data == null || data.Length <= 0)
            {
                throw new Exception("Can't write,data is null or empty");
            }

            stream.Seek(fileBeginOffset + writeBegin, SeekOrigin.Begin);
            stream.Write(data, 0, data.Length);
        }

        /// <summary>
        /// Write key-values to stream.
        /// </summary>
        /// <param name="keyValues">The key-value dictionary.</param>
        /// <param name="delimiterByte">Delimiter.</param>
        /// <returns></returns>
        protected virtual MemoryStream WriteKeyValuesToStream(Dictionary<string, string> keyValues, byte delimiterByte)
        {
            if (keyValues == null || keyValues.Count <= 0)
            {
                throw new Exception("Dictionary is null or empty");
            }

            var stream = new MemoryStream();
            stream.WriteByte(delimiterByte);
            foreach (var item in keyValues)
            {
                if (string.IsNullOrEmpty(item.Key) || string.IsNullOrEmpty(item.Value))
                {
                    throw new Exception("Dictionary key and value can't be null or empty");
                }
                var temp = ConvertKeyValueToByteArray(item.Key, item.Value, delimiterByte);
                stream.Write(temp.ToArray(), 0, temp.Count);
            }
            return stream;
        }

        /// <summary>
        /// Convert key-values to byte array.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="delimiterByte"></param>
        /// <returns></returns>
        protected virtual List<byte> ConvertKeyValueToByteArray(string key, string value, byte delimiterByte)
        {
            var delimiterChar = Encoding.UTF8.GetString(new[] { delimiterByte });
            var delimiterDoubleChar = string.Concat(delimiterChar, delimiterChar);
            var bytes = new List<byte>();
            if (key.Contains(delimiterChar))
            {
                key = key.Replace(delimiterChar, delimiterDoubleChar);
            }

            if (value.Contains(delimiterChar))
            {
                value = value.Replace(delimiterChar, delimiterDoubleChar);
            }

            var keys = Encoding.UTF8.GetBytes(key);
            var values = Encoding.UTF8.GetBytes(value);
            bytes.AddRange(keys);
            bytes.Add(delimiterByte);
            bytes.AddRange(values);
            bytes.Add(delimiterByte);
            return bytes;
        }

        /// <summary>
        /// Write data segment to stream.
        /// </summary>
        /// <param name="measurements"></param>
        /// <returns></returns>
        protected virtual MemoryStream WriteDataSegmentToStream(IList<FcsSignalMeasurement> measurements)
        {
            if (measurements == null || measurements.Count <= 0)
            {
                throw new Exception("Measurement array is null or empty");
            }
            var allMeasurementBitLength = measurements.Aggregate<FcsSignalMeasurement, long>(0, (current, item) => current + item.PnB / 8);
            uint startOffsetOfEvent = 0;
            MemoryStream stream = null;
            foreach (var item in measurements)
            {
                var byteLength = item.PnB / 8;
                if (stream == null)
                {
                    var manager = new RecyclableMemoryStreamManager();
                    //stream = new MemoryStream(new byte[allMeasurementBitLength * item.Values.Count]);
                    stream = manager.GetStream();
                }

                for (var i = 0; i < item.Values.Count; i++)
                {
                    var v = item.Values[i];
                    byte[] bytes;
                    switch (v)
                    {
                        case double d:
                            bytes = BitConverter.GetBytes(d);
                            break;
                        case float f:
                            bytes = BitConverter.GetBytes(f);
                            break;
                        case ulong ul:
                            bytes = BitConverter.GetBytes(ul);
                            break;
                        case uint ui:
                            bytes = BitConverter.GetBytes(ui);
                            break;
                        case ushort us:
                            bytes = BitConverter.GetBytes(us);
                            break;
                        case byte b:
                            bytes = BitConverter.GetBytes(b);
                            break;
                        default:
                            throw new Exception("Data type not supported");
                    }

                    if (bytes.Length > byteLength)
                    {
                        throw new Exception("Data type error,byte array is too larger");
                    }

                    var targetBytes = new byte[byteLength];
                    bytes.CopyTo(targetBytes, byteLength - bytes.Length);
                    stream.Seek(i * allMeasurementBitLength + startOffsetOfEvent, SeekOrigin.Begin);
                    stream.Write(targetBytes, 0, targetBytes.Length);
                }
                startOffsetOfEvent += byteLength;
            }
            return stream;
        }

        /// <summary>
        /// Reset text segment.
        /// </summary>
        /// <param name="measurements">The measurements of the dataset.</param>
        /// <param name="textSegment">The text segment.</param>
        protected abstract void ResetTextSegment(Dictionary<string, string> textSegment, IList<FcsSignalMeasurement> measurements);

        /// <summary>
        /// Split the text segment. 
        /// </summary>
        /// <param name="keyValues">The key-values.</param>
        /// <param name="measurementCount"></param>
        /// <return>Two dictionaries. 1. Text segment. 2. Supplement segment.
        /// </return>
        protected virtual Dictionary<string, string>[] SplitTextSegment(Dictionary<string, string> keyValues, int measurementCount)
        {
            if (keyValues == null || keyValues.Count <= 0)
            {
                throw new Exception("Split failed, dictionary is null or empty");
            }

            if (measurementCount < 0)
            {
                throw new Exception("Split error, measurement count is less then zero");
            }

            var textKeys = new List<string>
            {
                FcsKewywords.ByteOrdKey,
                FcsKewywords.CYTKey,
                FcsKewywords.DataTypeKey,
                FcsKewywords.PARKey,
                FcsKewywords.TOTKey
            };

            for (var i = 1; i <= measurementCount; i++)
            {
                textKeys.Add(string.Format(FcsKewywords.PnBKey, i));
                textKeys.Add(string.Format(FcsKewywords.PnEKey, i));
                textKeys.Add(string.Format(FcsKewywords.PnNKey, i));
                textKeys.Add(string.Format(FcsKewywords.PnRKey, i));
            }
            var text = new Dictionary<string, string>();//New text segment.
            var supplementalText = new Dictionary<string, string>();//New supplemental text segment.
            foreach (var item in keyValues)
            {
                if (textKeys.Contains(item.Key))
                {
                    text.Add(item.Key, item.Value);
                }
                else
                {
                    supplementalText.Add(item.Key, item.Value);
                }
            }
            return new[] { text, supplementalText };
        }

        /// <summary>
        /// Calculate the length of the text segment based on the length for every segment.
        /// </summary>
        /// <param name="headLength"></param>
        /// <param name="textLength"></param>
        /// <param name="dataLength"></param>
        /// <param name="supplementalTextLength"></param>
        /// <param name="analysisLength"></param>
        /// <param name="haveNext"></param>
        /// <returns></returns>
        protected virtual long CalculateSegmentTextLength(long headLength, long textLength, long dataLength, long supplementalTextLength, long analysisLength, bool haveNext)
        {
            long length = 0;
            if (dataLength > 0)
            {
                var temp = headLength + textLength;
                length += FcsKewywords.BeginDataKey.Length;
                length += FcsKewywords.EndDataKey.Length;
                length += 4;//4 delimiter bytes
                length += temp.ToString().Length; // Data begin value length.
                length += (temp + dataLength).ToString().Length; // Data end value length.
            }
            //if (supplementalTextLength > 0)
            {
                var temp = headLength + textLength + dataLength;
                length += FcsKewywords.BeginSTextKey.Length;
                length += FcsKewywords.EndSTextKey.Length;
                length += 4;//4 delimiter bytes
                length += supplementalTextLength > 0 ? temp.ToString().Length : 1;
                length += supplementalTextLength > 0 ? (temp + supplementalTextLength).ToString().Length : 1;
            }
            //if (analysisLength > 0)
            {
                var temp = headLength + textLength + dataLength + supplementalTextLength;
                length += FcsKewywords.BeginAnalysisKey.Length;
                length += FcsKewywords.EndAnalysisKey.Length;
                length += 4;//4 delimiter bytes
                length += analysisLength > 0 ? temp.ToString().Length : 1;
                length += analysisLength > 0 ? (temp + analysisLength).ToString().Length : 1;
            }
            if (haveNext)
            {
                var temp = headLength + textLength + dataLength + supplementalTextLength + analysisLength + 2;
                length += FcsKewywords.NextDataKey.Length;
                length += 2;//2 delimiter bytes
                length += temp.ToString().Length;
            }
            else
            {
                length += FcsKewywords.NextDataKey.Length;
                length += 3;//2 delimiter bytes + next data flag
            }
            return length;
        }
        /// <summary>
        /// Create an empty head segment, include FCS file version.
        /// </summary>
        /// <returns></returns>
        protected abstract byte[] CreateEmptyHeadBytes();

        /// <summary>
        /// Save datasets to the stream.
        /// </summary>
        /// <param name="stream">Writable stream.</param>
        /// <param name="list">Dataset list.</param>
        /// <returns></returns>
        public virtual void Save(Stream stream, params FcsDataSet[] list)
        {
            if (!stream.CanWrite || !stream.CanSeek)
            {
                throw new Exception("Save failed, stream can't write or seek");
            }

            if (list == null || list.Length <= 0)
            {
                throw new Exception("Save failed, dataset is null or empty");
            }
            long fileBeginOffset = 0;
            var datasetMaxIndex = list.Length - 1;
            for (var i = 0; i <= datasetMaxIndex; i++)
            {
                Save(stream, list[i], fileBeginOffset, i < datasetMaxIndex, out var nextOffset);
                fileBeginOffset += nextOffset;
            }
        }
        /// <summary>
        /// Save a dataset to the stream.
        /// The sequence is:
        /// 1. Head segment.
        /// 2. Text segment.
        /// 3. Data segment.
        /// 4. Supplement text segment.
        /// 5. Analysis segment.
        /// </summary>
        /// <param name="stream">Writable stream.</param>
        /// <param name="fcsDataSet">DataSet.</param>
        /// <param name="fileBeginOffset">The begin offset, based on the stream.</param>
        /// <param name="haveNext">Flag to indicate whether another dataset is behind.</param>
        /// <param name="nextOffset">The begin offset of the next dataset, based on the current dataset.</param>
        /// <returns></returns>
        protected virtual void Save(Stream stream, FcsDataSet fcsDataSet, long fileBeginOffset, bool haveNext, out long nextOffset)
        {
            ResetTextSegment(fcsDataSet.TextSegment, fcsDataSet.Measurements);
            // Remove all begin-end key-values, will be re-calculated later.
            if (fcsDataSet.TextSegment.ContainsKey(FcsKewywords.BeginAnalysisKey))
            {
                fcsDataSet.TextSegment.Remove(FcsKewywords.BeginAnalysisKey);
            }

            if (fcsDataSet.TextSegment.ContainsKey(FcsKewywords.EndAnalysisKey))
            {
                fcsDataSet.TextSegment.Remove(FcsKewywords.EndAnalysisKey);
            }

            if (fcsDataSet.TextSegment.ContainsKey(FcsKewywords.BeginSTextKey))
            {
                fcsDataSet.TextSegment.Remove(FcsKewywords.BeginSTextKey);
            }

            if (fcsDataSet.TextSegment.ContainsKey(FcsKewywords.EndSTextKey))
            {
                fcsDataSet.TextSegment.Remove(FcsKewywords.EndSTextKey);
            }

            if (fcsDataSet.TextSegment.ContainsKey(FcsKewywords.BeginDataKey))
            {
                fcsDataSet.TextSegment.Remove(FcsKewywords.BeginDataKey);
            }

            if (fcsDataSet.TextSegment.ContainsKey(FcsKewywords.EndDataKey))
            {
                fcsDataSet.TextSegment.Remove(FcsKewywords.EndDataKey);
            }

            if (fcsDataSet.TextSegment.ContainsKey(FcsKewywords.NextDataKey))
            {
                fcsDataSet.TextSegment.Remove(FcsKewywords.NextDataKey);
            }

            var headBytes = CreateEmptyHeadBytes();
            var textStream = WriteKeyValuesToStream(fcsDataSet.TextSegment, FcsKewywords.DelimiterByte);
            MemoryStream supplementalTextStream = null;
            var analysisStream = (fcsDataSet.AnalysisSegment != null && fcsDataSet.AnalysisSegment.Count > 0)
                ? WriteKeyValuesToStream(fcsDataSet.AnalysisSegment, FcsKewywords.DelimiterByte)
                : null;

            long headLength = headBytes.Length;
            var bitLength = fcsDataSet.Measurements.Aggregate(0L, (current, item) => current + item.PnB);

            var dataLength = (bitLength / 8) * Convert.ToInt32(fcsDataSet.TextSegment[FcsKewywords.TOTKey]);//The length of the data segment.
            var textLength = textStream.Length;
            var supplementalTextLength = 0L;
            var analysisLength = analysisStream?.Length ?? 0L;

            var beginendLength = 0L;
            var calculationLength = CalculateSegmentTextLength(headLength, textLength + beginendLength, dataLength, supplementalTextLength, analysisLength, haveNext);
            while (calculationLength != beginendLength)
            {
                beginendLength = calculationLength;
                calculationLength = CalculateSegmentTextLength(headLength, textLength + beginendLength, dataLength, supplementalTextLength, analysisLength, haveNext);
            }
            if ((headLength + textLength + beginendLength) > 99999999)//The full text segment can only be in the first 99999999 bytes.
            {
                var texts = SplitTextSegment(fcsDataSet.TextSegment, fcsDataSet.Measurements.Count);
                textStream = WriteKeyValuesToStream(texts[0], FcsKewywords.DelimiterByte);
                supplementalTextStream = WriteKeyValuesToStream(texts[1], FcsKewywords.DelimiterByte);
                textLength = textStream.Length;
                supplementalTextLength = supplementalTextStream.Length;
                beginendLength = 0L;
                calculationLength = CalculateSegmentTextLength(headLength, textLength + beginendLength, dataLength, supplementalTextLength, analysisLength, haveNext);
                while (calculationLength != beginendLength)
                {
                    beginendLength = calculationLength;
                    calculationLength = CalculateSegmentTextLength(headLength, textLength + beginendLength, dataLength, supplementalTextLength, analysisLength, haveNext);
                }
            }

            var segmentLocation = new MemoryStream();
            var textBegin = Encoding.UTF8.GetBytes(headLength.ToString());
            textBegin.CopyTo(headBytes, 18 - textBegin.Length);
            var textEnd = Encoding.UTF8.GetBytes((headLength + textLength + beginendLength - 1).ToString());
            textEnd.CopyTo(headBytes, 26 - textEnd.Length);

            var dataBegin = Encoding.UTF8.GetBytes((headLength + textLength + beginendLength).ToString());
            var dataEnd = Encoding.UTF8.GetBytes((headLength + textLength + beginendLength + dataLength - 1).ToString());
            if (dataEnd.Length <= 8)
            {
                dataBegin.CopyTo(headBytes, 34 - dataBegin.Length);
                dataEnd.CopyTo(headBytes, 42 - dataEnd.Length);
            }
            segmentLocation.WriteAll(Encoding.UTF8.GetBytes(FcsKewywords.BeginDataKey));
            segmentLocation.WriteByte(FcsKewywords.DelimiterByte);
            segmentLocation.WriteAll(dataBegin);
            segmentLocation.WriteByte(FcsKewywords.DelimiterByte);
            segmentLocation.WriteAll(Encoding.UTF8.GetBytes(FcsKewywords.EndDataKey));
            segmentLocation.WriteByte(FcsKewywords.DelimiterByte);
            segmentLocation.WriteAll(dataEnd);
            segmentLocation.WriteByte(FcsKewywords.DelimiterByte);

            if (supplementalTextLength > 0)
            {
                var stextBegin = (headLength + textLength + beginendLength + dataLength).ToString();
                var stextEnd = (headLength + textLength + beginendLength + dataLength + supplementalTextLength - 1).ToString();
                segmentLocation.WriteAll(ConvertKeyValueToByteArray(FcsKewywords.BeginSTextKey, stextBegin, FcsKewywords.DelimiterByte));
                segmentLocation.WriteAll(ConvertKeyValueToByteArray(FcsKewywords.EndSTextKey, stextEnd, FcsKewywords.DelimiterByte));
            }
            else
            {
                segmentLocation.WriteAll(ConvertKeyValueToByteArray(FcsKewywords.BeginSTextKey, "0", FcsKewywords.DelimiterByte));
                segmentLocation.WriteAll(ConvertKeyValueToByteArray(FcsKewywords.EndSTextKey, "0", FcsKewywords.DelimiterByte));
            }
            if (analysisLength > 0)
            {
                var analysisBegin = Encoding.UTF8.GetBytes((headLength + textLength + beginendLength + dataLength + supplementalTextLength).ToString());
                var analysisEnd = Encoding.UTF8.GetBytes((headLength + textLength + beginendLength + dataLength + supplementalTextLength + analysisLength - 1).ToString());
                if (analysisEnd.Length <= 8)
                {
                    analysisBegin.CopyTo(headBytes, 42 - analysisBegin.Length);
                    analysisEnd.CopyTo(headBytes, 50 - analysisEnd.Length);
                }
                segmentLocation.WriteAll(Encoding.UTF8.GetBytes(FcsKewywords.BeginAnalysisKey));
                segmentLocation.WriteByte(FcsKewywords.DelimiterByte);
                segmentLocation.WriteAll(analysisBegin);
                segmentLocation.WriteByte(FcsKewywords.DelimiterByte);
                segmentLocation.WriteAll(Encoding.UTF8.GetBytes(FcsKewywords.EndAnalysisKey));
                segmentLocation.WriteByte(FcsKewywords.DelimiterByte);
                segmentLocation.WriteAll(analysisEnd);
                segmentLocation.WriteByte(FcsKewywords.DelimiterByte);
            }
            else
            {
                segmentLocation.WriteAll(ConvertKeyValueToByteArray(FcsKewywords.BeginAnalysisKey, "0", FcsKewywords.DelimiterByte));
                segmentLocation.WriteAll(ConvertKeyValueToByteArray(FcsKewywords.EndAnalysisKey, "0", FcsKewywords.DelimiterByte));
            }
            if (haveNext)
            {
                nextOffset = headLength + textLength + beginendLength + dataLength + supplementalTextLength + analysisLength + 2;
                segmentLocation.WriteAll(ConvertKeyValueToByteArray(FcsKewywords.NextDataKey, nextOffset.ToString(), FcsKewywords.DelimiterByte));
            }
            else
            {
                nextOffset = 0;
                segmentLocation.WriteAll(ConvertKeyValueToByteArray(FcsKewywords.NextDataKey, "0", FcsKewywords.DelimiterByte));
            }
            //Write stream and calculate crc
            stream.Seek(fileBeginOffset, SeekOrigin.Begin);
            stream.Write(headBytes, 0, headBytes.Length);
            var crc = CrcCalculator.ComputeCrc(headBytes);
            textStream.WriteTo(stream);
            crc = CrcCalculator.ComputeCrc(textStream, crc);
            textStream.Dispose();
            segmentLocation.WriteTo(stream);
            crc = CrcCalculator.ComputeCrc(segmentLocation, crc);
            segmentLocation.Dispose();
            GC.Collect();
            var dataStream = WriteDataSegmentToStream(fcsDataSet.Measurements);
            dataStream.WriteTo(stream);
            crc = CrcCalculator.ComputeCrc(dataStream, crc);
            dataStream.Dispose();
            if (supplementalTextStream != null)
            {
                supplementalTextStream.WriteTo(stream);
                crc = CrcCalculator.ComputeCrc(supplementalTextStream, crc);
                supplementalTextStream.Dispose();
            }
            if (analysisStream != null)
            {
                analysisStream.WriteTo(stream);
                crc = CrcCalculator.ComputeCrc(analysisStream, crc);
                analysisStream.Dispose();
            }
            stream.Write(BitConverter.GetBytes(crc), 0, 2);
        }
    }
}
