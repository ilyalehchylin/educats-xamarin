using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

			var doubleList = value as List<double>;
			var chartEntries = doubleList.Select(d => new Microcharts.Entry((float)d)).ToArray();

			if (chartEntries != null && chartEntries.Length == 3) {
				chartEntries[0].Color = SKColor.Parse(Theme.Current.StatisticsChartLabsColor);
				chartEntries[1].Color = SKColor.Parse(Theme.Current.StatisticsChartTestsColor);
				chartEntries[2].Color = SKColor.Parse(Theme.Current.StatisticsChartVisitingColor);
			}

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
	}
}
