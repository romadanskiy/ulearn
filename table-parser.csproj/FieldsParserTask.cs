using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace TableParser
{
    [TestFixture]
    public class FieldParserTaskTests
    {
        public static void Test(string input, string[] expectedResult)
        {
            var actualResult = FieldsParserTask.ParseLine(input);
            Assert.AreEqual(expectedResult.Length, actualResult.Count);
            for (int i = 0; i < expectedResult.Length; ++i)
            {
                Assert.AreEqual(expectedResult[i], actualResult[i].Value);
            }
        }

        [TestCase("text", new[] { "text" })]
        [TestCase("hello world", new[] { "hello", "world" })]
        [TestCase("''", new[] { "" })]
        [TestCase("hello       world", new[] { "hello", "world" })]
        [TestCase("'\"'", new[] { "\"" })]
        [TestCase("\"\'\"", new[] { "\'" })]
        [TestCase(@"""\\""", new[] { "\\" })]
        [TestCase(@"""a", new[] { "a" })]
        [TestCase(@""" ", new[] { " " })]
        [TestCase("\"a\"\"b\"", new[] { "a", "b" })]
        [TestCase("a\"b\"", new[] { "a", "b" })]
        [TestCase("\"a\"b", new[] { "a", "b" })]
        [TestCase("a\"b\"", new[] { "a", "b" })]
        [TestCase(@"""QF \""""", new[] { "QF \"" })]
        [TestCase(@"'QF \''", new[] { "QF \'" })]
        [TestCase(@"", new string[0])]
        [TestCase(@" ""text"" ", new[] { "text" })]
        public static void RunTests(string input, string[] expectedOutput)
        {
            Test(input, expectedOutput);
        }
    }

    public class FieldsParserTask
    {
        public static List<Token> ParseLine(string line)
        {
            var tokenList = new List<Token>();
            var position = 0;
            while (position < line.Length)
            {
                if (line[position] == ' ')
                {
                    var lenght = SymbolIsSpace(line, position);
                    position += lenght;
                }
                else
                {
                    if (line[position] == '\'' || line[position] == '\"')
                        tokenList.Add(ReadQuotedField(line, position));
                    else
                        tokenList.Add(ReadField(line, position));
                    position += tokenList[tokenList.Count - 1].Length;
                }
            }

            return tokenList;
        }
        
        private static Token ReadField(string line, int startIndex)
        {
            var value = new StringBuilder();
            var position = startIndex;
            while (line[position] != ' ' && line[position] != '\'' && line[position] != '\"')
            {
                value.Append(line[position]);
                position++;
                if (position == line.Length) break;
            }

            return new Token(value.ToString(), startIndex, value.Length);
        }

        public static Token ReadQuotedField(string line, int startIndex)
        {
            return QuotedFieldTask.ReadQuotedField(line, startIndex);
        }

        public static int SymbolIsSpace (string line, int startIndex)
        {
            var count = 0;
            var position = startIndex;
            while (true)
            {
                if (position >= line.Length) break;
                if (line[position] == ' ')
                {
                    count++;
                    position++;
                }
                else break;
            }

            return count;
        }
    }
}