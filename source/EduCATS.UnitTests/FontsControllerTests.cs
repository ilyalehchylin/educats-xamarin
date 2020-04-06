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
	}
}
