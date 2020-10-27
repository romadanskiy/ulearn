using System.Linq;

namespace Recognizer
{
    public static class ThresholdFilterTask
    {
        public static double[,] ThresholdFilter(double[,] original, double whitePixelsFraction)
        {
            var x = original.GetLength(0);
            var y = original.GetLength(1);
            var blackAndWhite = new double[x, y];

            var t = FindT(original, whitePixelsFraction, x, y);

            GetBlackAndWhite(original, x, y, blackAndWhite, t);

            return blackAndWhite;
        }

        private static void GetBlackAndWhite(double[,] original, int x, int y, double[,] blackAndWhite, double t)
        {
            for (int i = 0; i < x; i++)
                for (int j = 0; j < y; j++)
                {
                    if (original[i, j] >= t)
                        blackAndWhite[i, j] = 1.0;
                    else
                        blackAndWhite[i, j] = 0.0;
                }
        }

        public static double FindT(double[,] original, double whitePixelsFraction, int x, int y)
        {
            var n = x * y;
            var pixelsArray = new double[n];
            for (int i = 0; i < x; i++)
                for (int j = 0; j < y; j++)
                {
                    pixelsArray[i * y + j] = original[i, j];
                }
            var orderedPixels = pixelsArray.OrderBy(pixel => pixel).ToArray();
            var countOfWhitePixels = (int)(whitePixelsFraction * n);
            double t;
            if (countOfWhitePixels == 0)
                t = double.MaxValue; // т.е. все пиксели должны стать черными
                // можно было бы написать t = 1.1, так как яркость от 0.0 до 1.0
                // но юлерн такой юлерн, что у них в тесте у пикселя яркость 123
                // либо пришлось бы ставить доп условия в методе GetBlackAndWhite
            else if (countOfWhitePixels >= n)
                t = double.MinValue; // т.е. все пиксели должны стать белыми 
            else
                t = orderedPixels[n - countOfWhitePixels];

            return t;
        }
    }
}