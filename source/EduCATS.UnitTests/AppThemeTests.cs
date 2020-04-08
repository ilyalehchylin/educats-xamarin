using EduCATS.Helpers.Forms;
using EduCATS.Themes;
using EduCATS.Themes.Templates;
using Moq;
using NUnit.Framework;

namespace EduCATS.UnitTests
{
	[TestFixture]
	public class AppThemeTests
	{
		AppTheme _theme;

		[SetUp]
		public void SetUp()
		{
			var mocked = Mock.Of<IPlatformServices>(ps => ps.Preferences.Theme == AppTheme.ThemeDefault);
			var mock = Mock.Get(mocked);
			mock.Setup(a => a.Device.SetNativeTheme("#000000")).Verifiable();
			_theme = new AppTheme(mock.Object);
		}

		[Test]
		public void SetThemeTest()
		{
			_theme.SetTheme(AppTheme.ThemeDefault);
			var actual = Theme.Current;
			Assert.AreEqual(typeof(DefaultTheme), actual.GetType());
		}

		[Test]
		public void SetThemeFromPrefsTest()
		{
			_theme.SetTheme(AppTheme.ThemeDefault, true);
			var actual = Theme.Current;
			Assert.AreEqual(typeof(DefaultTheme), actual.GetType());
		}
	}
}
