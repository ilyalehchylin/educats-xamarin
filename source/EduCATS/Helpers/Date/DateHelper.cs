using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using EduCATS.Helpers.Date.Enums;
using EduCATS.Helpers.Date.Extensions;
using Nyxbull.Plugins.CrossLocalization;

namespace EduCATS.Helpers.Date
{
	public static class DateHelper
	{
		const string defaultDate = "yyyy-MM-dd";
		const string defaultDateTime = defaultDate + " hh:mm:ss";
		const string insideRoundBracketsRegex = @"(?<=\().+?(?=\))";

		public static bool IsValidToday(string startDate, string expirationDate)
		{
			DateTime.TryParse(startDate, out DateTime startDateTime);
			DateTime.TryParse(expirationDate, out DateTime expirationDateTime);
			var currentDate = DateTime.Today;

			if (currentDate <= expirationDateTime && currentDate >= startDateTime) {
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
				return defaultDateTime;
			}

			return defaultDate;
		}

		public static double GetUnixFromString(string unixDateString)
		{
			var regex = Regex.Match(unixDateString, insideRoundBracketsRegex);
			var regexString = regex.Value;

			if (!string.IsNullOrEmpty(regexString)) {
				return Convert.ToDouble(regexString);
			}

			return 0;
		}

		public static DateTime Convert13DigitsUnixToDateTime(double unixTimestamp)
		{
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

			if (unixTimestamp < 1000000000000 || unixTimestamp > 9999999999999) {
				return dateTime;
			}

			return dateTime.AddMilliseconds(unixTimestamp);
		}

		public static string GetMonthName(int month)
		{
			if (month > 0 && month < 13) {
				var cultureInfo = CrossLocalization.GetCurrentCultureInfo();
				return cultureInfo.DateTimeFormat.GetMonthName(month);
			}

			return string.Empty;
		}

		static DayOfWeek getDayOfWeek(int dayOfWeek)
		{
			switch (dayOfWeek) {
				case 1:
					return DayOfWeek.Monday;
				case 2:
					return DayOfWeek.Tuesday;
				case 3:
					return DayOfWeek.Wednesday;
				case 4:
					return DayOfWeek.Thursday;
				case 5:
					return DayOfWeek.Friday;
				case 6:
					return DayOfWeek.Saturday;
				case 7:
					return DayOfWeek.Sunday;
				default:
					return DayOfWeek.Monday;
			}
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
				daysOfWeek.Add(
					dayOfWeekName[0]
					.ToString()
					.ToUpper());
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