using System;
using System.Collections.Generic;
using System.Linq;

namespace yield
{

	public static class MovingMaxTask
	{
		public static IEnumerable<DataPoint> MovingMax(this IEnumerable<DataPoint> data, int windowWidth)
		{
			var list = new LinkedList<double>();
			var queue = new Queue<double>();
			foreach(var e in data)
			{
				queue.Enqueue(e.OriginalY);

				while (list.Count != 0 && list.Last.Value < e.OriginalY)
				{
					list.RemoveLast();
				}
				list.AddLast(e.OriginalY);

				if (queue.Count > windowWidth)
					if (queue.Dequeue() == list.First.Value)
						list.RemoveFirst();

				yield return new DataPoint
				{
					X = e.X,
					OriginalY = e.OriginalY,
					MaxY = list.First()
				};

			}
		}
	}
}