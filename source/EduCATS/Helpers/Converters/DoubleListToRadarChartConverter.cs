using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using EduCATS.Themes;
using Microcharts;
using SkiaSharp;
using Xamarin.Forms;

namespace EduCATS.Helpers.Converters
{
	public class DoubleListToRadarChartConverter : IValueConverter
	{
		const float _lineSize = 5;
		const float _poinstSize = 20;
		const float _maxValue = 10;

		static SKColor _backgroundColor = SKColor.Empty;

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

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}
