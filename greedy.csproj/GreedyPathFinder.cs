using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Greedy.Architecture;
using Greedy.Architecture.Drawing;

namespace Greedy
{
	public class GreedyPathFinder : IPathFinder
	{
		public List<Point> FindPathToCompleteGoal(State state)
		{
			if (state.Goal > state.Chests.Count) return new List<Point>();
			var result = new List<Point>();
			var chests = new HashSet<Point>(state.Chests);
			var start = state.Position;
			var energySpent = 0;
			var finder = new DijkstraPathFinder();
			for (var i = 0; i < state.Goal; i++)
			{
				var pathWithCost = finder.GetPathsByDijkstra(state, start, chests).FirstOrDefault();
				if (pathWithCost == null) return new List<Point>(); 
				energySpent += pathWithCost.Cost;
				if (energySpent > state.Energy) return new List<Point>();
				result = result.Concat(pathWithCost.Path.Skip(1)).ToList();
				start = pathWithCost.End;
				chests.Remove(pathWithCost.End);
			}

			return result;
		}
	}
}