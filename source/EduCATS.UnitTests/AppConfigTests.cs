using System;
using EduCATS.Configuration;
using EduCATS.Constants;
using EduCATS.Fonts;
using EduCATS.Helpers.Files;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Logs;
using EduCATS.Themes;
using EduCATS.Themes.Templates;
using MonkeyCache.FileStore;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Nyxbull.Plugins.CrossLocalization;

namespace EduCATS.UnitTests
{
	[TestFixture]
	public class AppConfigTests
	{
		const string _hexColor = "#000000";
		const string _defaultLanguageCode = "en";
		const string _defaultTheme = "THEME_DEFAULT";
		const string _defaultFont = "font_default";

		[SetUp]
		public void SetUp()
		{
			var filePath = "file.txt";
			var fileMock = new Mock<IFileManager>();
			fileMock.Setup(m => m.Create(filePath)).Verifiable();
			fileMock.Setup(m => m.Delete(filePath)).Verifiable();
			fileMock.Setup(m => m.GetFileSize(filePath)).Verifiable();
			AppLogs.FileManager = fileMock.Object;

			var mocked = Mock.Of<IPlatformServices>(ps =>
				ps.Preferences.Font == _defaultFont &&
				ps.Preferences.Theme == _defaultTheme &&
				ps.Preferences.LanguageCode == _defaultLanguageCode &&
				ps.Device.GetAppDataDirectory() == "");

			var mock = Mock.Get(mocked);
			mock.Setup(a => a.Device.SetNativeTheme(_hexColor)).Verifiable();

			AppConfig.InitialSetup(mock.Object);
		}

		[Test]
		public void SetupLogsTest()
		{
			try {
				var actual = AppLogs.ReadLog();
				Assert.IsNull(actual);
			} catch (Exception ex) {
				Assert.Fail(ex.Message);
			}
		}

		[Test]
		public void SetupLocalizationPackageTest()
		{
			var actual = CrossLocalization.CurrentLanguageCode;
			Assert.AreEqual(_defaultLanguageCode, actual);
		}

		[Test]
		public void SetupCachingPackageTest()
		{
			var actual = Barrel.ApplicationId;
			Assert.AreEqual(actual, GlobalConsts.AppId);
		}

		[Test]
		public void SetupThemeTest()
		{
			var actual = JsonConvert.SerializeObject(Theme.Current);
			var expected = JsonConvert.SerializeObject(new DefaultTheme());
			Assert.AreEqual(actual, expected);
		}

		[Test]
		public void SetupFontsTest()
		{
			var actual = FontsController.GetCurrentFont();
			Assert.IsNull(actual);
		}
	}
}
