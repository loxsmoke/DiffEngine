using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace LoxSmoke.DiffEngine.Extensions
{
    /// <summary>
    /// String extension methods.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Get the specified number of the leftmost characters of the string.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Left(this string text, int length)
        {
            return text.Substring(0, length);
        }

        /// <summary>
        /// Get the specified number of the rightmost characters of the string.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Right(this string text, int length)
        {
            return text.Substring(text.Length - length);
        }

        /// <summary>
        /// Determine the common prefix of two strings.
        /// </summary>
        /// <param name="text1">First string.</param>
        /// <param name="text2">Second string.</param>
        /// <returns>The number of characters common to the start of each string.</returns>
        public static string CommonPrefix(this string text1, string text2)
        {
            var n = Math.Min(text1.Length, text2.Length);
            for (var i = 0; i < n; i++)
            {
                if (text1[i] != text2[i]) return text1.Left(i);
            }
            return text1.Left(n);
        }

        /// <summary>
        /// Determine the common suffix of two strings.
        /// </summary>
        /// <param name="text1">First string.</param>
        /// <param name="text2">Second string.</param>
        /// <returns>The number of characters common to the end of each string.</returns>
        public static string CommonSuffix(this string text1, string text2)
        {
            var length1 = text1.Length;
            var length2 = text2.Length;
            var n = Math.Min(text1.Length, text2.Length);
            for (var i = 1; i <= n; i++)
            {
                if (text1[length1 - i] != text2[length2 - i])
                {
                    return text1.Right(i - 1);
                }
            }
            return text1.Right(n);
        }
    }
}
