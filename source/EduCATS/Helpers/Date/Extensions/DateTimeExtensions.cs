using System;

namespace EduCATS.Helpers.Date.Extensions
{
	public static class DateTimeExtensions
	{
		const int _daysNumber = 7;

		public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
		{
			int diff = (_daysNumber + (dt.DayOfWeek - startOfWeek)) % _daysNumber;
			return dt.AddDays(-1 * diff).Date;
		}
	}
}
