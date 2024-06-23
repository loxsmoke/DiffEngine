using System;
using System.Collections.Generic;
using System.Text;

#pragma warning disable CS1591

namespace LoxSmoke.DiffEngine.Extensions
{
    /// <summary>
    /// Generic list extension methods.
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Find and return the common prefix of the two lists.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">First list</param>
        /// <param name="otherList">Second list</param>
        /// <returns>The common starting sequence of elements</returns>
        public static List<T> CommonPrefix<T>(this List<T> list, List<T> otherList)
        {
            var n = Math.Min(list.Count, otherList.Count);
            for (var i = 0; i < n; i++)
            {
                if (!list[i].Equals(otherList[i])) return list.Left(i);
            }
            return otherList.Left(n);
        }

        /// <summary>
        /// Find and return the common suffix of the two lists.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">First list</param>
        /// <param name="otherList">Second list</param>
        /// <returns>The common ending sequence of elements</returns>
        public static List<T> CommonSuffix<T>(this List<T> list, List<T> otherList)
        {
            var listLength = list.Count;
            var otherLength = otherList.Count;
            var n = Math.Min(list.Count, otherList.Count);
            for (var i = 1; i <= n; i++)
            {
                if (!list[listLength - i].Equals(otherList[otherLength - i]))
                {
                    return otherList.Right(i - 1);
                }
            }
            return list.Right(n);
        }

        /// <summary>
        /// Return the concatenation of the two lists.
        /// If one of the lists is empty then method does not create the copy of the list but 
        /// returns one that is not empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">First list</param>
        /// <param name="otherList">Second list</param>
        /// <returns>The concatenated list or one of the non-empty lists.</returns>
        public static List<T> Concat<T>(this List<T> list, List<T> otherList)
        {
            if (otherList is null || otherList.Count == 0) return list;
            if (list.Count == 0) return otherList;
            var newList = new List<T>(list.Count + otherList.Count);
            newList.AddRange(list);
            newList.AddRange(otherList);
            return newList;
        }

        /// <summary>
        /// Check if the list ends with the other list. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="otherList"></param>
        /// <returns>True if list ends with other list or false otherwise.</returns>
        public static bool EndsWith<T>(this List<T> list, List<T> otherList)
        {
            if (list.Count < otherList.Count) return false;
            return list.FragmentEquals(list.Count - otherList.Count, otherList.Count, otherList, 0);
        }

        /// <summary>
        /// Get the specified number of the items at the start of the list.
        /// Function does not modify the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="count">The number of elements to return.</param>
        /// <returns>The list with the cpecified number of elemets from the original list.</returns>
        public static List<T> Left<T>(this List<T> list, int count)
        {
            return list.GetRange(0, count);
        }

        /// <summary>
        /// Get the specified number of the items at the end of the list.
        /// Function does not modify the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="count">The number of elements to return.</param>
        /// <returns>The list with the cpecified number of elemets from the original list.</returns>
        public static List<T> Right<T>(this List<T> list, int count)
        {
            return list.GetRange(list.Count - count, count);
        }

        /// <summary>
        /// Get the common overlap length of two lists.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        /// <returns></returns>
        public static int CommonOverlapLength<T>(this List<T> list1, List<T> list2)
        {
            if (list1.Count == 0 || list2.Count == 0)
            {
                return 0;
            }

            // Ignore the start of the longer list1
            // or the end of the longer list 2
            var startList1 = 0;
            var listLength = Math.Min(list1.Count, list2.Count);
            if (list1.Count > listLength)
            {
                startList1 = list1.Count - listLength;
            }

            // Quick check for simple equality
            if (list1.FragmentEquals(startList1, listLength, list2, 0))
            {
                return listLength;
            }

            // Start by looking for a single character match
            // and increase length until no match is found.
            // Performance analysis: https://neil.fraser.name/news/2010/11/04/
            var best = 0;
            var patternLength = 1;
            while (true)
            {
                var found = list2.IndexOf(0, listLength, list1, startList1 + listLength - patternLength, patternLength);
                if (found == -1)
                {
                    return best;
                }
                patternLength += found;
                if (found == 0 ||
                    list2.FragmentEquals(0, patternLength, list1, startList1 + listLength - patternLength))
                {
                    best = patternLength;
                    patternLength++;
                }
            }
        }

        /// <summary>
        /// Find the specified sequence of the items in the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">List of the elements to look in.</param>
        /// <param name="patternList">The list of items to look for</param>
        /// <param name="startIndex">The 0-based index to start the search from. Negative values are treated as 0</param>
        /// <returns>
        /// The index where the specified sequence starts in the list.
        /// -1 if specified sequence was not found in the list.
        /// </returns>
        public static int IndexOf<T>(this List<T> list, List<T> patternList, int startIndex = 0)
        {
            if (list.Count < patternList.Count ||
                patternList.Count == 0) return -1;

            for (var i = Math.Max(0, startIndex); i <= list.Count - patternList.Count; i++)
            {
                if (list.FragmentEquals(i, patternList.Count, patternList, 0)) return i;
            }
            return -1;
        }

        /// <summary>
        /// Find the specified sequence of the items in the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="start"></param>
        /// <param name="len"></param>
        /// <param name="patternList"></param>
        /// <param name="patternStart"></param>
        /// <param name="patternLen"></param>
        /// <returns>
        /// If pattern was found then the index of the pattern relative to the specified start index.
        /// If pattern was not found then -1.
        /// </returns>
        public static int IndexOf<T>(this List<T> list, int start, int len,
            List<T> patternList, int patternStart, int patternLen)
        {
            if (len < patternLen || patternLen == 0) return -1;

            for (var i = start; i <= start + len - patternLen; i++)
            {
                if (list.FragmentEquals(i, patternLen, patternList, patternStart)) return i - start;
            }
            return -1;
        }

        /// <summary>
        /// Determines whether the beginning of the list matches a specified list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="otherList"></param>
        /// <returns>True if value matches the beginning of this list; otherwise, false.</returns>
        public static bool StartsWith<T>(this List<T> list, List<T> otherList)
        {
            if (list.Count < otherList.Count) return false;
            return list.FragmentEquals(0, otherList.Count, otherList, 0);
        }

        /// <summary>
        /// Check if the part of the list equals to the part of another list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The first list</param>
        /// <param name="from">The start of the data to be compared.</param>
        /// <param name="len">The number of elements to compare.</param>
        /// <param name="otherList">The other list</param>
        /// <param name="otherListStart">The start position in another list</param>
        /// <returns></returns>
        public static bool FragmentEquals<T>(this List<T> list, int from, int len, List<T> otherList, int otherListStart)
        {
            for (var i = 0; i < len; i++)
            {
                if (!list[from + i].Equals(otherList[otherListStart + i])) return false;
            }
            return true;
        }
    }
}
