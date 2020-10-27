using System;

namespace Recognizer
{
    internal static class SobelFilterTask
    {
        public static double[,] SobelFilter(double[,] g, double[,] sx)
        {
            var width = g.GetLength(0);
            var height = g.GetLength(1);
            var result = new double[width, height];
            var n = sx.GetLength(0);
            var sy = TransposeTheMatrix(sx, n);

            GetResult(g, result, width, height, sx, sy, n);

            return result;
        }

        private static void GetResult(double[,] g, double[,] result, int width, int height,
                                      double[,] sx, double[,] sy, int n)
        {
            var frame = n / 2;
            for (int x = frame; x < width - frame; x++)
                for (int y = frame; y < height - frame; y++)
                {
                    var gx = 0.0;
                    var gy = 0.0;
                    for (int i = 0; i < n; i++)
                        for (int j = 0; j < n; j++)
                        {
                            gx += sx[i, j] * g[x + (i - frame), y + (j - frame)];
                            gy += sy[i, j] * g[x + (i - frame), y + (j - frame)];
                        }

                    result[x, y] = Math.Sqrt(gx * gx + gy * gy);
                }
        }

        public static double[,] TransposeTheMatrix(double[,] sx, int n)
        {
            var sy = new double[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    sy[j, i] = sx[i, j];
                }

            return sy;
        }
    }
}