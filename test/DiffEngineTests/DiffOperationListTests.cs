using LoxSmoke.DiffEngine;
using LoxSmoke.DiffEngine.Extensions;
using LoxSmoke.DiffEngine.Sequences.String;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace DiffEngineTests
{
    public class DiffOperationListTests
    {
        #region Utility methods
        public static DiffOperation<CharSequence> ParseDiff(string text)
        {
            if (text.StartsWith("DELETE:")) return new CharSequence(text.Substring(7)).AsDelete();
            else if (text.StartsWith("INSERT:")) return new CharSequence(text.Substring(7)).AsInsert();
            else if (text.StartsWith("EQUAL:")) return new CharSequence(text.Substring(6)).AsEqual();
            return null;
        }

        public static DiffOperationList<CharSequence> ParseDiffList(string text)
        {
            if (text == null) return null;
            if (string.IsNullOrEmpty(text)) return new DiffOperationList<CharSequence>();
            var list = text.Split(',').Select(a => ParseDiff(a)).ToList();
            return new DiffOperationList<CharSequence>() { Diffs = list };
        }
        #endregion

        [Theory]
        [InlineData("", "", true)] // Empty
        [InlineData("", "EQUAL:a,DELETE:b,INSERT:c", false)] // One empty not equal
        [InlineData("EQUAL:a,DELETE:b,INSERT:c", "EQUAL:a,DELETE:b,INSERT:c", true)] // Equal Not empty
        [InlineData("EQUAL:a,DELETE:b,INSERT:c", "EQUAL:a,DELETE:b", false)] // Longer and shorter
        [InlineData("EQUAL:a,DELETE:b", "EQUAL:a,DELETE:b,INSERT:c", false)] // Shorter and longer
        public void ListEquals(string diffList, string otherDiffList, bool expectedIsEqual)
        {
            var list1 = ParseDiffList(diffList);
            var list2 = ParseDiffList(otherDiffList);
            var result = list1.Equals(list2);
            Assert.Equal(expectedIsEqual, result);
        }

        [Theory]
        [InlineData("EQUAL:a,DELETE:b,INSERT:c", "EQUAL:a,DELETE:b,INSERT:c")] // No change case.
        [InlineData("EQUAL:a,EQUAL:b,EQUAL:c", "EQUAL:abc")] // Merge equalities.
        [InlineData("DELETE:a,DELETE:b,DELETE:c", "DELETE:abc")] // Merge deletions.
        [InlineData("INSERT:a,INSERT:b,INSERT:c", "INSERT:abc")] // Merge insertions.
        [InlineData("DELETE:a,INSERT:b,DELETE:c,INSERT:d,EQUAL:e,EQUAL:f", "DELETE:ac,INSERT:bd,EQUAL:ef")] // Merge interweave.
        [InlineData("DELETE:a,INSERT:abc,DELETE:dc", "EQUAL:a,DELETE:d,INSERT:b,EQUAL:c")] // Prefix and suffix detection.
        [InlineData("EQUAL:x,DELETE:a,INSERT:abc,DELETE:dc,EQUAL:y", "EQUAL:xa,DELETE:d,INSERT:b,EQUAL:cy")] // Prefix and suffix detection with equalities.
        [InlineData("EQUAL:a,INSERT:ba,EQUAL:c", "INSERT:ab,EQUAL:ac")] // Slide edit left.
        [InlineData("EQUAL:c,INSERT:ab,EQUAL:a", "EQUAL:ca,INSERT:ba")] // Slide edit right.
        [InlineData("EQUAL:a,DELETE:b,EQUAL:c,DELETE:ac,EQUAL:x", "DELETE:abc,EQUAL:acx")] // Slide edit left recursive.
        [InlineData("EQUAL:x,DELETE:ca,EQUAL:c,DELETE:b,EQUAL:a", "EQUAL:xca,DELETE:cba")] // Slide edit right recursive.
        [InlineData("DELETE:b,INSERT:ab,EQUAL:c", "INSERT:a,EQUAL:bc")] // Empty merge.
        [InlineData("EQUAL:,INSERT:a,EQUAL:b", "INSERT:a,EQUAL:b")] // Empty equality.
        public void CleanupMerge(string diffList, string expectedDiffList)
        {
            var list = ParseDiffList(diffList);
            var expectedList = ParseDiffList(expectedDiffList);
            list.CleanupMerge();
            Assert.Equal(expectedList, list);
        }

        [Theory]
        [InlineData("DELETE:abc,INSERT:1234,EQUAL:xyz", 4)] // Trailing equality.
        [InlineData("EQUAL:xyz,DELETE:abc,INSERT:1234", 4)] // Leading equality.
        [InlineData("DELETE:abc,EQUAL:xyz,INSERT:1234", 7)] // Middle equality.
        public void LevenshteinDistance(string diffList, int expected)
        {
            var list = ParseDiffList(diffList);
            var result = list.LevenshteinDistance;
            Assert.Equal(expected, result);
        }
    }
}
