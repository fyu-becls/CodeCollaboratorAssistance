using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Text;

namespace Fcs.Core
{
    /// <summary>
    /// Provides culture-invariant string utility functions.
    /// </summary>

    public static class Invariant
    {
        /// <summary>
        /// Compares two strings.
        /// </summary>
        /// <param name="a">The first string to compare.</param>
        /// <param name="b">The scond string to compare.</param>
        /// <param name="ignore">true to ignore case when comparing the strings;
        /// otherwise, false.</param>
        /// <returns>A 32-bit signed integer indicating the lexical relationship
        /// between the two comparands. The value is zero if the two strings are
        /// equal. The value is less than zero if a is less than b. The value is
        /// greater than zero if a is greater than b.</returns>

        public static int Compare(string a, string b, bool ignore)
        {
            var options = ignore ? CompareOptions.IgnoreCase : CompareOptions.None;
            return CultureInfo.InvariantCulture.CompareInfo.Compare(a, b, options);
        }

        /// <summary>
        /// Determines whether the end of the specified text string matches the
        /// specified suffix, using culture-invariant casing rules.
        /// </summary>
        /// <param name="text">The text string to inspect.</param>
        /// <param name="suffix">The string suffix to seek.</param>
        /// <param name="ignore">true to ignore case when comparing the text and
        /// suffix; otherwise, false.</param>
        /// <returns>true if suffix matches the end of this string; otherwise,
        /// false.</returns>
        /// <exception cref="ArgumentNullException">suffix is null.</exception>

        public static bool EndsWith(string text, string suffix, bool ignore)
        {
            if (suffix == null) throw new ArgumentNullException("suffix");
            if (text == null) return false;
            var comparison = ignore ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            return text.EndsWith(suffix, comparison);
        }

        /// <summary>
        /// Determines whether two string objects have the same value, using
        /// culture-invariant casing rules.
        /// </summary>
        /// <param name="a">The first string to compare.</param>
        /// <param name="b">The scond string to compare.</param>
        /// <param name="ignore">true to ignore case when comparing the strings;
        /// otherwise, false.</param>
        /// <returns>true if the value of a is the same as the value of b;
        /// otherwise, false.</returns>

        public static bool Equals(string a, string b, bool ignore)
        {
            return Compare(a, b, ignore) == 0;
        }

        /// <summary>
        /// Replaces each format item in the specified text string with the
        /// corresponding argument in the specified object array.
        /// </summary>

        public static string Format(string text, params object[] args)
        {
            return string.Format(CultureInfo.InvariantCulture, text, args);
        }

        /// <summary>
        /// Allocate a new GUID and format it as a string value.
        /// </summary>

        public static string CreateGuidString()
        {
            return Invariant.Format("guid:{0}", Guid.NewGuid().ToString("N"));
        }

        /// <summary>
        /// Removes the specified suffix, if present,
        /// from the specified text string.
        /// </summary>

        public static string LopSuffix(string text, bool ignore, params string[] suffixes)
        {
            foreach (var suffix in suffixes)
            {
                if (EndsWith(text, suffix, ignore))
                {
                    return text.Substring(0, text.Length - suffix.Length).Trim();
                }
            }
            return text;
        }

        /// <summary>
        /// Tests the specified file path to see
        /// if it has the specified extension.
        /// </summary>

        public static bool MatchExtension(string path, string extension)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            if (extension == null)
            {
                throw new ArgumentNullException("extension");
            }
            var ext = (extension.Length <= 0 || extension[0] != '.' ? "." : string.Empty) + extension;
            return Equals(Path.GetExtension(path), ext, true);
        }

        /// <summary>
        /// Converts the string representation of a number to its double-precision
        /// floating-point number equivalent, using culture-invariant rules.
        /// </summary>
        /// <param name="text">The text string to parse.</param>
        /// <returns>A double-precision floating-point number that is equivalent
        /// to the numeric value in the specified text string.</returns>
        /// <exception cref="ArgumentNullException">text is null.</exception>
        /// <exception cref="FormatException">text does not represent a number in
        /// a valid format.</exception>
        /// <exception cref="OverflowException">text represents a number that is
        /// less than MinValue or greater than MaxValue.</exception>

