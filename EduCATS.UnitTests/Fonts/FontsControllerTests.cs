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
		const string _defaultFont = "font_default";
		const string _robotoFont = "Roboto";
		const string _robotoFontRegular = "Roboto-Regular";

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
		public void GetRegularFontAndroidTest()
		{
			var actual = FontsController.GetFont(_testRegularFont, false);
			Assert.AreEqual(_testRegularFont, actual);
		}

		[Test]
		public void GetBoldFontAndroidTest()
		{
			var actual = FontsController.GetFont(_testRegularFont, true);
			Assert.AreEqual(_testBoldFont, actual);
		}

		[Test]
		public void GetFontsTest()
		{
			var actual = FontsController.GetFonts();
			Assert.IsNotEmpty(actual);
		}

		[Test]
		public void SetDefaultFontTest()
		{
			var mock = new Mock<IPlatformServices>();
			mock.Setup(ps => ps.Preferences.Font).Returns(_defaultFont);
			FontsController.Initialize(mock.Object);
			FontsController.SetFont(_defaultFont);
			var actual = FontsController.GetCurrentFont();
			Assert.IsNull(actual);
		}

		[Test]
		public void SetFontTest()
		{
			var mock = new Mock<IPlatformServices>();
			mock.Setup(ps => ps.Preferences.Font).Returns(_robotoFontRegular);
			FontsController.Initialize(mock.Object);
			FontsController.SetFont(_robotoFontRegular);
			var actual = FontsController.GetCurrentFont();
			Assert.AreEqual(_robotoFontRegular, actual);
		}

		[Test]
		public void GetFontNameTest()
		{
			var actual = FontsController.GetFontName(_robotoFontRegular);
			Assert.AreEqual(_robotoFont, actual);
		}

		[Test]
		public void GetNullFontNameTest()
		{
			var actual = FontsController.GetFontName(null);
			Assert.IsNull(actual);
		}

		[Test]
		public void GetFontWithoutAliasNameTest()
		{
			var actual = FontsController.GetFontName(_robotoFont);
			Assert.AreEqual(_robotoFont, actual);
		}

		[Test]
		public void GetNullBoldFontAndroidTest()
		{
			var actual = FontsController.GetFont(null, true);
			Assert.IsNull(actual);
		}

		[Test]
		public void GetNullBoldFontIOSTest()
		{
			var actual = FontsController.GetFont(null, true);
			Assert.IsNull(actual);
		}
	}
}
