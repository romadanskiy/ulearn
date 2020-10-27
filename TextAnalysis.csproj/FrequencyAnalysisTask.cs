using System;
using System.Collections.Generic;
using System.Linq;

namespace TextAnalysis
{
    static class FrequencyAnalysisTask
    {
        public static Dictionary<string, string> GetMostFrequentNextWords(List<List<string>> text)
        {
            var result = new Dictionary<string, string>();
            var frequencyDictionary = new Dictionary<string, Dictionary<string, int>>();
            foreach (var sentence in text)
            {
                for (int i = 0; i < sentence.Count - 1; i++)
                {
                    var key = sentence[i];
                    FindNGram(frequencyDictionary, sentence, i, key, 1);
                    if (i < sentence.Count - 2)
                    {
                        key = sentence[i] + " " + sentence[i + 1];
                        FindNGram(frequencyDictionary, sentence, i, key, 2);
                    }
                }
            }
            foreach (var e in frequencyDictionary)
                result.Add(e.Key, e.Value.OrderByDescending(a => a.Value).ThenBy
                           (b => b.Key, StringComparer.Ordinal).First().Key);
            return result;
        }

        public static void FindNGram(Dictionary<string, Dictionary<string, int>> frequencyDictionary,
                                     List<string> sentence,
                                     int i, string key, int n)
        {
            if (!frequencyDictionary.ContainsKey(key))
                frequencyDictionary[key] = new Dictionary<string, int> { { sentence[i + n], 0 } };
            else if (!frequencyDictionary[key].ContainsKey(sentence[i + n]))
                frequencyDictionary[key][sentence[i + n]] = 0;
            frequencyDictionary[key][sentence[i + n]]++;
        }
    }
}