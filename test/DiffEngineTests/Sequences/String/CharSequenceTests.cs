using LoxSmoke.DiffEngine.Sequences.String;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DiffEngineTests.Sequences.String
{
    public class CharSequenceTests
    {
        [Theory]
        [InlineData("", "", true)] // Two empty
        [InlineData("1234", "", true)] // Empty in not empty
        [InlineData("", "1234", false)] // Not empty in empty
        [InlineData("123", "123", true)] // Equal lists
        [InlineData("12345", "123", true)] // Short list in long list
        [InlineData("12", "123", false)] // Long list in short list
        [InlineData("12367", "12345", false)] // Matching starts
        [InlineData("123", "456", false)] // Full mismatch
        public void StartsWith(string text1, string text2, bool expectedResult)
        {
            var s1 = new CharSequence(text1);
            var s2 = new CharSequence(text2);
            var result = s1.StartsWith(s2);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("", "", true)] // Two empty
        [InlineData("1234", "", true)] // Empty in not empty
        [InlineData("", "1234", false)] // Not empty in empty
        [InlineData("123", "123", true)] // Equal lists
        [InlineData("12345", "345", true)] // Short list in long list
        [InlineData("12", "012", false)] // Long list in short list
        [InlineData("321", "4321", false)] // Matching starts
        [InlineData("123", "456", false)] // Full mismatch
        public void EndsWith(string text1, string text2, bool expectedResult)
        {
            var s1 = new CharSequence(text1);
            var s2 = new CharSequence(text2);
            var result = s1.EndsWith(s2);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("1234", "1", 0, 0)]
        [InlineData("1234", "1", 1, -1)]
        [InlineData("1234", "123", 0, 0)]
        [InlineData("1234", "34", 0, 2)]
        [InlineData("1234", "12345", 0, -1)]
        public void IndexOf(string text, string otherText, int startIndex, int expectedResult)
        {
            var s1 = new CharSequence(text);
            var s2 = new CharSequence(otherText);
            var result = s1.IndexOf(s2, startIndex);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("", "abcd", 0)] // Null case.
        [InlineData("abc", "abcd", 3)] // Start overlap.
        [InlineData("123456", "abcd", 0)] // No overlap.
        [InlineData("123456xxx", "xxxabcd", 3)] // End overlap.
        [InlineData("abcdef021", "021zxwyu", 3)] // End with start overlap.
        public void CommonOverlapLength(string text1, string text2, int expected)
        {
            var result = new CharSequence(text1).CommonOverlapLength(new CharSequence(text2));
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("", 0, 0, "")]
        [InlineData("1234", 0, 2, "12")]
        public void Mid(string text, int from, int length, string expectedResult)
        {
            var expected = new CharSequence(expectedResult);
            var result = new CharSequence(text).Mid(from, length);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("", "", "")] // Two empty
        [InlineData("", "1234", "1234")] // Empty+not empty
        [InlineData("1234", "", "1234")] // Not empty+empty
        [InlineData("123", "123", "123123")] // Two lists
        public void Concat(string text1, string text2, string expectedResult)
        {
            var list1 = new CharSequence(text1);
            var list2 = new CharSequence(text2);
            var expected = new CharSequence(expectedResult);
            var result = list1.Concat(list2);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("abc", "xyz", "")] // No match.
        [InlineData("1234abcdef", "1234xyz", "1234")] // Some match.
        [InlineData("1234", "1234xyz", "1234")] // Whole case.
        public void CommonPrefix(string text1, string text2, string expectedResult)
        {
            var expected = new CharSequence(expectedResult);
            var result = new CharSequence(text1).CommonPrefix(new CharSequence(text2));
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("abc", "xyz", "")] // No match.
        [InlineData("abcdef1234", "xyz1234", "1234")] // Some match.
        [InlineData("1234", "xyz1234", "1234")] // Whole case.
        public void CommonSuffix(string text1, string text2, string expectedResult)
        {
            var expected = new CharSequence(expectedResult);
            var result = new CharSequence(text1).CommonSuffix(new CharSequence(text2));
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("", "", true)]
        [InlineData("123", "123", true)]
        [InlineData("123", "1230", false)]
        [InlineData("123", "", false)]
        public void Equals_(string text1, string text2, bool expectedResult)
        {
            var s1 = new CharSequence(text1);
            var s2 = new CharSequence(text2);
            var result = s1.Equals(s2);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("123", 0, "123", 0, true)]
        [InlineData("123", 0, "123", 1, false)]
        public void ItemEquals(string text1, int index1, string text2, int index2, bool expectedResult)
        {
            var s1 = new CharSequence(text1);
            var s2 = new CharSequence(text2);
            var result = s1.ItemEquals(index1, s2, index2);
            Assert.Equal(expectedResult, result);
        }
    }
}
