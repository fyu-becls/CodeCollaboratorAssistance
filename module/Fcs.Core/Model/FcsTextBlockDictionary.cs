using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Fcs.Core
{
    internal class FcsTextBlockDictionary : Dictionary<string, string>
    {
        public FcsTextBlockDictionary()
          : base(StringComparer.OrdinalIgnoreCase)
        {
            Reset();
        }

        public FcsTextBlockDictionary(byte[] text, bool utf8)
          : base(StringComparer.OrdinalIgnoreCase)
        {
            Reset(text, utf8);
        }

        public void Reset()
        {
            Clear();
        }

        public void Reset(byte[] text, bool utf8)
        {
            Clear();

            if (text != null && text.Length > 0)
            {
                // The delimiter is the first character.
                var delimiter = text[0];
                var offset = 1;

                // Extract keyword/value pairs.
                var inKeyword = true;
                var keyword = "";

                var kencoder = Encoding.ASCII;
                var vencoder = utf8 ? Encoding.UTF8 : Encoding.Default;

                using (var memStream = new MemoryStream())
                {
                    var pos = offset;
                    var end = text.Length;
                    byte value;
                    bool isDelimiter;

                    while (pos < end)
                    {
                        var inc = 0;

                        if (text[pos] == delimiter)
                        {
                            if (pos < (end - 1) && text[pos + 1] == delimiter)
                            {
                                value = text[pos];
                                isDelimiter = false;
                                inc = 2;
                            }
                            else
                            {
                                value = delimiter;
                                isDelimiter = true;
                                inc = 1;
                            }
                        }
                        else
                        {
                            value = text[pos];
                            isDelimiter = false;
                            inc = 1;
                        }

                        if (isDelimiter)
                        {
                            if (inKeyword)
                            {
                                keyword = kencoder.GetString(memStream.ToArray()).Trim();
                                inKeyword = false;
                            }
                            else
                            {
                                this[keyword] = vencoder.GetString(memStream.ToArray()).Trim();
                                keyword = "";
                                inKeyword = true;
                            }
                            memStream.SetLength(0);
                        }
                        else
                        {
                            memStream.WriteByte(value);
                        }

                        pos += inc;
                    }

                    if (inKeyword)
                    {
                        var s = kencoder.GetString(memStream.ToArray()).Trim();
                        if (!string.IsNullOrEmpty(s))
                        {
                            this[kencoder.GetString(memStream.ToArray())] = "";
                        }
                    }
                    else
                    {
                        this[keyword] = vencoder.GetString(memStream.ToArray());
                    }
                }
            }
        }
    }
}
