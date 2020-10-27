using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
	public class ParsingTask
	{
		/// <param name="lines">все строки файла, которые нужно распарсить. Первая строка заголовочная.</param>
		/// <returns>Словарь: ключ — идентификатор слайда, значение — информация о слайде</returns>
		/// <remarks>Метод должен пропускать некорректные строки, игнорируя их</remarks>
		public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines)
		{
			return lines
				.Skip(1)
				.Select(line => LineToSlideRecord(line))
				.Where(slideRecord => slideRecord != null)
				.ToDictionary(slideRecord => slideRecord.SlideId);
		}

		public static SlideRecord LineToSlideRecord(string line)
		{
			var record = line.Split(';');
			if (record.Length == 3 &&
				   int.TryParse(record[0], out int slideId) &&
				   Enum.TryParse(record[1], true, out SlideType slideType))
				return new SlideRecord(slideId, slideType, record[2]);
			return null;
		}

		/// <param name="lines">все строки файла, которые нужно распарсить. Первая строка — заголовочная.</param>
		/// <param name="slides">Словарь информации о слайдах по идентификатору слайда. 
		/// Такой словарь можно получить методом ParseSlideRecords</param>
		/// <returns>Список информации о посещениях</returns>
		/// <exception cref="FormatException">Если среди строк есть некорректные</exception>
		public static IEnumerable<VisitRecord> ParseVisitRecords(
			IEnumerable<string> lines, IDictionary<int, SlideRecord> slides)
		{
			return lines
				.Skip(1)
				.Select(line => LineToVisitRecord(line, slides));
		}

		public static VisitRecord LineToVisitRecord(string line, IDictionary<int, SlideRecord> slides)
		{
			var record = line.Split(';');
			if (record.Length == 4 &&
				   int.TryParse(record[0], out int userId) &&
				   int.TryParse(record[1], out int slideId) &&
				   DateTime.TryParse(record[2] + ' ' + record[3], out DateTime time) &&
				   slides.ContainsKey(slideId))
				return new VisitRecord(userId, slideId, time, slides[slideId].SlideType);
			throw new FormatException(string.Format("Wrong line [{0}]", line));
		}
	}
}