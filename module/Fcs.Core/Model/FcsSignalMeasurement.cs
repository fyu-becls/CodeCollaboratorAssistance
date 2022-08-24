using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Fcs.Core
{
    /// <summary>
    /// Required properties of FCS measurement.
    /// </summary>
    public partial class FcsSignalMeasurement
    {
        /// <summary>
        /// Short name for parameter n.
        /// </summary>
        public string PnN { get; set; }
        private uint _pnb;
        /// <summary>
        /// Number of bits reserved for measurement n
        /// </summary>
        public uint PnB
        {
            get { return _pnb; }
            set
            {
                _pnb = value;
                PnBByteLength = Convert.ToInt32(value / 8);
            }
        }
        public int PnBByteLength { get; private set; }
        /// <summary>
        /// Amplification type for measurement n.
        /// </summary>
        public FcsAmplificationType PnE { get; set; }
        /// <summary>
        /// Range for measurement n.
        /// </summary>
        public ulong PnR { get; set; }
        /// <summary>
        /// The data of this measurement.
        /// </summary>
        public IList Values { get; set; }
    }

    /// <summary>
    /// Optional properties of FCS measurement.
    /// </summary>
    public partial class FcsSignalMeasurement
    {
        /// <summary>
        /// Suggested visualization scale for FCS measurement n
        /// </summary>
        public FcsSuggestedVisualizationScale PnD { get; set; }
        /// <summary>
        /// Name of optical filter for FCS measurement n
        /// </summary>
        public string PnF { get; set; }
        /// <summary>
        /// Amplifier gain used for acquisition of FCS measurement n
        /// </summary>
        public double PnG { get; set; } = double.NaN;
        /// <summary>
        /// Excitation wavelength(s) for FCS measurement n
        /// </summary>
        public string PnL { get; set; }
        /// <summary>
        /// Excitation power for FCS measurement n
        /// </summary>
        public uint PnO { get; set; }
        /// <summary>
        /// Long name (description) used for FCS measurement n
        /// </summary>
        public string PnS { get; set; }
        /// <summary>
        /// Detector type for FCS measurement n
        /// </summary>
        public string PnT { get; set; }
        /// <summary>
        /// Detector voltage for FCS measurement n
        /// </summary>
        public double PnV { get; set; } = double.NaN;
        /// <summary>
        /// Data type for FCS measurement n
        /// </summary>
        public FcsDataType PnDATATYPE { get; set; }
    }

    /// <summary>
    /// Functions
    /// </summary>
    public partial class FcsSignalMeasurement
    {
        /// <summary>
        /// Add a data to the measurement data list.
        /// </summary>
        /// <param name="bytes">Byte array.</param>
        /// <param name="byteOrd">Byte order. Default little endian as Windows.</param>
        public virtual void AddValue(byte[] bytes, FcsByteEndianness byteOrd = FcsByteEndianness.LittleEndian)
        {
            if (bytes == null || bytes.Length <= 0)
            {
                return;
            }
            if (byteOrd == FcsByteEndianness.BigEndian && BitConverter.IsLittleEndian ||
                byteOrd == FcsByteEndianness.LittleEndian && !BitConverter.IsLittleEndian)
            {
                bytes = bytes.Reverse().ToArray();
            }

            switch (PnDATATYPE)
            {
                case FcsDataType.I:
                    if (Values == null)
                    {
                        if (PnB <= 8)
                        {
                            Values = new List<byte>();
                        }
                        else if (PnB > 8 && PnB <= 16)
                        {
                            Values = new List<ushort>();
                        }
                        else if (PnB > 16 && PnB <= 32)
                        {
                            Values = new List<uint>();
                        }
                        else if (PnB > 32 && PnB <= 64)
                        {
                            Values = new List<ulong>();
                        }
                        else
                        {
                            throw new Exception("Can't analyze data,PnB is too big");
                        }
                    }

                    if (PnB <= 8)
                    {
                        Values.Add(BitMask(bytes[bytes.Length - 1]));
                    }
                    else if (PnB > 8 && PnB <= 16)
                    {
                        Values.Add(BitMask(BitConverter.ToUInt16(bytes, bytes.Length > 2 ? bytes.Length - 2 : 0)));
                    }
                    else if (PnB > 16 && PnB <= 32)
                    {
                        Values.Add(BitMask(BitConverter.ToUInt32(bytes, bytes.Length > 4 ? bytes.Length - 4 : 0)));
                    }
                    else if (PnB > 32 && PnB <= 64)
                    {
                        Values.Add(BitMask(BitConverter.ToUInt64(bytes, bytes.Length > 8 ? bytes.Length - 8 : 0)));
                    }
                    else
                    {
                        throw new Exception("Can't analyse data,PnB is too big");
                    }
                    break;
                case FcsDataType.F:
                    if (Values == null)
                    {
                        Values = new List<float>();
                    }
                    Values.Add(BitConverter.ToSingle(bytes, bytes.Length > 4 ? bytes.Length - 4 : 0));
                    break;
                case FcsDataType.D:
                    if (Values == null)
                    {
                        Values = new List<double>();
                    }
                    Values.Add(BitConverter.ToDouble(bytes, bytes.Length > 8 ? bytes.Length - 8 : 0));
                    break;
                default:
                    throw new Exception("Can't analyze data,data type not supported");
            }
        }

        /// <summary>
        /// Bit mask calculation.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual byte BitMask(byte value)
        {
            if (PnR == 0)
            {
                return value;
            }

            return Convert.ToByte(value % PnR);
        }
        public virtual ushort BitMask(ushort value)
        {
            if (PnR == 0)
            {
                return value;
            }
            return Convert.ToUInt16(value % PnR);
        }
        public virtual uint BitMask(uint value)
        {
            if (PnR == 0)
            {
                return value;
            }
            return Convert.ToUInt32(value % PnR);
        }
        public virtual ulong BitMask(ulong value)
        {
            if (PnR == 0)
            {
                return value;
            }
            return value % PnR;
        }

        /// <summary>
        /// PnE calculation, just for datatype=i
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual double PnECalculation(ulong value)
        {
            if (PnR == 0 || this.PnE.PowerNumber == 0 && this.PnE.ZeroValue == 0)
            {
                return value;
            }
            return Math.Pow(10, this.PnE.PowerNumber * value / PnR) * (PnE.ZeroValue == 0d ? 1 : PnE.ZeroValue);
        }

        /// <summary>
        /// 线性放大 PnG 增益,3.2版本只用于datatype=i
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual double PnGCalculation(ulong value)
        {
            if (double.IsNaN(PnG) || PnG <= 0d || PnG == 1d) return value;
            return value / PnG;
        }
        public virtual double PnGCalculation(float value)
        {
            if (double.IsNaN(PnG) || PnG <= 0d || PnG == 1d) return value;
            return value / PnG;
        }
        public virtual double PnGCalculation(double value)
        {
            if (double.IsNaN(PnG) || PnG <= 0d || PnG == 1d) return value;
            return value / PnG;
        }

        /// <summary>
        /// 获取放大前的刻度值
        /// </summary>
        /// <returns></returns>
        public virtual IList<double> GetScaleValues()
        {
            if (PnDATATYPE == FcsDataType.I)
            {
                var list = new List<double>();
                if (PnB <= 8)
                {
                    foreach (byte item in Values)
                    {
                        if (!double.IsNaN(PnG) && PnG > 0d && PnG != 1d) list.Add(PnGCalculation(item));
                        else list.Add(PnECalculation(item));
                    }
                }
                else if (PnB > 8 && PnB <= 16)
                {
                    foreach (ushort item in Values)
                    {
                        if (!double.IsNaN(PnG) && PnG > 0d && PnG != 1d) list.Add(PnGCalculation(item));
                        else list.Add(PnECalculation(item));
                    }
                }
                else if (PnB > 16 && PnB <= 32)
                {
                    foreach (uint item in Values)
                    {
                        if (!double.IsNaN(PnG) && PnG > 0d && PnG != 1d) list.Add(PnGCalculation(item));
                        else list.Add(PnECalculation(item));
                    }
                }
                else if (PnB > 32 && PnB <= 64)
                {
                    foreach (ulong item in Values)
                    {
                        if (!double.IsNaN(PnG) && PnG > 0d && PnG != 1d) list.Add(PnGCalculation(item));
                        else list.Add(PnECalculation(item));
                    }
                }
                else throw new Exception("Can't analyse data,PnB is too big");
                return list;
            }
            else if (PnDATATYPE == FcsDataType.F)
            {
                var list = new List<double>();
                foreach (float item in Values) list.Add(PnGCalculation(item));
                return list;
            }
            else if (PnDATATYPE == FcsDataType.D)
            {
                var list = new List<double>();
                foreach (double item in Values) list.Add(PnGCalculation(item));
                return list;
            }
            else throw new Exception("Data type not supported");
        }

    }
}
