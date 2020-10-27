using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Greedy.Architecture;
using System.Drawing;

namespace Greedy
{
	public class DijkstraData
	{
		public Point Previous { get; set; }
		public int Price { get; set; }
	}

	public class DijkstraPathFinder
    {
		public IEnumerable<PathWithCost> GetPathsByDijkstra(State state, Point start,
            IEnumerable<Point> targets)
        {
			var chests = targets.ToHashSet();
			var track = new Dictionary<Point, DijkstraData>();
			track[start] = new DijkstraData { Price = 0, Previous = new Point(-1, -1) };
			var opened = new List<Point>();

			while (chests.Count != 0)
			{
				var toOpen = FindPointToOpen(track, opened);
				if (toOpen == new Point(-1, -1)) break;
				if (chests.Contains(toOpen))
				{
					yield return GetPathWithCost(track, toOpen);
					chests.Remove(toOpen);
				}
				OpenPoint(state, track, opened, toOpen);
				opened.Add(toOpen);
			}
		}

		private static Point FindPointToOpen(Dictionary<Point, DijkstraData> track, List<Point> opened)
		{
			var toOpen = new Point(-1, -1);
			var bestPrice = double.PositiveInfinity;
			foreach (var e in track.Keys)
			{
				if (!opened.Contains(e) && track[e].Price < bestPrice)
				{
					bestPrice = track[e].Price;
					toOpen = e;
				}
			}

			return toOpen;
		}

		private static void OpenPoint(State state, Dictionary<Point, DijkstraData> track, List<Point> opened, Point toOpen)
		{
			foreach (var e in GetNeighbors(state, toOpen))
			{
				if (opened.Contains(e)) continue;
				var currentPrice = track[toOpen].Price + state.CellCost[e.X, e.Y];
				var nextPoint = e;
				if (!track.ContainsKey(nextPoint) || track[nextPoint].Price > currentPrice)
				{
					track[nextPoint] = new DijkstraData { Previous = toOpen, Price = currentPrice };
				}
			}
		}

		public static Point[] GetNeighbors(State state, Point point)
		{
			var neighbors = new List<Point>();
			for (var dx = -1; dx <= 1; dx++)
				for (var dy = -1; dy <= 1; dy++)
				{
					if ((dx != 0 && dy != 0) || (dx == 0 && dy == 0)) continue;
					var neighbor = new Point(point.X + dx, point.Y + dy);
					if (state.InsideMap(neighbor) && !state.IsWallAt(neighbor))
						neighbors.Add(neighbor);
				}

			return neighbors.ToArray();
		}

		public static PathWithCost GetPathWithCost(Dictionary<Point, DijkstraData> track, Point end)
		{
			var result = new List<Point>();
			var cost = track[end].Price;
			while (end != new Point(-1, -1))
			{
				result.Add(end);
				end = track[end].Previous;
			}
			result.Reverse();

			return new PathWithCost(cost, result.ToArray());
		}
    }
}
