using Fcs.Core;
using Fcs.File;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace FcsParser.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileInfo = new FileInfo("Tube1.fcs");

            using (var mmf = MemoryMappedFile.CreateFromFile(@"Tube1.fcs", FileMode.Open, "ImgA"))
            {
                // Solution 1: Memory map + umanaged memory stream.
                //using (var accessor = mmf.CreateViewAccessor(0, fileInfo.Length, MemoryMappedFileAccess.ReadWrite))
                //{
                //    using (var fileStream = new UnmanagedMemoryStream(accessor.SafeMemoryMappedViewHandle, 0, fileInfo.Length, FileAccess.ReadWrite))
                //    {
                //        var fcs = new FcsFileParser3_1();
                //        long nextDataOffset;
                //        var dataset = fcs.ReadDataSet(fileStream, out nextDataOffset, 0);
                //        var firstM = dataset.Measurements[0];

                //        if (true)
                //        {
                //            for (long i = dataset.Measurements.Count; i < 80; i++)
                //            {
                //                dataset.Measurements.Add(Clone(firstM));
                //            }

                //            foreach (var m in dataset.Measurements)
                //            {
                //                for (int i = m.Values.Count; i < 20000000; i++)
                //                {
                //                    m.Values.Add(m.Values[0]);
                //                }
                //            }

                //            fcs.Save(fileStream, dataset);
                //        }
                //    }
                //}

                //// Solution 2: Memory map + managed memory stream.
                //// Create a random access view, from the 256th megabyte (the offset)
                //// to the 768th megabyte (the offset plus length).
                //using (var fileStream = mmf.CreateViewStream())
                //{
                //    var fcs = new FCSFile3_1();
                //    long nextDataOffset;
                //    var dataset = fcs.ReadDataSet(fileStream, out nextDataOffset, 0);
                //    var firstM = dataset.Measurements[0];
                //    for (long i = dataset.Measurements.Count; i < 80; i++)
                //    {
                //        dataset.Measurements.Add(Clone(firstM));
                //    }

                //    foreach (var m in dataset.Measurements)
                //    {
                //        for (int i = 0; i < 6000000; i++)
                //        {
                //            m.Values.Add(m.Values[0]);
                //        }
                //    }

                //    fcs.Save(fileStream, dataset);
                //}
            }

            // Solution 3: File stream.
            using (var fileStream = fileInfo.Open(FileMode.Open, FileAccess.ReadWrite))
            {
                var fcs = new FcsFileParser3_1();
                long nextDataOffset;
                var dataset = fcs.ReadDataSet(fileStream, out nextDataOffset, 0);
                var firstM = dataset.Measurements[0];
                if (true)
                {
                    for (long i = dataset.Measurements.Count; i < 120; i++)
                    {
                        dataset.Measurements.Add(firstM);
                    }

                    foreach (var m in dataset.Measurements)
                    {
                        for (int i = m.Values.Count; i < 20000000; i++)
                        {
                            m.Values.Add(m.Values[0]);
                        }
                    }
                }

                Console.WriteLine("Start save.");
                var fileOutInfo = new FileInfo("Tube1_Out.fcs");
                using (var fileStreamOut = fileOutInfo.Open(FileMode.CreateNew, FileAccess.ReadWrite))
                {
                    var fcs_out = new FcsFileParser3_1();
                    fcs_out.Save(fileStreamOut, dataset);
                }
            }
        }

        public static FcsSignalMeasurement Clone(FcsSignalMeasurement source)
        {
            var target = new FcsSignalMeasurement();
            var customerType = target.GetType();
            foreach (var prop in source.GetType().GetProperties())
            {
                var propGetter = prop.GetGetMethod();
                var propSetter = customerType.GetProperty(prop.Name).GetSetMethod(true);
                var valueToSet = propGetter.Invoke(source, null);
                propSetter.Invoke(target, new[] { valueToSet });
            }

            target.Values = new List<object>();
            foreach (var v in source.Values)
            {
                target.Values.Add(v);
            }

            return target;
        }
    }
}
