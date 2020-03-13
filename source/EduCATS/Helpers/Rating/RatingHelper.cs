using System.Collections.Generic;
using System.Linq;
using EduCATS.Data.Models.Statistics;

namespace EduCATS.Helpers.Rating
{
	public static class RatingHelper
	{
		public const double MaximumMark = 10;

		const double defaultMark = 0;

		public static double GetAverageMark(string averageMarkString)
		{
			if (string.IsNullOrEmpty(averageMarkString)) {
				return defaultMark;
			}

			double.TryParse(averageMarkString, out double averageMark);
			return averageMark;
		}

		public static double GetAverageVisiting(List<StatisticsVisitingModel> visitingList)
		{
			if (visitingList == null) {
				return defaultMark;
			}

			var averageVisiting = visitingList.Average(v => {
				double.TryParse(v.Mark, out double mark);
				return mark;
			});

			return MaximumMark - (5 * averageVisiting);
		}
	}
}
