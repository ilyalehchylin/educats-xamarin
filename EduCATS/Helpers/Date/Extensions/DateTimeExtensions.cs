using System;

namespace EduCATS.Helpers.Date.Extensions
{
	/// <summary>
	/// <see cref="DateTime"/> extension helpers.
	/// </summary>
	public static class DateTimeExtensions
	{
		/// <summary>
		/// Days in week.
		/// </summary>
		const int _daysInWeek = 7;

		/// <summary>
		/// Get start of week.
		/// </summary>
		/// <param name="date">Original date.</param>
		/// <param name="startOfWeek">Start day of week.</param>
		/// <returns></returns>
		public static DateTime StartOfWeek(this DateTime date, DayOfWeek startOfWeek)
		{
			int diff = (_daysInWeek + (date.DayOfWeek - startOfWeek)) % _daysInWeek;
			return date.AddDays(-1 * diff).Date;
		}
	}
}
