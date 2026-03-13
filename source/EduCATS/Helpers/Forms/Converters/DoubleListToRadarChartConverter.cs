using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using EduCATS.Pages.Statistics.Base.Models;
using EduCATS.Themes;
using Microcharts;
using SkiaSharp;
using Xamarin.Forms;

namespace EduCATS.Helpers.Forms.Converters
{
	/// <summary>
	/// List of double to <see cref="RadarChart"/> converter.
	/// </summary>
	public class DoubleListToRadarChartConverter : IValueConverter
	{
		/// <summary>
		/// Radar line size.
		/// </summary>
		const float _lineSize = 5;

		/// <summary>
		/// Radar point size.
		/// </summary>
		const float _poinstSize = 20;

		/// <summary>
		/// Radar maximum value.
		/// </summary>
		const float _maxValue = 10;

		/// <summary>
		/// Radar background <see cref="SKColor"/>.
		/// </summary>
		static SKColor _backgroundColor = SKColor.Empty;

		/// <summary>
		/// Convert.
		/// </summary>
		/// <param name="value">List of double.</param>
		/// <param name="targetType">Target type.</param>
		/// <param name="parameter">Parameter.</param>
		/// <param name="culture">Culture info.</param>
		/// <returns>Converter <see cref="RadarChart"/>.</returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null) {
				return null;
			}

			var metrics = value as List<StatsChartEntryModel>;
			if (metrics == null || metrics.Count == 0)
			{
				return null;
			}

			var chartEntries = metrics.Select(entry => new ChartEntry((float)entry.Value)
			{
				Color = getMetricColor(entry.Type)
			}).ToArray();

			return new RadarChart {
				LineSize = _lineSize,
				MaxValue = _maxValue,
				Entries = chartEntries,
				PointSize = _poinstSize,
				BackgroundColor = _backgroundColor
			};
		}

		/// <summary>
		/// Convert back.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="targetType">Target type.</param>
		/// <param name="parameter">Parameter.</param>
		/// <param name="culture">Culture info.</param>
		/// <returns>Object.</returns>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}

		static SKColor getMetricColor(StatsChartMetricType type)
		{
			return type switch
			{
				StatsChartMetricType.Pract => SKColor.Parse(Theme.Current.StatisticsChartPractColor),
				StatsChartMetricType.Labs => SKColor.Parse(Theme.Current.StatisticsChartLabsColor),
				StatsChartMetricType.Tests => SKColor.Parse(Theme.Current.StatisticsChartTestsColor),
				StatsChartMetricType.Course => SKColor.Parse(Theme.Current.StatisticsChartCourseColor),
				_ => SKColor.Parse(Theme.Current.StatisticsChartRatingColor)
			};
		}
	}
}
