using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TableParser
{
    [TestFixture]
    public class QuotedFieldTaskTests
    {
        [TestCase("''", 0, "", 2)]
        [TestCase("'a'", 0, "a", 3)]
        [TestCase(@"some_text ""QF \"""" other_text", 10, "QF \"", 7)]
        [TestCase("'a' bc", 0, "a", 3)]
        [TestCase("\"abc\"", 0, "abc", 5)]

        public void Test(string line, int startIndex, string expectedValue, int expectedLength)
        {
            var actualToken = QuotedFieldTask.ReadQuotedField(line, startIndex);
            Assert.AreEqual(actualToken, new Token(expectedValue, startIndex, expectedLength));
        }
    }

    class QuotedFieldTask
    {
        public static char Quote;
        public static int Length;
        public static string Value;
        public static Token ReadQuotedField(string line, int startIndex)
        {
            Quote = line[startIndex];
            var position = startIndex;
            var token = FindToken(line, position);
            Length = token.Length;
            Value = FindValue(token);

            return new Token(Value, startIndex, Length);
        }

        private static string FindValue(string token)
        {
            var value = new StringBuilder();
            for (var i = 0; i < token.Length; i++)
            {
                if (token[i] == '\\')
                {
                    i++;
                    value.Append(token[i]);
                }
                else if (token[i] != Quote)
                {
                    value.Append(token[i]);
                }
            }

            return value.ToString();
        }

        private static string FindToken(string line, int position)
        {
            var token = new StringBuilder();
            for (var i = position; i < line.Length; i++)
            {
                token.Append(line[i]);
                if (i > position)
                    if (line[i] == Quote && line[i - 1] != '\\')
                        break;
            }

            return token.ToString();
        }
    }
}
