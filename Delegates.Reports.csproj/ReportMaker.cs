using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.Reports
{
	public static class ReportMaker
	{
		public static string MakeReport(IEnumerable<Measurement> measurements, Format format, Statistics statistics)
		{
			var data = measurements.ToList();
			var result = new StringBuilder();
			result.Append(format.MakeCaption(statistics.Caption));
			result.Append(format.BeginList());
			result.Append(format.MakeItem("Temperature",
				statistics.MakeStatistics(data.Select(z => z.Temperature)).ToString()));
			result.Append(format.MakeItem("Humidity",
				statistics.MakeStatistics(data.Select(z => z.Humidity)).ToString()));
			result.Append(format.EndList());

			return result.ToString();
		}
	}

	public class Format
	{
		public readonly Func<string, string> MakeCaption;
		public readonly Func<string> BeginList;
		public readonly Func<string, string, string> MakeItem;
		public readonly Func<string> EndList;

		public Format(Func<string, string> makeCaption, 
			Func<string> beginList,
			Func<string, string, string> makeItem,
			Func<string> endList)
		{
			MakeCaption = makeCaption;
			BeginList = beginList;
			MakeItem = makeItem;
			EndList = endList;
		}
	}

	public class Statistics
	{
		public readonly string Caption;
		public readonly Func<IEnumerable<double>, object> MakeStatistics;

		public Statistics(string caption, Func<IEnumerable<double>, object> makeStatistics)
		{
			Caption = caption;
			MakeStatistics = makeStatistics;
		}
	}

	public static class ReportMakerHelper
	{
		public static string MeanAndStdHtmlReport(IEnumerable<Measurement> data)
		{
			return ReportMaker.MakeReport(data, UseHtmlFormat(), FindMeanAndStd());
		}

		public static string MedianMarkdownReport(IEnumerable<Measurement> data)
		{
			return ReportMaker.MakeReport(data, UseMarkdownFormat(), FindMedian());
		}

		public static string MeanAndStdMarkdownReport(IEnumerable<Measurement> data)
		{
			return ReportMaker.MakeReport(data, UseMarkdownFormat(), FindMeanAndStd());
		}

		public static string MedianHtmlReport(IEnumerable<Measurement> data)
		{
			return ReportMaker.MakeReport(data, UseHtmlFormat(), FindMedian());
		}

		private static Format UseHtmlFormat()
		{
			return new Format(
				(caption) => $"<h1>{caption}</h1>",
				() => "<ul>",
				(valueType, entry) => $"<li><b>{valueType}</b>: {entry}",
				() => "</ul>"
			);
		}

		private static Format UseMarkdownFormat()
		{
			return new Format(
				(caption) => $"## {caption}\n\n",
				() => "",
				(valueType, entry) => $" * **{valueType}**: {entry}\n\n",
				() => ""
			);
		}

		private static Statistics FindMeanAndStd()
		{
			return new Statistics(
				"Mean and Std",
				(data) =>
				{
					var list = data.ToList();
					var mean = list.Average();
					var std = Math.Sqrt(list.Select(z => Math.Pow(z - mean, 2)).Sum() / (list.Count - 1));

					return new MeanAndStd
					{
						Mean = mean,
						Std = std
					};
				}
			);
		}

		private static Statistics FindMedian()
		{
			return new Statistics(
				"Median",
				(data) =>
				{
					var list = data.OrderBy(z => z).ToList();
					if (list.Count % 2 == 0)
						return (list[list.Count / 2] + list[list.Count / 2 - 1]) / 2;

					return list[list.Count / 2];
				}
			);
		}
	}
}
