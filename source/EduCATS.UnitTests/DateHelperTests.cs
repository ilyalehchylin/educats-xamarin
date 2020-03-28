using System;
using EduCATS.Helpers.Date;
using NUnit.Framework;

namespace EduCATS.UnitTests
{
	[TestFixture]
	public class DateHelperTests
	{
		[Test]
		public void CheckDatesDifferenceTest()
		{
			var startDate = new DateTime(2020, 03, 20);
			var endDate = new DateTime(2020, 03, 21);
			var expectedResult = new TimeSpan(24, 0, 0);
			var result = DateHelper.CheckDatesDifference(startDate, endDate);
			Assert.AreEqual(expectedResult, result);
		}
	}
}
