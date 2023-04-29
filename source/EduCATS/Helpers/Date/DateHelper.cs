using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using EduCATS.Helpers.Date.Enums;
using EduCATS.Helpers.Date.Extensions;
using EduCATS.Helpers.Extensions;
using Nyxbull.Plugins.CrossLocalization;

namespace EduCATS.Helpers.Date
{
	/// <summary>
	/// Date helper.
	/// </summary>
	public static class DateHelper
	{
		/// <summary>
		/// Regex pattern to get unix numbers inside round brackets.
		/// </summary>
		/// <example>
		///	<c>Date(1582980651357)</c>
		/// </example>
		const string _insideRoundBracketsRegex = @"(?<=\().+?(?=\))";

		/// <summary>
		/// Minimum unix timestamp.
		/// </summary>
		const double _unixMinTimestamp = 1000000000000;

		/// <summary>
		/// Maximum unix timestamp.
		/// </summary>
		const double _unixMaxTimestamp = 9999999999999;

		/// <summary>
		/// Default date string.
		/// </summary>
		public const string DefaultDateTime = "0001-01-01";

		/// <summary>
		/// Date string.
		/// </summary>
		public const string DateTime = "dd.MM.yyyy";

		/// <summary>
		/// Default date & time format (without seconds).
		/// </summary>
		public const string DefaultDateTimeFormat = "dd-MM-yyyy HH:mm";

		/// <summary>
		/// Get dates difference in <see cref="TimeSpan"/>.
		/// </summary>
		/// <param name="startDate">Start date.</param>
		/// <param name="endDate">End date.</param>
		/// <returns>Time difference.</returns>
		public static TimeSpan CheckDatesDifference(DateTime startDate, DateTime endDate)
		{
			return endDate - startDate;
		}

		/// <summary>
		/// Parse string for unix numbers.
		/// </summary>
		/// <param name="unixDateString">Unix date string
		/// in "<c>Date(123456789999)</c>" format.</param>
		/// <returns>Unix date.</returns>
		public static double GetUnixFromString(string unixDateString)
		{
			var regex = Regex.Match(unixDateString, _insideRoundBracketsRegex);
			var regexString = regex.Value;

			if (!string.IsNullOrEmpty(regexString)) {
				return Convert.ToDouble(regexString);
			}

			return 0;
		}

		/// <summary>
		/// Convert 13-digits unix to <see cref="DateTime"/>.
		/// </summary>
		/// <param name="unixTimestamp">13-digits unix timestamp.</param>
		/// <returns>Converted <see cref="DateTime"/>.</returns>
		public static DateTime Convert13DigitsUnixToDateTime(double unixTimestamp)
		{
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

			if (unixTimestamp < _unixMinTimestamp || unixTimestamp > _unixMaxTimestamp) {
				return dateTime;
			}

			return dateTime.AddMilliseconds(unixTimestamp);
		}

		/// <summary>
		/// Gete month name by number.
		/// </summary>
		/// <param name="month">Month number.</param>
		/// <returns>Month name.</returns>
		public static string GetMonthName(int month)
		{
			if (month > 0 && month < 13) {
				var cultureInfo = CrossLocalization.GetCurrentCultureInfo();
				return cultureInfo.DateTimeFormat.GetMonthName(month)
					?.FirstCharToUpper();
			}

			return string.Empty;
		}

		/// <summary>
		/// Get day of week by number.
		/// </summary>
		/// <param name="dayOfWeek">Day of week number.</param>
		/// <returns>Day of week enumeration.</returns>
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

		/// <summary>
		/// Get day of week name by number.
		/// </summary>
		/// <param name="dayOfWeek">Day of week number.</param>
		/// <returns>Day of week name.</returns>
		static string getDayOfWeekName(int dayOfWeek)
		{
			var cultureInfo = CrossLocalization.GetCurrentCultureInfo();
			return cultureInfo.DateTimeFormat.GetDayName(getDayOfWeek(dayOfWeek));
		}

		/// <summary>
		/// Get list of days' first letters.
		/// </summary>
		/// <returns>List of days' first letters.</returns>
		public static List<string> GetDaysWithFirstLetters()
		{
			var daysOfWeek = new List<string>();


			for (int dayOfWeek = 1; dayOfWeek < 8; dayOfWeek++) {
				var dayOfWeekName = getDayOfWeekName(dayOfWeek);
				daysOfWeek.Add(dayOfWeekName[0].ToString().ToUpper());
			}

			return daysOfWeek;
		}

		/// <summary>
		/// Get start of week date.
		/// </summary>
		/// <param name="currentDate">Current date.</param>
		/// <param name="week">Week enumeration.</param>
		/// <returns>Week start date.</returns>
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

		/// <summary>
		/// Get weeks days by date.
		/// </summary>
		/// <param name="date">Date.</param>
		/// <returns>Week days.</returns>
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
