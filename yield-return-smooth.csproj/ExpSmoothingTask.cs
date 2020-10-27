using System.Collections.Generic;
using System.Linq;

namespace yield
{
	public static class ExpSmoothingTask
	{
		public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> data, double alpha)
		{
			//if (data.Count() == 0) yield break;
			/*
			var e1 = data.First();
			yield return new DataPoint { X = e1.X, OriginalY = e1.OriginalY, ExpSmoothedY = e1.OriginalY };

			var prev = e1.OriginalY;
			foreach (var e in data.Skip(1))
			{
				var expSmoothedY = alpha * e.OriginalY + (1 - alpha) * prev;
				yield return new DataPoint
				{
					X = e.X,
					OriginalY = e.OriginalY,
					ExpSmoothedY = alpha * e.OriginalY + (1 - alpha) * prev,
				};
				prev = expSmoothedY;
			}*/

			var prev = 0.0;
			var a = 1.0;
			foreach (var e in data)
			{
				var expSmoothedY = a * e.OriginalY + (1 - a) * prev;
				yield return new DataPoint
				{
					X = e.X,
					OriginalY = e.OriginalY,
					ExpSmoothedY = a * e.OriginalY + (1 - a) * prev,
				};
				a = alpha;
				prev = expSmoothedY;
			}
		}
	}
}