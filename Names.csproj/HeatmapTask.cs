using System;

namespace Names
{
    internal static class HeatmapTask
    {
        public static HeatmapData GetBirthsPerDateHeatmap(NameData[] names)
        {
            var xLabels = new string[30];
            for (int i = 0; i < 30; i++)
                xLabels[i] = (i + 2).ToString();

            var yLabels = new string[12];
            for (int i = 0; i < 12; i++)
                yLabels[i] = (i + 1).ToString();

            var heat = new double[30, 12];
            foreach (var name in names)
                if (name.BirthDate.Day != 1)
                {
                    heat[name.BirthDate.Day - 2, name.BirthDate.Month - 1]++;
                }

            return new HeatmapData(
                "Пример карты интенсивностей",
                heat,
                xLabels,
                yLabels);
        }
    }
}