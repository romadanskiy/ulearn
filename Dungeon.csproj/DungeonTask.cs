using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon
{
	public class DungeonTask
	{
		public static MoveDirection[] FindShortestPath(Map map)
		{
			var pathToExit = BfsTask.FindPaths(map, map.InitialPosition, new[] { map.Exit }).FirstOrDefault(); // путь от старта до выхода

			if (pathToExit == null) return new MoveDirection[0]; // если до выхода нельзя добраться, то вернуть пустой массив

			if (map.Chests.Any(chest => pathToExit.Contains(chest)))
				return GetMoveDirectionsArray(pathToExit.ToList()); // если путь от страта до выхода содержит сундук, вернуть этот путь

			var pathsToChests = BfsTask.FindPaths(map, map.InitialPosition, map.Chests); // пути от старта до сундуков
			var pathsWithChestToExit = pathsToChests
				.Select(path => Tuple.Create(path, BfsTask.FindPaths(map, path.Value, new[] { map.Exit }).FirstOrDefault()));
			    // пути до выхода через каждый сундук

			var minPath = FindMinPath(pathsWithChestToExit); // минимальный путь через сундук до выхода

			if (minPath == null) return GetMoveDirectionsArray(pathToExit.ToList());

			return GetMoveDirectionsArray(minPath.Item1.ToList())
				.Concat(GetMoveDirectionsArray(minPath.Item2.ToList()))
				.ToArray();

		}

		private static Tuple<SinglyLinkedList<Point>, SinglyLinkedList<Point>> FindMinPath(
			IEnumerable<Tuple<SinglyLinkedList<Point>, SinglyLinkedList<Point>>> paths)
		{
			if (paths.Count() == 0) return null;
			if (paths.First().Item2 == null) return null;

			var minLength = int.MaxValue;
			var minPath = paths.First();
			foreach(var path in paths)
			{
				if (path.Item1.Length + path.Item2.Length < minLength)
				{
					minLength = path.Item1.Length + path.Item2.Length;
					minPath = path;
				}
			}
			
			return minPath;
		}

		private static MoveDirection[] GetMoveDirectionsArray(List<Point> path)
		{
			if (path == null) return new MoveDirection[0];

			path.Reverse();
			var moves = new List<MoveDirection>();
			var prev = path.First();
			foreach (var point in path.Skip(1))
			{
				var current = point;
				var dx = current.X - prev.X;
				var dy = current.Y - prev.Y;
				if (dx == 1) moves.Add(MoveDirection.Right);
				else if (dx == -1) moves.Add(MoveDirection.Left);
				else if (dy == 1) moves.Add(MoveDirection.Down);
				else moves.Add(MoveDirection.Up);
				prev = current;
			}

			return moves.ToArray();
		}
	}
}
