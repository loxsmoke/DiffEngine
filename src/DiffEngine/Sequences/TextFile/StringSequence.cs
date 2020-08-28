using LoxSmoke.DiffEngine.Extensions;
using LoxSmoke.DiffEngine.Sequences.Generic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LoxSmoke.DiffEngine.Sequences.TextFile
{
    /// <summary>
    /// The sequence of strings. 
    /// Represents the text file where each element is the line of the file.
    /// </summary>
    public class StringSequence :
        ItemList<StringSequence, int>
    {
        /// <summary>
        /// The list of unique text strings. Index of the string in the list is used as a pseudo-hash code.
        /// </summary>
        public List<string> UniqueLines;

        /// <summary>
        /// Get all the text lines of this sequence. 
        /// </summary>
        public IEnumerable<string> Lines => Data.Select(index => UniqueLines[index]);

        /// <summary>
        /// Create the sequence of strings.
        /// </summary>
        /// <param name="lineHashes">The list of pseudo-hashes. Each item in the list is the index of the string in the uniqueLines list.</param>
        /// <param name="uniqueLines">The list of unique strings.</param>
        public StringSequence(List<int> lineHashes, List<string> uniqueLines)
        {
            this.Data = lineHashes;
            this.UniqueLines = uniqueLines;
        }

        /// <summary>
        /// Clone this object and replace the list of line hashes with the new list.
        /// </summary>
        /// <param name="newLineHashes">The new list of line hashes</param>
        /// <returns>The clone</returns>
        protected override StringSequence CloneWith(List<int> newLineHashes)
        {
            var clone = base.CloneWith(newLineHashes) as StringSequence;
            clone.UniqueLines = UniqueLines;
            return clone;
        }

        /// <summary>
        /// Return the concatenation of two sequences.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>The combined sequence</returns>
        public override StringSequence Concat(StringSequence other)
        {
            if (other == null || other.Length == 0) return this;
            if (Length == 0) return other;
            return base.Concat(other);
        }

        /// <summary>
        /// Determines whether two object instances are equal.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as StringSequence);
        }

        /// <summary>
        /// Determines whether two object instances are equal.
        /// </summary>
        /// <param name="other">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(StringSequence other)
        {
            if (other == null) return false;
            if (other.UniqueLines != UniqueLines) return false;
            if (other.Data == null && Data == null) return true;
            if ((other.Data == null) != (Data == null)) return false;
            if (Data.Count != other.Data.Count) return false;
            return Data.SequenceEqual(other.Data);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            if (UniqueLines == null || Data == null) return 0;
            return UniqueLines.GetHashCode() ^ Data.GetHashCode();
        }

        /// <summary>
        /// Load two text files as string sequences for comparison.
        /// String sequences for both files share the same unique string list.
        /// </summary>
        /// <param name="fileName1">The first file</param>
        /// <param name="fileName2">The second file</param>
        /// <returns>Two string sequences</returns>
        public static (StringSequence firstFile, StringSequence secondFile)
            Load(string fileName1, string fileName2)
        {
            return Load(File.ReadAllLines(fileName1), File.ReadAllLines(fileName2));
        }

        /// <summary>
        /// Load two string enumerations as string sequences for comparison.
        /// String sequences the same unique string list.
        /// </summary>
        /// <param name="lines1">The first string enumeration</param>
        /// <param name="lines2">The second string enumeration</param>
        /// <returns>Two string sequences</returns>
        public static (StringSequence firstFile, StringSequence secondFile)
            Load(IEnumerable<string> lines1, IEnumerable<string> lines2)
        {
            var allLineHashes = new Dictionary<string, int>();
            var allLines = new List<string>();

            var hashes1 = Load(allLineHashes, allLines, lines1);
            var hashes2 = Load(allLineHashes, allLines, lines2);
            return (new StringSequence(hashes1, allLines), new StringSequence(hashes2, allLines));
        }

        /// <summary>
        /// Read the list of lines, add unique lines to the dictionary and return the list of hashes.
        /// </summary>
        /// <param name="allLineHashes">Dictionary containing unique strings and their indexes in the allLines list.</param>
        /// <param name="allLines">The list of unique strings.</param>
        /// <param name="lines">The list of strings</param>
        /// <returns>The list of pseudo-hashes</returns>
        public static List<int> Load(
            Dictionary<string, int> allLineHashes,
            List<string> allLines,
            IEnumerable<string> lines)
        {
            var hashes = new List<int>();
            foreach (var line in lines)
            {
                if (!allLineHashes.TryGetValue(line, out var value))
                {
                    value = allLines.Count;
                    allLineHashes.Add(line, value);
                    allLines.Add(line);
                }
                hashes.Add(value);
            }
            return hashes;
        }
    }
}
