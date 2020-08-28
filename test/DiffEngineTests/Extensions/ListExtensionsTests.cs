using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using LoxSmoke.DiffEngine.Extensions;

namespace DiffEngineTests.Extensions
{
    public class ListExtensionsTests
    {
        public static List<int> ParseList(string text)
        {
            if (string.IsNullOrEmpty(text)) return new List<int>();
            return text.Split(',').Select(s => int.Parse(s)).ToList();
        }

        [Theory]
        [InlineData("", "4,5,6", 0)] // Null case.
        [InlineData("1,2,3", "4,5,6", 0)] // Null case.
        [InlineData("1,2,3,4,91,92,93,94,95", "1,2,3,4,88,87,86", 4)] // Non-null case.
        [InlineData("1,2,3,4", "1,2,3,4,88,87,88", 4)] // Whole case.
        public void CommonPrefix(string listText1, string listText2, int expected)
        {
            var list1 = ParseList(listText1);
            var list2 = ParseList(listText2);
            var result = list1.CommonPrefix(list2);
            Assert.Equal(expected, result.Count);
        }

        [Theory]
        [InlineData("", "9,8,7", 0)] // Null case.
        [InlineData("1,2,3", "9,8,7", 0)] // Null case.
        [InlineData("99,98,97,96,95,94,1,2,3,4", "77,76,75,1,2,3,4", 4)] // Non-null case.
        [InlineData("1,2,3,4", "77,76,75,1,2,3,4", 4)] // Whole case.
        public void CommonSuffix(string listText1, string listText2, int expected)
        {
            var list1 = ParseList(listText1);
            var list2 = ParseList(listText2);
            var calculated = list1.CommonSuffix(list2);
            Assert.Equal(expected, calculated.Count);
        }

        [Theory]
        [InlineData("", "1,2,3,4", 0)] // Null case.
        [InlineData("1,2,3", "1,2,3,4", 3)] // Whole case 1.
        [InlineData("0,1,2,3", "1,2,3", 3)] // Whole case 2.
        [InlineData("1,2,3,4,5,6", "91,92,93,94", 0)] // No overlap.
        [InlineData("1,2,3,4,5,6,0,0,0", "0,0,0,99,98,97,96", 3)] // End with start overlap.
        [InlineData("1,2,3,4,5,6,0,2,1", "0,2,1,99,98,97,96", 3)] // End with start overlap.
        public void CommonOverlapLength(string listText1, string listText2, int expected)
        {
            var list1 = ParseList(listText1);
            var list2 = ParseList(listText2);
            var calculated = list1.CommonOverlapLength(list2);
            Assert.Equal(expected, calculated);
        }

        [Theory]
        [InlineData("", "", "")] // Two empty
        [InlineData("", "1,2,3,4", "1,2,3,4")] // Empty+not empty
        [InlineData("1,2,3,4", "", "1,2,3,4")] // Not empty+empty
        [InlineData("1,2,3", "1,2,3", "1,2,3,1,2,3")] // Two lists
        public void Concat(string listText1, string listText2, string expectedListText)
        {
            var list1 = ParseList(listText1);
            var list2 = ParseList(listText2);
            var expectedList = ParseList(expectedListText);
            var result = list1.Concat(list2);
            Assert.Equal(expectedList, result);
        }

