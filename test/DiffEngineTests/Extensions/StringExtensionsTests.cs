using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using static LoxSmoke.DiffEngine.Extensions.StringExtensions;

namespace DiffEngineTests.Extensions
{
    public class StringExtensionsTests
    {
        [Theory]
        [InlineData("", 0, "")]
        [InlineData("a", 0, "")]
        [InlineData("abc", 1, "a")]
        [InlineData("abc", 3, "abc")]
        public void Left(string text, int leftCount, string expectedResults)
        {
            var result = text.Left(leftCount);
            Assert.Equal(expectedResults, result);
        }

        [Theory]
        [InlineData("", 0, "")]
        [InlineData("a", 0, "")]
        [InlineData("abc", 1, "c")]
        [InlineData("abc", 3, "abc")]
        public void Right(string text, int leftCount, string expectedResults)
        {
            var result = text.Right(leftCount);
            Assert.Equal(expectedResults, result);
        }

        [Theory]
        [InlineData("", "abc", "")] // Null case.
        [InlineData("abc", "def", "")] // Null case.
        [InlineData("abcd1234", "abcd9876", "abcd")] //  Non-null case.
        [InlineData("abcd", "abcd0123", "abcd")] // Whole case.
        public void CommonPrefix(string text1, string text2, string expected)
        {
            var result = text1.CommonPrefix(text2);
            Assert.Equal(expected, result);
        }
    }
}