        public static double ParseDouble(string text)
        {
            return double.Parse(text, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the string representation of a number to its 32-bit signed
        /// integer equivalent, using culture-invariant rules.
        /// </summary>
        /// <param name="text">The text string to parse.</param>
        /// <returns>A 32-bit signed integer that is equivalent to the numeric
        /// value in the specified text string.</returns>
        /// <exception cref="ArgumentNullException">text is null.</exception>
        /// <exception cref="FormatException">text does not represent a number in
        /// a valid format.</exception>
        /// <exception cref="OverflowException">text represents a number that is
        /// less than MinValue or greater than MaxValue.</exception>

        [SuppressMessage("Microsoft.Naming", "CA1720")]
        public static int ParseInt(string text)
        {
            return int.Parse(text, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the string representation of a number to its 64-bit signed
        /// integer equivalent, using culture-invariant rules.
        /// </summary>
        /// <param name="text">The text string to parse.</param>
        /// <returns>A 64-bit signed integer that is equivalent to the numeric
        /// value in the specified text string.</returns>
        /// <exception cref="ArgumentNullException">text is null.</exception>
        /// <exception cref="FormatException">text does not represent a number in
        /// a valid format.</exception>
        /// <exception cref="OverflowException">text represents a number that is
        /// less than MinValue or greater than MaxValue.</exception>

        [SuppressMessage("Microsoft.Naming", "CA1720")]
        public static long ParseLong(string text)
        {
            return long.Parse(text, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Determines whether the beginning of the specified text string matches
        /// the specified prefix, using culture-invariant casing rules.
        /// </summary>
        /// <param name="text">The text string to inspect.</param>
        /// <param name="prefix">The string prefix to seek.</param>
        /// <param name="ignore">true to ignore case when comparing the text and
        /// prefix; otherwise, false.</param>
        /// <returns>true if prefix matches the beginning of this string;
        /// otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">prefix is null.</exception>

        public static bool StartsWith(string text, string prefix, bool ignore)
        {
            if (prefix == null) throw new ArgumentNullException("prefix");
            if (text == null) return false;
            var comparison = ignore ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            return text.StartsWith(prefix, comparison);
        }

        /// <summary>
        /// Returns a string that converts the specified DateTime value to
        /// a date string formatted according to the FCS 3.1 specification.
        /// </summary>
        /// <param name="value">The DateTime value.</param>
        /// <returns>The resulting date string.</returns>

        public static string ToFcsDate(DateTime value)
        {
            return ToUpper(Format("{0:dd}-{0:MMM}-{0:yyyy}", value));
        }

        /// <summary>
        /// Returns a string that converts the specified DateTime value to
        /// a time string formatted according to the FCS 3.1 specification.
        /// </summary>
        /// <param name="value">The DateTime value.</param>
        /// <returns>The resulting time string.</returns>

        public static string ToFcsTime(DateTime value)
        {
            var f = (int)Math.Round(value.Millisecond / 10.0, 0, MidpointRounding.AwayFromZero);
            return Format("{0:HH}:{0:mm}:{0:ss}.{1:D2}", value, f);
        }

        /// <summary>
        /// Returns a string that contains the contents of the input buffer
        /// represented as hexadecimal digits.
        /// </summary>
        /// <param name="buffer">The input buffer.</param>
        /// <returns>The resulting hexadecimal string.</returns>
        /// <remarks>
        /// <para>This method is useful for converting the hash value created
        /// by HashAlgorithm.ComputeHash into a text string.</para>
        /// </remarks>

        public static string ToHexString(byte[] buffer)
        {
            var text = new StringBuilder(buffer.Length * 2);
            foreach (var value in buffer)
            {
                text.AppendFormat("{0:X2}", value);
            }
            return text.ToString();
        }

        /// <summary>
        /// Returns a copy of the specified text string converted to lowercase,
        /// using culture-invariant casing rules.
        /// </summary>
        /// <param name="text">The text string to convert to lowercase.</param>
        /// <returns>The text string, converted to lowercase.</returns>
        /// <exception cref="ArgumentNullException">text is null.</exception>

        [SuppressMessage("Microsoft.Globalization", "CA1308")]
        public static string ToLower(string text)
        {
            if (text == null) throw new ArgumentNullException("text");
            return text.ToLower(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the numeric value to its string representation, using
        /// culture-invariant rules.
        /// </summary>
        /// <param name="value">The numeric value to convert.</param>
        /// <returns>The string representation of the value.</returns>

        public static string ToString(decimal value)
        {
            // Decimal.ToString outputs trailing zeroes by default.
            // http://msdn.microsoft.com/en-us/library/dwhawy9k.aspx#GFormatString
            //
            // This is a nifty trick to get rid of them.
            // http://stackoverflow.com/questions/4525854/remove-trailing-zeros/7983330#7983330

            return (value / 1.000000000000000000000000000000000m).ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the numeric value to its string representation, using
        /// culture-invariant rules.
        /// </summary>
        /// <param name="value">The numeric value to convert.</param>
        /// <returns>The string representation of the value.</returns>

        public static string ToString(double value)
        {
            return value.ToString("R", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the numeric value to its string representation, using
        /// culture-invariant rules.
        /// </summary>
        /// <param name="value">The numeric value to convert.</param>
        /// <returns>The string representation of the value.</returns>

        public static string ToString(int value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the numeric value to its string representation, using
        /// culture-invariant rules.
        /// </summary>
        /// <param name="value">The numeric value to convert.</param>
        /// <returns>The string representation of the value.</returns>

        public static string ToString(long value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns a copy of the specified text string converted to uppercase,
        /// using culture-invariant casing rules.
        /// </summary>
        /// <param name="text">The text string to convert to uppercase.</param>
        /// <returns>The text string, converted to uppercase.</returns>
        /// <exception cref="ArgumentNullException">text is null.</exception>

        public static string ToUpper(string text)
        {
            if (text == null) throw new ArgumentNullException("text");
            return text.ToUpper(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Try to parse a string into a double value.
        /// </summary>
        /// <param name="text">The string.</param>
        /// <param name="value">The double value.</param>
        /// <returns>true if the string is parsed successfully, or false
        /// otherwise.</returns>

        public static bool TryParse(string text, out double value)
        {
            return double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out value);
        }

        /// <summary>
        /// Try to parse a string into an integer value.
        /// </summary>
        /// <param name="text">The string.</param>
        /// <param name="value">The integer value.</param>
        /// <returns>true if the string is parsed successfully, or false
        /// otherwise.</returns>

        public static bool TryParse(string text, out int value)
        {
            const NumberStyles style =
              NumberStyles.AllowExponent |
              NumberStyles.AllowLeadingSign |
              NumberStyles.AllowLeadingWhite |
              NumberStyles.AllowThousands |
              NumberStyles.AllowTrailingSign |
              NumberStyles.AllowTrailingWhite;
            return int.TryParse(text, style, CultureInfo.InvariantCulture, out value);
        }

        /// <summary>
        /// Try to parse a string into a long value.
        /// </summary>
        /// <param name="text">The string.</param>
        /// <param name="value">The long value.</param>
        /// <returns>true if the string is parsed successfully, or false
        /// otherwise.</returns>

        public static bool TryParse(string text, out long value)
        {
            return long.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out value);
        }
    }
}
