using System;
using System.Reflection;
using System.Threading.Tasks;
using EduCATS.Constants;
using EduCATS.Data.Models;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Forms.Dialogs;
using EduCATS.Helpers.Forms.Pages;
using EduCATS.Helpers.Forms.Settings;
using EduCATS.Networking;
using EduCATS.Networking.AppServices;
using EduCATS.Pages.ForgotPassword.ViewModels;
using EduCATS.Pages.Login.ViewModels;
using EduCATS.Pages.Login.Views;
using EduCATS.Pages.Registration.ViewModels;
using Moq;
using NUnit.Framework;
using Nyxbull.Plugins.CrossLocalization;

namespace EduCATS.UnitTests
{
	[TestFixture]
	public class RegistrationTests
	{
		[TestCase("TestLecturer5", "TestLecturer5")]
		[Test]
		public async void RefreshToken_Test(string username, string password)
		{
			try
			{
				var mockedServices = Mock.Of<IPlatformServices>();
				mockedServices.Preferences = Mock.Of<IPreferences>();
				mockedServices.Preferences.Server = Servers.EduCatsAddress;
				var mockedLoginPageView = new Mock<LoginPageViewModel>(mockedServices).Object;
				mockedLoginPageView.Username = username;
				mockedLoginPageView.Password = password;
				var actual = await mockedLoginPageView.RefreshToken();
				Assert.IsNotEmpty(actual);
			}
			catch (Exception ex)
			{
				Assert.Fail(ex.Message);
			}
		}
	}
}