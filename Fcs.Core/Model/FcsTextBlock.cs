using System;
using System.Collections.Generic;

namespace Fcs.Core
{
    /// <summary>
    /// Parses an FCS text block.
    /// </summary>
    /// <remarks>
    /// <para>An FCS text block is a long delimited string of name/value pairs
    /// as described in section 3.2 of the FCS 3.0 file specification. Briefly,
    /// the first character of the string defines the delimiter, delimiters are
    /// quoted by doubling them, and no value may be zero length. Names are
    /// case-insensitive for comparison, but case should be preserved.</para>
    /// <para>The FCS text block may appear twice in an FCS data set, once for
    /// the primary text segment, and again for the secondary text segment. It
    /// also appears several times in a CXP protocol file (Statistics, Gates,
    /// Regions, Setup.c00).</para>
    /// </remarks>
    public class FcsTextBlock
    {
        private FcsTextBlockDictionary _dictionary;
        private List<string> _vector;

        /// <summary>
        /// </summary>

        public IDictionary<string, string> Dictionary
        {
            get { return _dictionary; }
        }

        /// <summary>
        /// Initializes a new instance of the TextBlock class that is empty.
        /// </summary>

        public FcsTextBlock()
        {
            _dictionary = new FcsTextBlockDictionary();
        }

        /// <summary>
        /// Initializes a new instance of the TextBlock class by parsing the
        /// specified text.
        /// </summary>
        /// <param name="text">A string in FCS-style delimited text format.
        /// </param>
        /// <param name="utf8">If true, then keyword values will be parsed
        /// as UTF-8 text.</param>
        public FcsTextBlock(byte[] text, bool utf8)
        {
            _dictionary = new FcsTextBlockDictionary(text, utf8);
        }

        /// <summary>
        /// Removes all keywords and values from the TextBlock.
        /// </summary>

        public void Clear()
        {
            _dictionary.Reset();
            _vector = null;
        }

        /// <summary>
        /// Remove all keywords and values from the TextBlock and load the text
        /// block with new values by parsing the specified text.
        /// </summary>
        /// <param name="text">A string in FCS-style delimited text format.
        /// </param>
        /// <param name="utf8">If true, then keyword values will be parsed
        /// as UTF-8 text.</param>

        public void Reset(byte[] text, bool utf8)
        {
            _dictionary.Reset(text, utf8);
            _vector = null;
        }

        /// <summary>
        /// Preserves all keywords and values from the TextBlock. Parses the
        /// specified text and adds them into the TextBlock, replacing any
        /// existing keywords with the new values in the specified text.
        /// </summary>
        /// <param name="text">A string in FCS-style delimited text format.
        /// </param>
        /// <param name="utf8">If true, then keyword values will be parsed
        /// as UTF-8 text.</param>

        public void Merge(byte[] text, bool utf8)
        {
            var merge = new FcsTextBlockDictionary(text, utf8);
            foreach (var pair in merge)
            {
                _dictionary[pair.Key] = pair.Value;
            }
            _vector = null;
        }

        /// <summary>
        /// Gets the number of keyword/value pairs contained in the TextBlock.
        /// </summary>
        /// <value>The number of keyword/value pairs contained in the TextBlock.
        /// </value>

        public int Count
        {
            get
            {
                return _dictionary.Count;
            }
        }

        /// <summary>
        /// Gets the keyword at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the keyword to get.
        /// </param>
        /// <returns>The keyword at the specified index.</returns>
        /// <exception cref="ArgumentOutOfRangeException">index is less than
        /// 0, or index is equal to or greater than Count.</exception>

        public string GetKeyword(int index)
        {
            if (_vector == null) LoadVector();
            return _vector[index];
        }

        /// <summary>
        /// Determines whether the TextBlock contains the specified keyword.
        /// </summary>
        /// <param name="keyword">The keyword to locate in the TextBlock.</param>
        /// <returns>true if the TextBlock contains an element with the specified
        /// keyword; otherwise, false.</returns>

        public bool ContainsKeyword(string keyword)
        {
            return _dictionary.ContainsKey(keyword);
        }

