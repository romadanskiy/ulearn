using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
	public static class ExtensionsTask
	{
		/// <summary>
		/// Медиана списка из нечетного количества элементов — это серединный элемент списка после сортировки.
		/// Медиана списка из четного количества элементов — это среднее арифметическое 
        /// двух серединных элементов списка после сортировки.
		/// </summary>
		/// <exception cref="InvalidOperationException">Если последовательность не содержит элементов</exception>
		public static double Median(this IEnumerable<double> items)
		{
			var list = items.OrderBy(i => i).ToList();
			var count = list.Count;
			if (count == 0) throw new InvalidOperationException();
			double min;
			if (count % 2 != 0)
				min = list[count / 2];
			else
				min = (list[count / 2 - 1] + list[count / 2]) / 2;

			return min;
		}

		/// <returns>
		/// Возвращает последовательность, состоящую из пар соседних элементов.
		/// Например, по последовательности {1,2,3} метод должен вернуть две пары: (1,2) и (2,3).
		/// </returns>
		public static IEnumerable<Tuple<T, T>> Bigrams<T>(this IEnumerable<T> items)
		{
			var prev = default(T);
			var ItemHasPrev = false;
			foreach(var item in items)
			{
				if (ItemHasPrev)
					yield return Tuple.Create(prev, item);
				ItemHasPrev = true;
				prev = item;
			}
		}
	}
}