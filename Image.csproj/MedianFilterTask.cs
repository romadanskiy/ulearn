using System.Collections.Generic;
using System.Linq;

namespace Recognizer
{
	internal static class MedianFilterTask
	{
		/* 
		 * Для борьбы с пиксельным шумом, подобным тому, что на изображении,
		 * обычно применяют медианный фильтр, в котором цвет каждого пикселя, 
		 * заменяется на медиану всех цветов в некоторой окрестности пикселя.
		 * https://en.wikipedia.org/wiki/Median_filter
		 * 
		 * Используйте окно размером 3х3 для не граничных пикселей,
		 * Окно размером 2х2 для угловых и 3х2 или 2х3 для граничных.
		 */
		public static double[,] MedianFilter(double[,] original)
        {
            var x = original.GetLength(0);
            var y = original.GetLength(1);

            var mediansArray = new double[x, y];
            GetMediansArray(original, mediansArray, x, y);

            return mediansArray;
        }

        private static void GetMediansArray(double[,] original, double[,] mediansArray, int x, int y)
        {
            for (int i = 0; i < x; i++)
                for (int j = 0; j < y; j++)
                {
                    mediansArray[i, j] = FindMedian(original, x, y, i, j);
                    /*if (i == 0 && j == 0) //1
                        mediansArray[i, j] = FindMedian(original, x, y, i, i + 1, j, j + 1);
                    else if (i == 0 && j == y - 1) //2
                        mediansArray[i, j] = FindMedian(original, x, y, i, i + 1, j - 1, j);
                    else if (i == x - 1 && j == 0) //3
                        mediansArray[i, j] = FindMedian(original, x, y, i - 1, i, j, j + 1);
                    else if (i == x - 1 && j == y - 1) //4
                        mediansArray[i, j] = FindMedian(original, x, y, i - 1, i, j - 1, j);
                    else if (i == 0 && j > 0 && j < y - 1) //5
                        mediansArray[i, j] = FindMedian(original, x, y, i, i + 1, j - 1, j + 1);
                    else if (i == x - 1 && j > 0 && j < y - 1) //6
                        mediansArray[i, j] = FindMedian(original, x, y, i - 1, i, j - 1, j + 1);
                    else if (j == 0 && i > 0 && i < x - 1) //7
                        mediansArray[i, j] = FindMedian(original, x, y, i - 1, i + 1, j, j + 1);
                    else if (j == y - 1 && i > 0 && i < x - 1) //8
                        mediansArray[i, j] = FindMedian(original, x, y, i - 1, i + 1, j - 1, j);
                    else //9
                        mediansArray[i, j] = FindMedian(original, x, y, i - 1, i + 1, j - 1, j + 1);*/
                }
        }

        public static double FindMedian(double[,] original, int x, int y, int i, int j)
        {
            double median;
            var list = new List<double>();
            for (int a = i - 1; a <= i + 1; a++)
                for (int b = j - 1; b <= j + 1; b++)
                {
                    if (a >= 0 && a < x && b >= 0 && b < y)
                    {
                        list.Add(original[a, b]);
                    }
                }
            var array = list.ToArray().OrderBy(m => m).ToArray();
            var length = array.Length;
            if (length % 2 != 0)
                median = array[length / 2];
            else
                median = (array[length / 2] + array[length / 2 - 1]) / 2.0;

            return median;
        }

        /*public static double FindMedian(double[,] original, int x, int y, int i0, int i1,  int j0, int j1)
        {
            double median;
            var list = new List<double>();
            for (int i = i0; i <= i1; i++)
                for (int j = j0; j <= j1; j++) 
                {
                    if (i < x && j < y)
                    {
                        list.Add(original[i, j]);
                    }
                }
            var array = list.ToArray().OrderBy(m => m).ToArray();
            var length = array.Length;
            if (length % 2 != 0)
                median = array[length / 2];
            else
                median = (array[length / 2] + array[length / 2 - 1]) / 2.0;

            return median;
        }*/
	}
}