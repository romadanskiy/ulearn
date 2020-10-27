using System;
using System.Linq;

namespace Names
{
    internal static class HistogramTask
    {
        public static HistogramData GetBirthsPerDayHistogram(NameData[] names, string name)
        {
            var days = new string[31];
            for (int i = 0; i < days.Length; i++)
                days[i] = (i + 1).ToString();      

            var birthsCounts = new double[31];
            foreach (var n in names)
                if (n.Name == name && n.BirthDate.Day != 1)
                    birthsCounts[n.BirthDate.Day - 1]++;

            return new HistogramData(
                string.Format("Рождаемость людей с именем '{0}'", name),
                days,
                birthsCounts);
        }
    }
}