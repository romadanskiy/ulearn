using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rivals
{
	public class RivalsTask
	{
		public static IEnumerable<OwnedLocation> AssignOwners(Map map)
		{
			var visited = new HashSet<Point>();
			var queue = new Queue<OwnedLocation>();
			for (var i = 0; i < map.Players.Length; i++)
			{
				queue.Enqueue(new OwnedLocation(i, new Point(map.Players[i].X, map.Players[i].Y), 0));
				visited.Add(map.Players[i]);
			}
			while (queue.Count != 0)
			{
				var owned = queue.Dequeue();
				var point = owned.Location;
				yield return new OwnedLocation(owned.Owner, new Point(point.X, point.Y), owned.Distance);

				for (var dy = -1; dy <= 1; dy++)
					for (var dx = -1; dx <= 1; dx++)
						if (dx != 0 && dy != 0) continue;
						else
						{
							var currentPoint = new Point(point.X + dx, point.Y + dy);
							if (!IsNewPoint(currentPoint, map, visited)) continue;
							visited.Add(currentPoint);
							queue.Enqueue(new OwnedLocation(owned.Owner, new Point(point.X + dx, point.Y + dy), owned.Distance + 1));
						}
			}
		}

		public static bool IsNewPoint(Point point, Map map, HashSet<Point> visited)
		{
			return point.X >= 0 && point.Y >= 0 &&
				point.X < map.Maze.GetLength(0) && point.Y < map.Maze.GetLength(1) &&
				!(map.Maze[point.X, point.Y] is MapCell.Wall) &&
				!visited.Contains(point);
		}
	}
}
