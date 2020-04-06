using System;
using System.Reflection;
using EduCATS.Constants;
using EduCATS.Helpers.Date;
using EduCATS.Helpers.Date.Enums;
using NUnit.Framework;
using Nyxbull.Plugins.CrossLocalization;

namespace EduCATS.UnitTests
{
	[TestFixture]
	public class DateHelperTests
	{
		const double _unixInvalidTimestamp = 11111111111111;
		const string _unixTimestamp = "Date(1582980651357)";
		const double _unixRawTimestamp = 1582980651357;
		const string _convertedUnixDateString = "2020-02-29 12:50:51.357";
		const string _monthJanuary = "January";
		const string _monthFebruary = "February";
		const string _monthMarch = "March";
		const string _monthApril = "April";
		const string _monthMay = "May";
		const string _monthJune = "June";
		const string _monthJuly = "July";
		const string _monthAugust = "August";
		const string _monthSeptember = "September";
		const string _monthOctober = "October";
		const string _monthNovember = "November";
		const string _monthDecember = "December";
		static readonly DateTime _defaultDate = new DateTime(2020, 4, 7);

		[SetUp]
		public void SetUp()
		{
			var assembly = typeof(App).GetTypeInfo().Assembly;
			CrossLocalization.Initialize(
				assembly,
				GlobalConsts.RunNamespace,
				GlobalConsts.LocalizationDirectory);

			CrossLocalization.AddLanguageSupport(Languages.EN);
			CrossLocalization.SetDefaultLanguage(Languages.EN.LangCode);
			CrossLocalization.SetLanguage(Languages.EN.LangCode);
		}

		[Test]
		public void CheckDatesDifferenceTest()
		{
			var startDate = new DateTime(2020, 03, 20);
			var endDate = new DateTime(2020, 03, 21);
			var expectedResult = new TimeSpan(24, 0, 0);
			var result = DateHelper.CheckDatesDifference(startDate, endDate);
			Assert.AreEqual(expectedResult, result);
		}

		[Test]
		public void GetUnixFromStringTest()
		{
			var actual = DateHelper.GetUnixFromString(_unixTimestamp);
			Assert.AreEqual(_unixRawTimestamp, actual);
		}

		[Test]
		public void Convert13DigitsUnixToDateTimeTest()
		{
			var expected = DateTime.Parse(_convertedUnixDateString);
			var actual = DateHelper.Convert13DigitsUnixToDateTime(_unixRawTimestamp);
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void ConvertInvalid13DigitsUnixToDateTimeTest()
		{
			var expected = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			var actual = DateHelper.Convert13DigitsUnixToDateTime(_unixInvalidTimestamp);
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetMonthNameTest()
		{
			for (var i = 1; i < 13; i++) {
				var actual = DateHelper.GetMonthName(i);

				switch (i) {
					case 1:
						Assert.AreEqual(_monthJanuary, actual);
						break;
					case 2:
						Assert.AreEqual(_monthFebruary, actual);
						break;
					case 3:
						Assert.AreEqual(_monthMarch, actual);
						break;
					case 4:
						Assert.AreEqual(_monthApril, actual);
						break;
					case 5:
						Assert.AreEqual(_monthMay, actual);
						break;
					case 6:
						Assert.AreEqual(_monthJune, actual);
						break;
					case 7:
						Assert.AreEqual(_monthJuly, actual);
						break;
					case 8:
						Assert.AreEqual(_monthAugust, actual);
						break;
					case 9:
						Assert.AreEqual(_monthSeptember, actual);
						break;
					case 10:
						Assert.AreEqual(_monthOctober, actual);
						break;
					case 11:
						Assert.AreEqual(_monthNovember, actual);
						break;
					case 12:
						Assert.AreEqual(_monthDecember, actual);
						break;
				}
			}
		}

		[Test]
		public void GetInvalidMonthName()
		{
			var actual = DateHelper.GetMonthName(15);
			Assert.AreEqual(string.Empty, actual);
		}

		[Test]
		public void GetDaysWithFirstLettersTest()
		{
			var actual = DateHelper.GetDaysWithFirstLetters();
			Assert.AreEqual("M", actual[0]);
			Assert.AreEqual("T", actual[1]);
			Assert.AreEqual("W", actual[2]);
			Assert.AreEqual("T", actual[3]);
			Assert.AreEqual("F", actual[4]);
			Assert.AreEqual("S", actual[5]);
			Assert.AreEqual("S", actual[6]);
		}

		[Test]
		public void GetPreviousWeekStartDate()
		{
			var date = _defaultDate;
			var expected = new DateTime(2020, 3, 30);
			var actual = DateHelper.GetWeekStartDate(date, WeekEnum.Previous);
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetCurrentWeekStartDate()
		{
			var date = _defaultDate;
			var expected = new DateTime(2020, 4, 6);
			var actual = DateHelper.GetWeekStartDate(date, WeekEnum.Current);
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetNextWeekStartDate()
		{
			var date = _defaultDate;
			var expected = new DateTime(2020, 4, 13);
			var actual = DateHelper.GetWeekStartDate(date, WeekEnum.Next);
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetWeekDaysTest()
		{
			var weekDays = DateHelper.GetWeekDays(_defaultDate);
			var startOfWeekDate = new DateTime(2020, 4, 6);

			for (var i = 0; i < 7; i++) {
				var actual = weekDays[i];
				var expected = startOfWeekDate.AddDays(i);
				Assert.AreEqual(expected, actual);
			}
		}
	}
}