        /// <summary>
        /// Gets the value associated with the specified keyword.
        /// </summary>
        /// <param name="keyword">The keyword of the value to get.</param>
        /// <returns>The value associated with the specified keyword. If the
        /// specified keyword is not found, KeyNotFoundException is thrown.
        /// </returns>
        /// <exception cref="ArgumentNullException">keyword is null.</exception>
        /// <exception cref="KeyNotFoundException">The property is retrieved
        /// and keyword does not exist in the collection.</exception>
        /// <remarks>
        /// <para>The C# language uses the this keyword to define the indexers
        /// instead of implementing the Item property.</para>
        /// </remarks>

        public string this[string keyword]
        {
            get
            {
                return _dictionary[keyword];
            }
        }

        /// <summary>
        /// Gets the value associated with the specified keyword.
        /// </summary>
        /// <param name="keyword">The keyword of the value to get.</param>
        /// <param name="value">When this method returns, contains the value
        /// associated with the specified keyword, if the keyword is found;
        /// otherwise, an empty string.</param>
        /// <returns>true if the TextBlock contains an element with the
        /// specified key; otherwise, false.</returns>
        /// <remarks>
        /// <para>This method combines the functionality of the ContainsKeyword
        /// method and the Item property.</para>
        /// <para>If the keyword is not found, then the value parameter gets the
        /// empty string.</para>
        /// <para>Use the TryGetValue method if your code frequently attempts to
        /// access keywords that are not in the text block. Using this method is
        /// more efficient than catching the KeyNotFoundException thrown by the
        /// Item property.</para>
        /// </remarks>

        public bool TryGetValue(string keyword, out string value)
        {
            return _dictionary.TryGetValue(keyword, out value);
        }

        /// <summary>
        /// Gets the value associated with the specified keyword. If the keyword
        /// does not exist, return null.
        /// </summary>
        /// <param name="keyword">The keyword of the value to get.</param>
        /// <returns>The value associated with the specified keyword. If the
        /// specified keyword is not found, then null is returned.
        /// </returns>

        public string Query(string keyword)
        {
            string value;
            if (!_dictionary.TryGetValue(keyword, out value)) return null;
            return value;
        }

        /// <summary>
        /// Gets the value associated with the specified keyword. If the keyword
        /// does not exist, return a default value instead.
        /// </summary>
        /// <param name="keyword">The keyword of the value to get.</param>
        /// <param name="def">The default value to use if the keyword does not
        /// exist.</param>
        /// <returns>The value associated with the specified keyword. If the
        /// specified keyword is not found, the default value is returned.
        /// </returns>
        /// <exception cref="ArgumentNullException">keyword or def is null.
        /// </exception>

        public string Query(string keyword, string def)
        {
            if (def == null) throw new ArgumentNullException("def");
            string value;
            if (!_dictionary.TryGetValue(keyword, out value)) return def;
            return value;
        }

        /// <summary>
        /// Gets the value associated with the specified keyword. If the keyword
        /// does not exist or if the value of the keyword is not a valid integer,
        /// return a default value instead.
        /// </summary>
        /// <param name="keyword">The keyword of the value to get.</param>
        /// <param name="def">The default integer value to use if the keyword
        /// does not exist or is not a valid integer.</param>
        /// <returns>The value associated with the specified keyword. If the
        /// specified keyword is not found or is not a valid integer, the default
        /// value is returned.
        /// </returns>
        /// <exception cref="ArgumentNullException">keyword is null.</exception>

        public int Query(string keyword, int def)
        {
            string text;
            if (!_dictionary.TryGetValue(keyword, out text)) return def;

            int n;
            if (!Invariant.TryParse(text, out n)) return def;

            return n;
        }

        /// <summary>
        /// Gets the value associated with the specified keyword. If the keyword
        /// does not exist or if the value of the keyword is not a valid long,
        /// return a default value instead.
        /// </summary>
        /// <param name="keyword">The keyword of the value to get.</param>
        /// <param name="def">The default long value to use if the keyword
        /// does not exist or is not a valid integer.</param>
        /// <returns>The value associated with the specified keyword. If the
        /// specified keyword is not found or is not a valid long, the default
        /// value is returned.
        /// </returns>
        /// <exception cref="ArgumentNullException">keyword is null.</exception>

        public long Query(string keyword, long def)
        {
            string text;
            if (!_dictionary.TryGetValue(keyword, out text)) return def;

            long n;
            if (!Invariant.TryParse(text, out n)) return def;

            return n;
        }

        private void LoadVector()
        {
            _vector = new List<string>();
            foreach (var pair in _dictionary)
            {
                _vector.Add(pair.Key);
            }
            _vector.Sort();
        }
    }
}
