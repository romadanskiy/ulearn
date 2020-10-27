using System.Collections.Generic;
using System.Text;

namespace TextAnalysis
{
    static class TextGeneratorTask
    {
        public static string ContinuePhrase(
            Dictionary<string, string> nextWords, 
            string phraseBeginning, 
            int wordsCount)
        {
            var wordsArray = phraseBeginning.Split(' ');
            var lastWord = wordsArray[wordsArray.Length - 1];
            var secondLastWord = "";
            if (wordsArray.Length > 1)
                secondLastWord = wordsArray[wordsArray.Length - 2];

            var phrase = new StringBuilder(phraseBeginning);
            BuildPhrase(nextWords, wordsCount, phrase, lastWord, secondLastWord);

            return phrase.ToString();
        }

        private static void BuildPhrase(
            Dictionary<string, string> nextWords, 
            int wordsCount, 
            StringBuilder phrase, 
            string lastWord, 
            string secondLastWord)
        {
            for (int i = 0; i < wordsCount; i++)
            {
                if (!string.IsNullOrEmpty(secondLastWord) && nextWords.ContainsKey(secondLastWord + " " + lastWord))
                {
                    var key = secondLastWord + " " + lastWord;
                    (secondLastWord, lastWord) = AddLastWordToPhrase(nextWords, phrase, key, lastWord);
                }
                else if (nextWords.ContainsKey(lastWord))
                {
                    var key = lastWord;
                    (secondLastWord, lastWord) = AddLastWordToPhrase(nextWords, phrase, key, lastWord);
                }
                else
                    break;
            }
        }

        private static (string, string) AddLastWordToPhrase(
            Dictionary<string, string> nextWords, 
            StringBuilder phrase, 
            string key, 
            string lastWord)
        {
            phrase.Append(" " + nextWords[key]);
            return (lastWord, nextWords[key]);
        }
    }
}



//
//
//
/*using System.Collections.Generic;
using System.Text;

namespace TextAnalysis
{
    static class TextGeneratorTask
    {
        public static string ContinuePhrase(Dictionary<string, string> nextWords,
                                            string phraseBeginning, int wordsCount)
        {
            var wordsList = new List<string>(phraseBeginning.Split(' '));
            var phrase = new StringBuilder(phraseBeginning);
            for (int i = 0; i < wordsCount; i++)
            {
                if (wordsList.Count > 1 &&
                    nextWords.ContainsKey(wordsList[wordsList.Count - 2] + " " + wordsList[wordsList.Count - 1]))
                {
                    phrase.Append(" " + nextWords[wordsList[wordsList.Count - 2] + " " + wordsList[wordsList.Count - 1]]);
                    wordsList.Add(nextWords[wordsList[wordsList.Count - 2] + " " + wordsList[wordsList.Count - 1]]);
                }
                else if (nextWords.ContainsKey(wordsList[wordsList.Count - 1]))
                {
                    phrase.Append(" " + nextWords[wordsList[wordsList.Count - 1]]);
                    wordsList.Add(nextWords[wordsList[wordsList.Count - 1]]);
                }
                else
                    break;
            }
            return phrase.ToString();
        }
    }
}*/
