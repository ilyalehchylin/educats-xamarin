using System;
using NUnit.Framework;
using EduCATS.Helpers.Date.Extensions;

namespace EduCATS.UnitTests
{
	[TestFixture]
	public class DateTimeExtensionsTests
	{
		[Test]
		public void StartOfWeekTest()
		{
			var date = new DateTime(2020, 4, 10);
			var actual = date.StartOfWeek(DayOfWeek.Monday);
			var expected = new DateTime(2020, 4, 6);
			Assert.AreEqual(expected, actual);
		}
	}
}
