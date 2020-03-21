using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using EduCATS.Helpers.Date.Enums;
using EduCATS.Helpers.Date.Extensions;
using EduCATS.Helpers.Extensions;
using Nyxbull.Plugins.CrossLocalization;

namespace EduCATS.Helpers.Date
{
	public static class DateHelper
	{
		const string _defaultDate = "yyyy-MM-dd";
		const string _defaultDateTime = _defaultDate + " hh:mm:ss";
		const string _insideRoundBracketsRegex = @"(?<=\().+?(?=\))";
		const double _unixMinTimestamp = 1000000000000;
		const double _unixMaxTimestamp = 9999999999999;

		public const string DefaultDateTime = "0001-01-01";
		public const string DefaultDateTimeFormat = "dd-MM-yyyy hh:mm";

		public static bool IsValidToday(string startDate, string expirationDate)
		{
			DateTime.TryParse(startDate, out DateTime startDateTime);
			DateTime.TryParse(expirationDate, out DateTime expirationDateTime);
			var currentDate = DateTime.Today;

			if (currentDate <= expirationDateTime &&
				currentDate >= startDateTime) {
				return true;
			}

			return false;
		}

		public static DateTime SubtractMonth(DateTime dateSource, int monthCount)
		{
			return dateSource.AddMonths(-monthCount);
		}

		public static TimeSpan CheckDatesDifference(DateTime startDate, DateTime endDate)
		{
			return endDate - startDate;
		}

		public static bool CheckDateForUpdate(DateTime previousDate, double timeToBePassed)
		{
			var currentTime = DateTime.Now;
			var hoursPassed = CheckDatesDifference(previousDate, currentTime).TotalHours;

			if (hoursPassed >= timeToBePassed) {
				return true;
			}

			return false;
		}

		public static string GetDefaultStyle(bool time = false)
		{
			if (time) {
				return _defaultDateTime;
			}

			return _defaultDate;
		}

		public static double GetUnixFromString(string unixDateString)
		{
			var regex = Regex.Match(unixDateString, _insideRoundBracketsRegex);
			var regexString = regex.Value;

			if (!string.IsNullOrEmpty(regexString)) {
				return Convert.ToDouble(regexString);
			}

			return 0;
		}

		public static DateTime Convert13DigitsUnixToDateTime(double unixTimestamp)
		{
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

			if (unixTimestamp < _unixMinTimestamp || unixTimestamp > _unixMaxTimestamp) {
				return dateTime;
			}

			return dateTime.AddMilliseconds(unixTimestamp);
		}

		public static string GetMonthName(int month)
		{
			if (month > 0 && month < 13) {
				var cultureInfo = CrossLocalization.GetCurrentCultureInfo();
				return cultureInfo.DateTimeFormat.GetMonthName(month)
					?.FirstCharToUpper();
			}

			return string.Empty;
		}

		static DayOfWeek getDayOfWeek(int dayOfWeek)
		{
			return dayOfWeek switch
			{
				1 => DayOfWeek.Monday,
				2 => DayOfWeek.Tuesday,
				3 => DayOfWeek.Wednesday,
				4 => DayOfWeek.Thursday,
				5 => DayOfWeek.Friday,
				6 => DayOfWeek.Saturday,
				7 => DayOfWeek.Sunday,
				_ => DayOfWeek.Monday,
			};
		}

		public static string GetDayOfWeekName(int dayOfWeek)
		{
			var cultureInfo = CrossLocalization.GetCurrentCultureInfo();
			return cultureInfo.DateTimeFormat.GetDayName(getDayOfWeek(dayOfWeek));
		}

		public static List<string> GetDaysWithFirstLetters()
		{
			var daysOfWeek = new List<string>();


			for (int dayOfWeek = 1; dayOfWeek < 8; dayOfWeek++) {
				var dayOfWeekName = GetDayOfWeekName(dayOfWeek);
				daysOfWeek.Add(dayOfWeekName[0].ToString().ToUpper());
			}

			return daysOfWeek;
		}

		public static DateTime GetWeekStartDate(DateTime currentDate, WeekEnum week)
		{
			switch (week) {
				case WeekEnum.Previous:
					currentDate = currentDate.AddDays(-7);
					break;
				case WeekEnum.Next:
					currentDate = currentDate.AddDays(7);
					break;
			}

			return currentDate.StartOfWeek(DayOfWeek.Monday);
		}

		public static List<DateTime> GetWeekDays(DateTime date)
		{
			var dates = new List<DateTime>();
			date = date.StartOfWeek(DayOfWeek.Monday);

			for (int day = 1; day < 8; day++) {
				dates.Add(date);
				date = date.AddDays(1);
			}

			return dates;
		}
	}
}
