using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Autocomplete
{
    internal class AutocompleteTask
    {
        /// <returns>
        /// Возвращает первую фразу словаря, начинающуюся с prefix.
        /// </returns>
        /// <remarks>
        /// Эта функция уже реализована, она заработает, 
        /// как только вы выполните задачу в файле LeftBorderTask
        /// </remarks>
        public static string FindFirstByPrefix(IReadOnlyList<string> phrases, string prefix)
        {
            var index = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count) + 1;
            if (index < phrases.Count && phrases[index].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                return phrases[index];
            
            return null;
        }

        /// <returns>
        /// Возвращает первые в лексикографическом порядке count (или меньше, если их меньше count) 
        /// элементов словаря, начинающихся с prefix.
        /// </returns>
        /// <remarks>Эта функция должна работать за O(log(n) + count)</remarks>
        public static string[] GetTopByPrefix(IReadOnlyList<string> phrases, string prefix, int count)
        {
            // тут стоит использовать написанный ранее класс LeftBorderTask
            var phrasesList = new List<string>();
            var leftIndex = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count) + 1;
            var rightIndex = leftIndex + count - 1;
            for (int i = leftIndex; i <= rightIndex; i++)
            {
                if(i < phrases.Count && phrases[i].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    phrasesList.Add(phrases[i]);
            }

            return phrasesList.ToArray();
        }

        /// <returns>
        /// Возвращает количество фраз, начинающихся с заданного префикса
        /// </returns>
        public static int GetCountByPrefix(IReadOnlyList<string> phrases, string prefix)
        {
            // тут стоит использовать написанные ранее классы LeftBorderTask и RightBorderTask
            var leftIndex = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count) + 1;
            var rightIndex = RightBorderTask.GetRightBorderIndex(phrases, prefix, -1, phrases.Count) - 1;
            var count = 0;
            for (int i = leftIndex; i <= rightIndex; i++)
            {
                if (i < phrases.Count && phrases[i].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    count++;
            }
            return count;
        }
    }

    [TestFixture]
    public class AutocompleteTests
    {
        //GetTopByPrefix_Tests

        [Test]
        public void TopByPrefix_IsEmpty_WhenNoPhrases()
        {
            var actualTopWords = AutocompleteTask.GetTopByPrefix(new string[] { }, "z", 2);
            CollectionAssert.IsEmpty(actualTopWords);
        }

        [Test]
        public void TopByPrefix_IsEmpty_WhenNoPhrasesStartsWithPrefix()
        {
            var actualTopWords = AutocompleteTask.GetTopByPrefix(new[] { "a", "b" }, "z", 2);
            CollectionAssert.IsEmpty(actualTopWords);
        }

        [Test]
        public void TopByPrefix_ContainsAllPhrases_WhenEmptyPrefix()
        {
            var expectedTopWords = new[] { "a", "b" };
            var actualTopWords = AutocompleteTask.GetTopByPrefix(new[] { "a", "b" }, "", 2);
            Assert.AreEqual(expectedTopWords, actualTopWords);
        }

        [Test]
        public void TopByPrefix_WhenCountIsGreaterThenPhrasesCount()
        {
            var expectedTopWords = new[] { "aa", "ab" };
            var actualTopWords = AutocompleteTask.GetTopByPrefix(new[] { "aa", "ab" }, "a", 5);
            Assert.AreEqual(expectedTopWords, actualTopWords);
        }

        [Test]
        public void TopByPrefix_WhenCountIsLessThenGoodPhrasesCount()
        {
            var expectedTopWords = new[] { "aa" };
            var actualTopWords = AutocompleteTask.GetTopByPrefix(new[] { "aa", "ab" }, "a", 1);
            Assert.AreEqual(expectedTopWords, actualTopWords);
        }

        //GetCountByPrefix_Tests

        [Test]
        public void CountByPrefix_IsTotalCount_WhenEmptyPrefix()
        {
            var expectedCount = 2;
            var actualCount = AutocompleteTask.GetCountByPrefix(new[] { "a", "b" }, "");
            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        public void CountByPrefix_IsZero_WhenNoGoodPhrases()
        {
            var expectedCount = 0;
            var actualCount = AutocompleteTask.GetCountByPrefix(new[] { "a", "b" }, "z");
            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        public void CountByPrefix_IsZero_WhenNoPhrases()
        {
            var expectedCount = 0;
            var actualCount = AutocompleteTask.GetCountByPrefix(new string[] { }, "z");
            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        public void CountByPrefix_WhenNotAllPhrasesStartsWithPrefix()
        {
            var expectedCount = 1;
            var actualCount = AutocompleteTask.GetCountByPrefix(new[] { "a", "b" }, "a");
            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        public void CountByPrefix_WhenAllPhrasesStartsWithPrefix()
        {
            var expectedCount = 2;
            var actualCount = AutocompleteTask.GetCountByPrefix(new[] { "aa", "ab" }, "a");
            Assert.AreEqual(expectedCount, actualCount);
        }
    }
}
