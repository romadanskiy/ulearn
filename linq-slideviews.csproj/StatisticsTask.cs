using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
	public class StatisticsTask
	{
		public static double GetMedianTimePerSlide(List<VisitRecord> visits, SlideType slideType)
		{
			if (visits.Count == 0)
				return 0;

			var result = visits
				.GroupBy(visit => visit.UserId)
				.Select(group => group.OrderBy(visitRecord => visitRecord.DateTime))
				.Select(visitRecords => visitRecords.Bigrams())
				.SelectMany(i => i)
				.Where(pair => pair.Item1.SlideType == slideType && pair.Item1.SlideId != pair.Item2.SlideId)
				.Select(pair => (pair.Item2.DateTime - pair.Item1.DateTime).TotalMinutes)
				.Where(minute => minute >= 1 && minute <= 120);

			if (result.Count() == 0)
				return 0;

			return result.Median();
		}
	}
}