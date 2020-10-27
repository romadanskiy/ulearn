using System.Collections.Generic;
using System.Text;

namespace TextAnalysis
{
    static class SentencesParserTask
    {
        public static List<List<string>> ParseSentences(string text)
        {
            var sentencesList = new List<List<string>>();
            text = text.ToLower();
            var sentencesArray = text.Split(new char[] { '.', '!', '?', ';', ':', '(', ')' });
            var symbolsList = new List<char>();
            foreach(var sentence in sentencesArray)
            {
                for (int i = 0; i < sentence.Length; i++)
                {
                    var symbol = sentence[i];
                    if (!char.IsLetter(symbol) && symbol != '\'' && !symbolsList.Contains(symbol))
                        symbolsList.Add(symbol);
                }
                var wordsArray = sentence.Split(symbolsList.ToArray());
                var wordsList = new List<string>();
                foreach(var word in wordsArray)
                {
                    if (word.Length != 0)
                        wordsList.Add(word);
                }
                if (wordsList.Count != 0) 
                    sentencesList.Add(wordsList);
            }
            return sentencesList;
        }
    }
}