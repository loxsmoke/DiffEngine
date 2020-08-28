using LoxSmoke.DiffEngine.Extensions;
using LoxSmoke.DiffEngine.Interfaces;
using System;

namespace LoxSmoke.DiffEngine.Sequences.String
{
    /// <summary>
    /// The sequence of characters also known as string.
    /// </summary>
    public class CharSequence : IItemSequence<CharSequence>
    {
        /// <summary>
        /// The text.
        /// </summary>
        public string Text;

        /// <summary>
        /// Create the new CharSequence.
        /// </summary>
        /// <param name="text"></param>
        public CharSequence(string text)
        {
            this.Text = text;
        }

        /// <summary>
        /// Get the text length.
        /// </summary>
        public int Length => Text.Length;

        /// <inheritdoc/>
        public bool StartsWith(CharSequence sequence)
        {
            return Text.StartsWith(sequence.Text, StringComparison.Ordinal);
        }

        /// <inheritdoc/>
        public bool EndsWith(CharSequence sequence)
        {
            return Text.EndsWith(sequence.Text, StringComparison.Ordinal);
        }

        /// <inheritdoc/>
        public int IndexOf(CharSequence sequence, int startFrom)
        {
            return Text.IndexOf(sequence.Text, startFrom, StringComparison.Ordinal);
        }

        /// <summary>
        /// Determine if the suffix of this string is the prefix of sequence.
        /// </summary>
        /// <param name="sequence"></param>
        /// <returns>The number of characters common to the end of the first
        /// string and the start of the second string.</returns>
        public int CommonOverlapLength(CharSequence sequence)
        {
            var text1 = Text;
            var text2 = sequence.Text;
            // Eliminate the null case.
            if (string.IsNullOrEmpty(text1) || string.IsNullOrEmpty(text2))
            {
                return 0;
            }

            // Truncate the longer string.
            var textLength = Math.Min(text1.Length, text2.Length);
            if (text1.Length > textLength)
            {
                text1 = text1.Substring(text1.Length - textLength);
            }
            else if (textLength < text2.Length)
            {
                text2 = text2.Left(textLength);
            }
            // Quick check for exact match
            if (text1 == text2)
            {
                return textLength;
            }

            // Start by looking for a single character match
            // and increase length until no match is found.
            // Performance analysis: https://neil.fraser.name/news/2010/11/04/
            var best = 0;
            var patternLength = 1;
            while (true)
            {
                var pattern = text1.Substring(textLength - patternLength);
                int found = text2.IndexOf(pattern, StringComparison.Ordinal);
                if (found < 0) // Not found
                {
                    return best;
                }
                patternLength += found;
                if (found == 0 ||
                    text1.Substring(textLength - patternLength) == text2.Left(patternLength))
                {
                    best = patternLength;
                    patternLength++;
                }
            }
        }

        /// <inheritdoc/>
        public CharSequence Mid(int from, int length)
        {
            return new CharSequence(Text.Substring(from, length));
        }

        /// <inheritdoc/>
        public CharSequence Concat(CharSequence other)
        {
            if (other == null) return this;
            return new CharSequence(Text + other.Text);
        }

        /// <inheritdoc/>
        public CharSequence CommonPrefix(CharSequence sequence)
        {
            return new CharSequence(Text.CommonPrefix(sequence.Text));
        }

        /// <inheritdoc/>
        public CharSequence CommonSuffix(CharSequence sequence)
        {
            return new CharSequence(Text.CommonSuffix(sequence.Text));
        }

        /// <summary>
        /// Determines whether two object instances are equal.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as CharSequence);
        }

        /// <summary>
        /// Determines whether two object instances are equal.
        /// </summary>
        /// <param name="sequence">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public bool Equals(CharSequence sequence)
        {
            return (sequence != null &&
                (sequence.Text == null && Text == null ||
                sequence.Text != null && Text != null && Text == sequence.Text));
        }
        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return Text?.GetHashCode() ?? 0;
        }

        /// <inheritdoc/>
        public bool ItemEquals(int index, CharSequence otherSequence, int otherIndex)
        {
            return Text[index] == otherSequence.Text[otherIndex];
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return Text;
        }
    }
}
