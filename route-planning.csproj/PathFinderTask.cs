using System;
using System.Collections.Generic;
using System.Drawing;

namespace RoutePlanning
{
	public static class PathFinderTask
	{
        /*public static int[] FindBestCheckpointsOrder(Point[] checkpoints)
		{
			var bestOrder = MakeTrivialPermutation(checkpoints);
			return bestOrder;
		}

		private static int[] MakeTrivialPermutation(Point[] checkpoints)
		{
            var size = checkpoints.Length;
            var bestOrder = new int[size];
            var permutationsList = new List<int[]>();
            MakePermutations(permutationsList, size, bestOrder, 1);
            bestOrder = FindBestOrder(permutationsList, checkpoints, bestOrder);
            
			return bestOrder;
		}

        private static void MakePermutations(List<int[]> permutationsList, int size, int[] permutation, int position)
        {
            if (position == permutation.Length)
            {
                var array = new int[permutation.Length];
                Array.Copy(permutation, array, permutation.Length);
                permutationsList.Add(array);
                return;
            }

            for (int i = 0; i < permutation.Length; i++)
            {
                var index = Array.IndexOf(permutation, i, 0, position);
                if (index != -1)
                    continue;
                permutation[position] = i;
                MakePermutations(permutationsList, size, permutation, position + 1);
            }
        }

        private static int[] FindBestOrder(List<int[]> permutationsList, Point[] checkpoints, int[] bestOrder)
        {
            var min = double.MaxValue;
            foreach (var permutation in permutationsList)
            {
                var way = GetWay(permutation, checkpoints);
                if (way < min)
                { 
                    min = way;
                    bestOrder = permutation;
                }
            }

            return bestOrder;
        }
        
        private static double GetWay(int[] permutation, Point[] checkpoints)
        {
            var way = 0.0;
            for (int i = 0; i < permutation.Length - 1; i++)
            {
                var x1 = checkpoints[permutation[i]].X;
                var y1 = checkpoints[permutation[i]].Y;
                var x2 = checkpoints[permutation[i + 1]].X;
                var y2 = checkpoints[permutation[i + 1]].Y;
                way += Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
            }

            return way;
        }*/
        public static double min = 0.0;

        public static int[] FindBestCheckpointsOrder(Point[] checkpoints)
        {
            var size = checkpoints.Length;
            var bestOrder = new int[size];
            MakeTrivialPermutation(checkpoints, bestOrder, size, new int[size], 1);
            min = 0.0;

            return bestOrder;
        }

        /*private static int[] MakeTrivialPermutation(Point[] checkpoints)
        {
            var size = checkpoints.Length;
            var bestOrder = new int[size];
            CheckAllPermutations(checkpoints, 0.0, size, bestOrder, 1);

            return bestOrder;
        }*/

        private static void MakeTrivialPermutation(Point[] checkpoints, int[] bestOrder, int size, int[] permutation, int position)
        {
            if (position == permutation.Length)
            {
                Array.Copy(permutation, bestOrder, size);
                min = GetPathLength(permutation, position, checkpoints);
                return;
            }

            for (int i = 0; i < permutation.Length; i++)
            {
                var index = Array.IndexOf(permutation, i, 0, position);
                if (index != -1)
                {
                    if (GetPathLength(permutation, position, checkpoints) > min)
                        return;
                    continue; 
                }
                permutation[position] = i;
                MakeTrivialPermutation(checkpoints, bestOrder, size, permutation, position + 1);
            }
        }

        /*private static void CheckAllPermutations(Point[] checkpoints, double min, int size, int[] permutation, int position)
        {
            if (position == permutation.Length)
            {
                min = GetPathLenght(permutation, position, checkpoints);
                return;
            }

            if (position > 0 && min != 0)
            {
                if (GetPathLenght(permutation, position, checkpoints) > min)
                    return;

            }
            for (int i = 0; i < permutation.Length; i++)
            {
                var index = Array.IndexOf(permutation, i, 0, position);
                if (index != -1)
                    continue;
                permutation[position] = i;
                CheckAllPermutations(checkpoints, min, size, permutation, position + 1);
            }
        }*/

        private static double GetPathLength(int[] permutation, int position, Point[] checkpoints)
        {
            var pathLength = 0.0;
            for (int i = 0; i < position - 1; i++)
            {
                var x1 = checkpoints[permutation[i]].X;
                var y1 = checkpoints[permutation[i]].Y;
                var x2 = checkpoints[permutation[i + 1]].X;
                var y2 = checkpoints[permutation[i + 1]].Y;
                pathLength += Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
            }

            return pathLength;
        }
    }
}