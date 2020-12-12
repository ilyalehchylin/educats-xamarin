using EduCATS.Constants;
using EduCATS.Data.Models;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Forms.Pages;
using EduCATS.Helpers.Logs;
using EduCATS.Pages.Registration.ViewModels;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Nyxbull.Plugins.CrossLocalization;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace EduCATS.UnitTests
{
	[TestFixture]
	public class RegistrationTests
	{
		const string userName = "Romanen";
		const string password = "Romanko123";
		const string confirmPassword = "Romanko123";
		const string name = "Romanenko";
		const string surName = "Romanenkok";
		const string Patronymic = "Romanenkokok";
		const int groupId = 6096;
		const int questionId = 1;
		const string answerToSecretQuestion = "HelloWorld";
		RegistrationPageViewModel register;

		[SetUp]
		public void SetUp()
		{
			var group = new GroupItemModel();
			var mockedServices = Mock.Of<IPlatformServices>();
			register = new RegistrationPageViewModel(mockedServices);
			register.UserName = userName;
			register.Password = password;
			register.ConfirmPassword = confirmPassword;
			register.Name = name;
			register.Surname = surName;
			register.Patronymic = Patronymic;
			register.Group = group;
			group.Id = groupId;
			register.QuestionId = "Mother's maiden name?";
			register.SelectedQuestionId = 1;
			register.AnswerToSecretQuestion = answerToSecretQuestion;
			var assembly = typeof(App).GetTypeInfo().Assembly;
			CrossLocalization.Initialize(
				assembly,
				GlobalConsts.RunNamespace,
				GlobalConsts.LocalizationDirectory);
			CrossLocalization.AddLanguageSupport(Languages.EN);
			CrossLocalization.AddLanguageSupport(Languages.RU);
			CrossLocalization.AddLanguageSupport(Languages.LT);
			CrossLocalization.AddLanguageSupport(Languages.BE);
			CrossLocalization.AddLanguageSupport(Languages.DE);
			CrossLocalization.AddLanguageSupport(Languages.PL);
			CrossLocalization.SetDefaultLanguage(Languages.EN.LangCode);
		}

		[Test]
		public void PasswordEqualsConfirmPassword_Equals_AreEquals()
		{
			var actual = register.Password;
			var expected = register.ConfirmPassword;
			Assert.AreEqual(actual, expected);
		}

		[Test]
		public void PasswordLength_PasswordLengthGreaterThan6_Greater()
		{
			var actual = register.Password.Length;
			var expected = 6;
			Assert.Greater(actual, expected);
		}

		[Test]
		public void PasswordLength_PasswordLengthLessThan30_Less()
		{
			var actual = register.Password.Length;
			var expected = 30;
			Assert.Less(actual, expected);
		}

		[Test]
		public void UserNameLength_UserNameLengthGreaterThan3_Greater()
		{
			var actual = register.UserName.Length;
			var expected = 3;
			Assert.Greater(actual, expected);
		}

		[Test]
		public void UserNameLength_UserNemLessThan30_Less()
		{
			var actual = register.UserName.Length;
			var expected = 30;
			Assert.Less(actual, expected);
		}

		[Test]
		public void LatinPassword_LatinLettersInPassword_True()
		{
			var actual = register.LatinPassword();
			Assert.IsTrue(actual);
		}

		[Test]
		public void LatinUserName_LatinLettersInUserName_True()
		{
			var actual = register.LatinUserName();
			Assert.IsTrue(actual);
		}

		[Test]
		public void UpperCaseLettersInPassword_UpperCaseLettersMoreThan1_1()
		{
			var actual = register.UpperCaseLettersInPassword();
			var expected = 1;
			Assert.GreaterOrEqual(actual, expected);
		}



		//[Test]
		//public async Task RegistrationPostAsync_Test()
		//{
		//	try
		//	{
		//		var mock = new Mock<IPlatformServices>();
		//		var viewModel = new RegistrationPageViewModel(mock.Object);
		//		var result = await viewModel.RegistrationPostAsync(userName, name, surName, Patronymic, password, confirmPassword,
		//			groupId, questionId, answerToSecretQuestion);
		//		Assert.IsNull(result.Key);
		//	}
		//	catch (Exception ex)
		//	{
		//		Assert.Fail(ex.Message);
		//	}
		//}

		[Test]
		public async Task SuccsesfullSignUp_Test()
		{
			try
			{
				var group = new GroupItemModel();
				var Mocked = new Mock<IPlatformServices>();
				Mocked.Setup(m => m.Dialogs.ShowLoading(CrossLocalization.Translate("chek_In"))).Verifiable();
				var reg = new RegistrationPageViewModel(Mocked.Object);
				reg.UserName = userName;
				reg.Password = password;
				reg.ConfirmPassword = confirmPassword;
				reg.Name = name;
				reg.Surname = surName;
				reg.Patronymic = Patronymic;
				reg.Group = group;
				group.Id = groupId;
				reg.SelectedQuestionId = 1;
				reg.AnswerToSecretQuestion = answerToSecretQuestion;
				reg.QuestionId = CrossLocalization.Translate("mother_last_name");
				var result = await reg.startRegister();
				Assert.IsNull(result.Result);
			}
			catch (Exception ex)
			{
				Assert.Fail(ex.Message);
			}
		}


		[Test]
		public void CheckCredentials_NoEmptyFields_True()
		{
			try
			{
				var actual = register.checkCredentials();
				Assert.IsTrue(actual);
			}
			catch (Exception ex)
			{
				Assert.Fail(ex.Message);
			}
		}
	}
}