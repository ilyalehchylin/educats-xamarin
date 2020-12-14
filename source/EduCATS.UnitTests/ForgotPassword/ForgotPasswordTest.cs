using EduCATS.Helpers.Forms;
using EduCATS.Pages.ForgotPassword.ViewModels;
using Moq;
using NUnit.Framework;
using System;

namespace EduCATS.UnitTests
{
	[TestFixture]
	public class ForgotPasswordTest
	{
		[TestCase("134535QWergf", "134535QWergf")]
		[TestCase("123456qwer", "123456qwer")]
		[Test]
		public void PasswordEqualsConfirmPassword_Equals_AreEquals(string newPassword, string confirmPassword)
		{
			var actual = newPassword;
			var expected = confirmPassword;
			Assert.AreEqual(actual, expected);
		}

		[TestCase("134535QWergf", "134535Qergf")]
		[TestCase("13453323vvQWergf", "13453wfsd332")]
		[Test]
		public void PasswordEqualsConfirmPassword_Equals_NotEquals(string newPassword, string confirmPassword)
		{
			var actual = newPassword;
			var expected = confirmPassword;
			Assert.AreNotEqual(actual, expected);
		}

		[TestCase("524635746gdf")]
		[TestCase("fkjdhsgh392321")]
		[Test]
		public void PasswordLength_PasswordLengthGreaterThan6_Greater(string newPassword)
		{
			var actual = newPassword.Length;
			var expected = 6;
			Assert.Greater(actual, expected);
		}

		[TestCase("524f")]
		[TestCase("fkj")]
		[Test]
		public void PasswordLength_PasswordLengthGreaterThan6_Less(string newPassword)
		{
			var actual = newPassword.Length;
			var expected = 6;
			Assert.Less(actual, expected);
		}

		[TestCase("524f00134293283243232423h4b33333333333333333333333333333333")]
		[TestCase("fkjdgshkjgfdsghkljjhgfdtrt25525234t2tt2t2t32t23t232t3t23t23t23")]
		[Test]
		public void PasswordLength_PasswordLengthLessThan30_Great(string password)
		{
			var actual = password.Length;
			var expected = 30;
			Assert.Greater(actual, expected);
		}

		[TestCase("524f00134293283243232423h4b")]
		[TestCase("fkjdgshkjgfdsghkljjhgfd")]
		[Test]
		public void PasswordLength_PasswordLengthLessThan30_Less(string newPassword)
		{
			var actual = newPassword.Length;
			var expected = 30;
			Assert.Less(actual, expected);
		}

		[TestCase("Hello", "Hello2345", "Hello2345", "Privet!", "ghdjskdfjsn")]
		[Test]
		public void CheckCredentials_NoEmptyFields_True(string username, string newPassword, 
			string confirmPassword, string answerToSecretQuestion, string questionId)
		{
			try
			{
				var mockedServices = Mock.Of<IPlatformServices>();
				var forgotPassword = new ForgotPasswordPageViewModel(mockedServices);
				forgotPassword.UserName = username;
				forgotPassword.NewPassword = newPassword;
				forgotPassword.ConfirmPassword = confirmPassword;
				forgotPassword.AnswerToSecretQuestion = answerToSecretQuestion;
				forgotPassword.QuestionId = questionId;
				var actual = forgotPassword.checkCredentials();
				Assert.IsTrue(actual);
			}
			catch (Exception ex)
			{
				Assert.Fail(ex.Message);
			}
		}

		[TestCase("Hello", "Hello2345", "Hello2345", null, "ghdjskdfjsn")]
		[Test]
		public void CheckCredentials_NoEmptyFields_False(string username, string newPassword,
			string confirmPassword, string answerToSecretQuestion, string questionId)
		{
			try
			{
				var mockedServices = Mock.Of<IPlatformServices>();
				var forgotPassword = new ForgotPasswordPageViewModel(mockedServices);
				forgotPassword.UserName = username;
				forgotPassword.NewPassword = newPassword;
				forgotPassword.ConfirmPassword = confirmPassword;
				forgotPassword.AnswerToSecretQuestion = answerToSecretQuestion;
				forgotPassword.QuestionId = questionId;
				var actual = forgotPassword.checkCredentials();
				Assert.IsFalse(actual);
			}
			catch (Exception ex)
			{
				Assert.Fail(ex.Message);
			}
		}
	}
}
