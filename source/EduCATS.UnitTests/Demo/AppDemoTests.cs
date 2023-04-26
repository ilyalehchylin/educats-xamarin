using EduCATS.Constants;
using EduCATS.Demo;
using System.Reflection;
using NUnit.Framework;
using Nyxbull.Plugins.CrossLocalization;
using EduCATS.Fonts;
using System.Net;
using System;

namespace EduCATS.UnitTests
{
	[TestFixture]
	public class AppDemoTests
	{
		const string _demoString = "demo";
		const string _demoWithUpperCasedLetterString = "Demo";
		const string _invalidDemoString = "invalid_demo";

		[Test]
		public void CheckDemoAccountTest()
		{
			var validCredentialsResult = AppDemo.Instance.CheckDemoAccount(_demoString, _demoString);
			Assert.IsTrue(validCredentialsResult);
			var invalidUsernameResult = AppDemo.Instance.CheckDemoAccount(_invalidDemoString, _demoString);
			Assert.IsFalse(invalidUsernameResult);
			var invalidPasswordResult = AppDemo.Instance.CheckDemoAccount(_demoString, _invalidDemoString);
			Assert.IsFalse(invalidPasswordResult);
			var upperCasedUsernameResult = AppDemo.Instance.CheckDemoAccount(_demoWithUpperCasedLetterString, _demoString);
			Assert.IsTrue(upperCasedUsernameResult);
			var upperCasedPasswordResult = AppDemo.Instance.CheckDemoAccount(_demoString, _demoWithUpperCasedLetterString);
			Assert.IsFalse(upperCasedPasswordResult);
			var invalidCredentialsResult = AppDemo.Instance.CheckDemoAccount(_invalidDemoString, _invalidDemoString);
			Assert.IsFalse(invalidCredentialsResult);
			var invalidUsernameCredentialsResult = AppDemo.Instance.CheckDemoAccount(_invalidDemoString, _demoString);
			Assert.IsFalse(invalidUsernameCredentialsResult);
			var invalidPasswordCredentialsResult = AppDemo.Instance.CheckDemoAccount(_demoString, _invalidDemoString);
			Assert.IsFalse(invalidPasswordCredentialsResult);
		}

		[Test]
		public void IsDemoAccountTest()
		{
			AppDemo.Instance.IsDemoAccount = true;
			Assert.IsTrue(AppDemo.Instance.IsDemoAccount);
			AppDemo.Instance.IsDemoAccount = false;
			Assert.IsFalse(AppDemo.Instance.IsDemoAccount);
		}

		[Test]
		public void GetInvalidResponseTest()
		{
			var response = AppDemo.Instance.GetInvalidResponse();
			Assert.IsEmpty(response.Key);
			Assert.AreEqual(response.Value, HttpStatusCode.BadRequest);
		}

		[Test]
		public void GetDemoResponseTest()
		{
			var availableTestsResponse = AppDemo.Instance.GetDemoResponse(AppDemoType.AvailableTests);
			Assert.AreEqual(availableTestsResponse.Value, HttpStatusCode.OK);
			var conceptTreeResponse = AppDemo.Instance.GetDemoResponse(AppDemoType.ConceptTree);
			Assert.AreEqual(conceptTreeResponse.Value, HttpStatusCode.OK);
			var filesResponse = AppDemo.Instance.GetDemoResponse(AppDemoType.Files);
			Assert.AreEqual(filesResponse.Value, HttpStatusCode.OK);
			var infoLecturersResponse = AppDemo.Instance.GetDemoResponse(AppDemoType.InfoLecturers);
			Assert.AreEqual(infoLecturersResponse.Value, HttpStatusCode.OK);
			var labsResponse = AppDemo.Instance.GetDemoResponse(AppDemoType.Labs);
			Assert.AreEqual(labsResponse.Value, HttpStatusCode.OK);
			var labsResultsResponse = AppDemo.Instance.GetDemoResponse(AppDemoType.LabsResults);
			Assert.AreEqual(labsResultsResponse.Value, HttpStatusCode.OK);
			var loginResponse = AppDemo.Instance.GetDemoResponse(AppDemoType.Login);
			Assert.AreEqual(loginResponse.Value, HttpStatusCode.OK);
			var newsResponse = AppDemo.Instance.GetDemoResponse(AppDemoType.News);
			Assert.AreEqual(newsResponse.Value, HttpStatusCode.OK);
			var ptsResponse = AppDemo.Instance.GetDemoResponse(AppDemoType.PracticalTestStatistics);
			Assert.AreEqual(ptsResponse.Value, HttpStatusCode.OK);
			var profileInfoResponse = AppDemo.Instance.GetDemoResponse(AppDemoType.ProfileInfo);
			Assert.AreEqual(profileInfoResponse.Value, HttpStatusCode.OK);
			var profileInfoSubjectsResponse = AppDemo.Instance.GetDemoResponse(AppDemoType.ProfileInfoSubjects);
			Assert.AreEqual(profileInfoSubjectsResponse.Value, HttpStatusCode.OK);
			var recommendationsResponse = AppDemo.Instance.GetDemoResponse(AppDemoType.Recommendations);
			Assert.AreEqual(recommendationsResponse.Value, HttpStatusCode.OK);
			var rootConceptsResponse = AppDemo.Instance.GetDemoResponse(AppDemoType.RootConcepts);
			Assert.AreEqual(rootConceptsResponse.Value, HttpStatusCode.OK);
			var statisticsResponse = AppDemo.Instance.GetDemoResponse(AppDemoType.Statistics);
			Assert.AreEqual(statisticsResponse.Value, HttpStatusCode.OK);
			var testResponse = AppDemo.Instance.GetDemoResponse(AppDemoType.Test);
			Assert.AreEqual(testResponse.Value, HttpStatusCode.OK);
			var testAnswerAndGetNextResponse = AppDemo.Instance.GetDemoResponse(AppDemoType.TestAnswerAndGetNext);
			Assert.AreEqual(testAnswerAndGetNextResponse.Value, HttpStatusCode.OK);
			var testNextQuestionResponse = AppDemo.Instance.GetDemoResponse(AppDemoType.TestNextQuestion);
			Assert.AreEqual(testNextQuestionResponse.Value, HttpStatusCode.OK);
			var testUserAnswersResponse = AppDemo.Instance.GetDemoResponse(AppDemoType.TestUserAnswers);
			Assert.AreEqual(testUserAnswersResponse.Value, HttpStatusCode.OK);
			var noneResponse = AppDemo.Instance.GetDemoResponse(AppDemoType.None);
			Assert.AreEqual(noneResponse.Value, HttpStatusCode.BadRequest);
			var profileInfoCalendarResponse = AppDemo.Instance.GetDemoResponse(AppDemoType.ProfileInfoCalendar);
			var todayDate = DateTime.Today;
			var todayDateString = todayDate.ToString("yyyy-MM-dd");
			var tomorrowDateString = todayDate.AddDays(1).ToString("yyyy-MM-dd");
			Assert.AreEqual(profileInfoCalendarResponse.Value, HttpStatusCode.OK);
			StringAssert.Contains(todayDateString, profileInfoCalendarResponse.Key);
			StringAssert.Contains(tomorrowDateString, profileInfoCalendarResponse.Key);
		}
	}
}
