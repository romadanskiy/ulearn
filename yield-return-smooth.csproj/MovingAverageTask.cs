using System.Collections.Generic;

namespace yield
{
	public static class MovingAverageTask
	{
		public static IEnumerable<DataPoint> MovingAverage(this IEnumerable<DataPoint> data, int windowWidth)
		{
			var queue = new Queue<double>();
			var sum = 0.0;
			foreach(var e in data)
			{
				if (queue.Count == windowWidth)
					sum -= queue.Dequeue();
				queue.Enqueue(e.OriginalY);
				sum += e.OriginalY;
				yield return new DataPoint
				{
					X = e.X,
					OriginalY = e.OriginalY,
					AvgSmoothedY = sum / queue.Count
				};
			}
		}
	}
}