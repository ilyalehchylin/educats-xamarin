namespace EduCATS.Pages.Statistics.Base.Models
{
	public enum StatsChartMetricType
	{
		Pract,
		Labs,
		Tests,
		Course,
		Rating
	}

	public class StatsChartEntryModel
	{
		public StatsChartEntryModel(StatsChartMetricType type, double value)
		{
			Type = type;
			Value = value;
		}

		public StatsChartMetricType Type { get; }

		public double Value { get; }
	}
}
