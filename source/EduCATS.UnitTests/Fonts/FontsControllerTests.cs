using System.Reflection;
using EduCATS.Constants;
using EduCATS.Fonts;
using EduCATS.Helpers.Forms;
using Moq;
using NUnit.Framework;
using Nyxbull.Plugins.CrossLocalization;

namespace EduCATS.UnitTests
{
	[TestFixture]
	public class FontsControllerTests
	{
		const string _testRegularFont = "TestFont-Regular";
		const string _testBoldFont = "TestFont-Bold";
		const string _androidRuntimePlatform = "Android";
		const string _iosRuntimePlatform = "iOS";
		const string _defaultFont = "font_default";
		const string _robotoFont = "Roboto";
		const string _robotoFontRegular = "Roboto-Regular";

		IPlatformServices _mockedAndroidServices;
		IPlatformServices _mockedIOSServices;

		[SetUp]
		public void SetUp()
		{
			_mockedAndroidServices = Mock.Of<IPlatformServices>(m =>
				m.Device.GetRuntimePlatform() == _androidRuntimePlatform);

			_mockedIOSServices = Mock.Of<IPlatformServices>(m =>
				m.Device.GetRuntimePlatform() == _iosRuntimePlatform);

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
		public void GetRegularFontAndroidTest()
		{
			FontsController.Initialize(_mockedAndroidServices);
			var expected = $"{_testRegularFont}.ttf#{_testRegularFont}";
			var actual = FontsController.GetFont(_testRegularFont, false);
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetRegularFontIOSTest()
		{
			var mockedServices = Mock.Of<IPlatformServices>(m =>
				m.Device.GetRuntimePlatform() == _iosRuntimePlatform);

			FontsController.Initialize(_mockedIOSServices);
			var actual = FontsController.GetFont(_testRegularFont, false);
			Assert.AreEqual(_testRegularFont, actual);
		}

		[Test]
		public void GetBoldFontAndroidTest()
		{
			FontsController.Initialize(_mockedAndroidServices);
			var expected = $"{_testBoldFont}.ttf#{_testBoldFont}";
			var actual = FontsController.GetFont(_testRegularFont, true);
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetBoldFontIOSTest()
		{
			FontsController.Initialize(_mockedIOSServices);
			var actual = FontsController.GetFont(_testRegularFont, true);
			Assert.AreEqual(_testBoldFont, actual);
		}

		[Test]
		public void GetFontsTest()
		{
			FontsController.Initialize(_mockedIOSServices);
			var actual = FontsController.GetFonts();
			Assert.IsNotEmpty(actual);
		}

		[Test]
		public void SetDefaultFontTest()
		{
			var mock = Mock.Get(_mockedIOSServices);
			mock.Setup(ps => ps.Preferences.Font).Returns(_defaultFont);
			FontsController.Initialize(mock.Object);
			FontsController.SetFont(_defaultFont);
			var actual = FontsController.GetCurrentFont();
			Assert.IsNull(actual);
		}

		[Test]
		public void SetFontTest()
		{
			var mock = Mock.Get(_mockedIOSServices);
			mock.Setup(ps => ps.Preferences.Font).Returns(_robotoFontRegular);
			FontsController.Initialize(mock.Object);
			FontsController.SetFont(_robotoFontRegular);
			var actual = FontsController.GetCurrentFont();
			Assert.AreEqual(_robotoFontRegular, actual);
		}

		[Test]
		public void GetFontNameTest()
		{
			FontsController.Initialize(_mockedIOSServices);
			var actual = FontsController.GetFontName(_robotoFontRegular);
			Assert.AreEqual(_robotoFont, actual);
		}

		[Test]
		public void GetNullFontNameTest()
		{
			FontsController.Initialize(_mockedIOSServices);
			var actual = FontsController.GetFontName(null);
			Assert.IsNull(actual);
		}

		[Test]
		public void GetFontWithoutAliasNameTest()
		{
			FontsController.Initialize(_mockedIOSServices);
			var actual = FontsController.GetFontName(_robotoFont);
			Assert.AreEqual(_robotoFont, actual);
		}

		[Test]
		public void GetNullBoldFontAndroidTest()
		{
			FontsController.Initialize(_mockedAndroidServices);
			var actual = FontsController.GetFont(null, true);
			Assert.IsNull(actual);
		}

		[Test]
		public void GetNullBoldFontIOSTest()
		{
			FontsController.Initialize(_mockedIOSServices);
			var actual = FontsController.GetFont(null, true);
			Assert.IsNull(actual);
		}
	}
}
