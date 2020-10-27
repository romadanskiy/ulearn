using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon
{
	public class BfsTask
	{
	    public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Point[] chests)
        {
			var visited = new HashSet<Point>();
			var chestsHashSet = chests.Select(i => i).ToHashSet();
			var queue = new Queue<SinglyLinkedList<Point>>();
			queue.Enqueue(new SinglyLinkedList<Point>(start, null));
			visited.Add(start);
			while (queue.Count != 0)
			{
				var linkedPoint = queue.Dequeue();
				var point = linkedPoint.Value;
				if (chestsHashSet.Contains(point))
					yield return linkedPoint;

				for (var dy = -1; dy <= 1; dy++)
					for (var dx = -1; dx <= 1; dx++)
						if (dx != 0 && dy != 0) continue;
				        else 
						{
							var currentPoint = new Point { X = point.X + dx, Y = point.Y + dy };
							if (!IsNewPoint(currentPoint, map, visited)) continue;
							visited.Add(currentPoint);
							queue.Enqueue(new SinglyLinkedList<Point>(currentPoint, linkedPoint)); 
						}
			}
		}

		public static bool IsNewPoint(Point point, Map map, HashSet<Point> visited)
		{
			return point.X >= 0 && point.Y >= 0 &&
				point.X < map.Dungeon.GetLength(0) && point.Y < map.Dungeon.GetLength(1) &&
				!(map.Dungeon[point.X, point.Y] is MapCell.Wall) && 
				!visited.Contains(point);
		}
	}
}