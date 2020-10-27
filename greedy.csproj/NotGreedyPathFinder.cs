using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Greedy.Architecture;
using Greedy.Architecture.Drawing;

namespace Greedy
{
	public class NotGreedyPathFinder : IPathFinder
	{
		private List<Point> bestPath;
		private int bestFoundChests;

		public List<Point> FindPathToCompleteGoal(State state)
		{
			FindBestPath(state, state.Position, 0, 0, state.Chests, new List<Point>());

			return bestPath;
		}

		private void FindBestPath(State state, Point start, int energySpent, int foundChests,
			                      IEnumerable<Point> chests, List<Point> path)
		{
			if (bestFoundChests == state.Chests.Count) return;
			if (foundChests > bestFoundChests)
			{
				bestPath = path;
				bestFoundChests = foundChests;
			}
			var finder = new DijkstraPathFinder();
			var paths = finder.GetPathsByDijkstra(state, start, chests);
			foreach (var pathWithCost in paths)
			{
				if (energySpent + pathWithCost.Cost <= state.Energy)
					FindBestPath(state,
								 pathWithCost.End,
								 energySpent + pathWithCost.Cost,
								 foundChests + 1,
								 chests.Except(new[] { pathWithCost.End }),
								 path.Concat(pathWithCost.Path.Skip(1)).ToList());
			}
		}
	}
}