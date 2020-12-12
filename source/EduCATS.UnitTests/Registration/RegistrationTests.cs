using System;
using EduCATS.Data.Models;
using EduCATS.Helpers.Forms;
using EduCATS.Pages.Registration.ViewModels;
using Moq;
using NUnit.Framework;

namespace EduCATS.UnitTests
{
	[TestFixture]
	public class RegistrationTests
	{
		[TestCase("134535QWergf", "134535QWergf")]
		[TestCase("123456qwer", "123456qwer")]
		[Test]
		public void PasswordEqualsConfirmPassword_Equals_AreEquals(string password, string confirmPassword)
		{
			var actual = password;
			var expected = confirmPassword;
			Assert.AreEqual(actual, expected);
		}

		[TestCase("134535QWergf", "134535Qergf")]
		[TestCase("13453323vvQWergf", "13453wfsd332")]
		[Test]
		public void PasswordEqualsConfirmPassword_Equals_NotEquals(string password, string confirmPassword)
		{
			var actual = password;
			var expected = confirmPassword;
			Assert.AreNotEqual(actual, expected);
		}

		[TestCase("524635746gdf")]
		[TestCase("fkjdhsgh392321")]
		[Test]
		public void PasswordLength_PasswordLengthGreaterThan6_Greater(string password)
		{
			var actual = password.Length;
			var expected = 6;
			Assert.Greater(actual, expected);
		}

		[TestCase("524f")]
		[TestCase("fkj")]
		[Test]
		public void PasswordLength_PasswordLengthGreaterThan6_Less(string password)
		{
			var actual = password.Length;
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
		public void PasswordLength_PasswordLengthLessThan30_Less(string password)
		{
			var actual = password.Length;
			var expected = 30;
			Assert.Less(actual, expected);
		}


		[TestCase("524f00134293283243232423h4b")]
		[TestCase("fkjdgshkjgfdsghkljjhgfd")]
		[Test]
		public void UserNameLength_UserNameLengthGreaterThan3_Greater(string username)
		{
			var actual = username.Length;
			var expected = 3;
			Assert.Greater(actual, expected);
		}

		[TestCase("5")]
		[TestCase("fk")]
		[Test]
		public void UserNameLength_UserNameLengthGreaterThan3_Less(string username)
		{
			var actual = username.Length;
			var expected = 3;
			Assert.Less(actual, expected);
		}

		[TestCase("djhfjfkhfgdhdfgh")]
		[TestCase("djhfjfkdfdk")]
		[Test]
		public void UserNameLength_UserNemLessThan30_Less(string username)
		{
			var actual = username.Length;
			var expected = 30;
			Assert.Less(actual, expected);
		}

		[TestCase("djhfjfkhfgdhdf243023u93r4238r032jr42034jr02384r0823j4r02834gh")]
		[TestCase("djhfjfkfdsjksjlkfdkffdsjflkdlkfjsdlkfsfkljfdksljfdskljdfdk")]
		[Test]
		public void UserNameLength_UserNemLessThan30_Great(string username)
		{
			var actual = username.Length;
			var expected = 30;
			Assert.Greater(actual, expected);
		}

		[TestCase("dhdjds3323")]
		[TestCase("3n423jn243kk32lkj")]
		[Test]
		public void LatinPassword_LatinLettersInPassword_True(string password)
		{
			var mockedServices = Mock.Of<IPlatformServices>();
			var register = new RegistrationPageViewModel(mockedServices);
			register.Password = password;
			var actual = register.LatinPassword();
			Assert.IsTrue(actual);
		}

		[TestCase("Привет")]
		[TestCase("ггрнн5533ьь")]
		[Test]
		public void LatinPassword_LatinLettersInPassword_False(string password)
		{
			var mockedServices = Mock.Of<IPlatformServices>();
			var register = new RegistrationPageViewModel(mockedServices);
			register.Password = password;
			var actual = register.LatinPassword();
			Assert.IsFalse(actual);
		}

		[TestCase("1234eeeeffgg")]
		[TestCase("fdsj3332")]
		[Test]
		public void LatinUserName_LatinLettersInUserName_True(string username)
		{
			var mockedServices = Mock.Of<IPlatformServices>();
			var register = new RegistrationPageViewModel(mockedServices);
			register.UserName = username;
			var actual = register.LatinUserName();
			Assert.IsTrue(actual);
		}

		[TestCase("1234аорыв")]
		[TestCase("ываываваоы32323")]
		[Test]
		public void LatinUserName_LatinLettersInUserName_False(string username)
		{
			var mockedServices = Mock.Of<IPlatformServices>();
			var register = new RegistrationPageViewModel(mockedServices);
			register.UserName = username;
			var actual = register.LatinUserName();
			Assert.IsFalse(actual);
		}

		[TestCase("1234FFDsse")]
		[TestCase("dsagfD32323")]
		[Test]
		public void UpperCaseLettersInPassword_UpperCaseLettersMoreThan1_1(string password)
		{
			var expected = 1;
			var mockedServices = Mock.Of<IPlatformServices>();
			var register = new RegistrationPageViewModel(mockedServices);
			register.Password = password;
			var actual = register.UpperCaseLettersInPassword();
			Assert.GreaterOrEqual(actual, expected);
		}

		[TestCase("1234dsse")]
		[TestCase("dsagf32323")]
		[Test]
		public void UpperCaseLettersInPassword_UpperCaseLettersLessThan1_0(string password)
		{
			var expected = 1;
			var mockedServices = Mock.Of<IPlatformServices>();
			var register = new RegistrationPageViewModel(mockedServices);
			register.Password = password;
			var actual = register.UpperCaseLettersInPassword();
			Assert.LessOrEqual(actual, expected);
		}

		[TestCase("Roman1234", "12345qwer", "12345qwer", "name1", "name2", "1")]
		[Test]
		public void CheckCredentials_NoEmptyFields_True(string userName, string password, string confirmPassword,
			string name, string surName, int groupId)
		{
			try
			{
				var group = new GroupItemModel();
				var mockedServices = Mock.Of<IPlatformServices>();
				var reg = new RegistrationPageViewModel(mockedServices);
				reg.UserName = userName;
				reg.Password = password;
				reg.ConfirmPassword = confirmPassword;
				reg.Name = name;
				reg.Surname = surName;
				reg.Group = group;
				group.Id = groupId;
				var actual = reg.checkCredentials();
				Assert.IsTrue(actual);
			}
			catch (Exception ex)
			{
				Assert.Fail(ex.Message);
			}
		}

		[TestCase("Roman1234", "12345qwer", "12345qwer", "name1", null, "1")]
		[Test]
		public void CheckCredentials_NoEmptyFields_False(string userName, string password, string confirmPassword,
			string name, string surName, int groupId)
		{
			try
			{
				var group = new GroupItemModel();
				var mockedServices = Mock.Of<IPlatformServices>();
				var reg = new RegistrationPageViewModel(mockedServices);
				reg.UserName = userName;
				reg.Password = password;
				reg.ConfirmPassword = confirmPassword;
				reg.Name = name;
				reg.Surname = surName;
				reg.Group = group;
				group.Id = groupId;
				var actual = reg.checkCredentials();
				Assert.IsFalse(actual);
			}
			catch (Exception ex)
			{
				Assert.Fail(ex.Message);
			}
		}
	}
}