        [Theory]
        [InlineData("", "", true)] // Two empty
        [InlineData("1,2,3,4", "", true)] // Empty in not empty
        [InlineData("", "1,2,3,4", false)] // Not empty in empty
        [InlineData("1,2,3", "1,2,3", true)] // Equal lists
        [InlineData("1,2,3,4,5", "1,2,3", true)] // Short list in long list
        [InlineData("1,2", "1,2,3", false)] // Long list in short list
        [InlineData("1,2,3,6,7", "1,2,3,4,5", false)] // Matching starts
        [InlineData("1,2,3", "4,5,6", false)] // Full mismatch
        public void StartsWith(string listText1, string listText2, bool expectedResult)
        {
            var list1 = ParseList(listText1);
            var list2 = ParseList(listText2);
            var result = list1.StartsWith(list2);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("", "", true)] // Two empty
        [InlineData("1,2,3,4", "", true)] // Empty in not empty
        [InlineData("", "1,2,3,4", false)] // Not empty in empty
        [InlineData("1,2,3", "1,2,3", true)] // Equal lists
        [InlineData("1,2,3,4,5", "3,4,5", true)] // Short list in long list
        [InlineData("1,2", "0,1,2", false)] // Long list in short list
        [InlineData("3,2,1", "4,3,2,1", false)] // Matching starts
        [InlineData("1,2,3", "4,5,6", false)] // Full mismatch
        public void EndsWith(string listText1, string listText2, bool expectedResult)
        {
            var list1 = ParseList(listText1);
            var list2 = ParseList(listText2);
            var result = list1.EndsWith(list2);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("", 0, "")]
        [InlineData("1,2,3", 0, "")]
        [InlineData("1,2,3", 2, "1,2")]
        [InlineData("1,2,3", 3, "1,2,3")]
        public void Left(string listText, int count, string expectedListText)
        {
            var list = ParseList(listText);
            var expectedResult = ParseList(expectedListText);
            var result = list.Left(count);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("", 0, "")]
        [InlineData("1,2,3", 0, "")]
        [InlineData("1,2,3", 2, "2,3")]
        [InlineData("1,2,3", 3, "1,2,3")]
        public void Right(string listText, int count, string expectedListText)
        {
            var list = ParseList(listText);
            var expectedResult = ParseList(expectedListText);
            var result = list.Right(count);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("", "", 0, -1)]
        [InlineData("1,2,3,4", "1", 0, 0)]
        [InlineData("1,2,3,4", "1", 1, -1)]
        [InlineData("1,2,3,4", "1,2,3", 0, 0)]
        [InlineData("1,2,3,4", "3,4", 0, 2)]
        [InlineData("1,2,3,4", "", 0, -1)]
        [InlineData("1,2,3,4", "1,2,3,4,5", 0, -1)]
        public void IndexOf(string listText, string otherListText, int startIndex, int expectedResult)
        {
            var list = ParseList(listText);
            var otherList = ParseList(otherListText);
            var result = list.IndexOf(otherList, startIndex);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("", 0, 0, "", 0, 0, -1)]
        [InlineData("1,2,3,4", 0, 2, "1", 0, 1, 0)]
        [InlineData("1,2,3,4", 1,3, "1", 0, 1, -1)]
        [InlineData("1,2,3,4", 0,3, "1,2,3", 0, 3, 0)]
        [InlineData("1,2,3,4", 0,3, "1,2,3", 1, 2, 1)]
        [InlineData("1,2,3,4", 0,4, "3,4", 0, 2, 2)]
        [InlineData("1,2,3,4", 0,4, "1,2,3,4,5", 0, 5,-1)]
        [InlineData("1,2,3,4", 0,4, "1,2,3,4,5", 0, 4, 0)]
        public void IndexOf_PartialList(string listText, int start, int count,
            string otherListText, int otherListStart, int otherListLen, int expectedResult)
        {
            var list = ParseList(listText);
            var otherList = ParseList(otherListText);
            var result = list.IndexOf(start, count, otherList, otherListStart, otherListLen);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("", 0,0, "", 0, true)]
        [InlineData("1,2,3", 1,1, "2", 0, true)]
        [InlineData("1,2,3", 0,1, "2", 0, false)]
        public void FragmentEquals(string listText, int start, int count, string otherListText, int otherListStart, bool expectedResult)
        {
            var list = ParseList(listText);
            var otherList = ParseList(otherListText);
            var result = list.FragmentEquals(start, count, otherList, otherListStart);
            Assert.Equal(expectedResult, result);
        }
    }
}
