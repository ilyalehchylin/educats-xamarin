using EduCATS.Fonts;
using EduCATS.Helpers.Forms;
using Moq;
using NUnit.Framework;
using Xamarin.Forms;

namespace EduCATS.UnitTests
{
	[TestFixture]
	public class FontSizeControllerTests
	{
		const double _small = 20.0;
		const double _medium = 30.0;
		const double _large = 40.0;
		const double _extraLarge = 45.0;
		const double _extraLargeWithAddition = 50.0;
		const double _vwAddition = 1;

		Mock<IPlatformServices> _fontLargeMock;
		Mock<IPlatformServices> _fontSmallMock;

		[SetUp]
		public void SetUp()
		{
			var fontLargeMocked = Mock.Of<IPlatformServices>(ps => ps.Preferences.IsLargeFont == true);
			_fontLargeMock = setupMock(fontLargeMocked);

			var fontSmallMocked = Mock.Of<IPlatformServices>(ps => ps.Preferences.IsLargeFont == false);
			_fontSmallMock = setupMock(fontSmallMocked);
		}

		[Test]
		public void GetLargeSizeTest()
		{
			FontSizeController.PlatformServices = _fontLargeMock.Object;
			var actual_1 = FontSizeController.GetSize(0, typeof(object));
			Assert.AreEqual(_large, actual_1);
			var actual_2 = FontSizeController.GetSize((NamedSize)1, typeof(object));
			Assert.AreEqual(_medium, actual_2);
			var actual_3 = FontSizeController.GetSize((NamedSize)2, typeof(object));
			Assert.AreEqual(_large, actual_3);
			var actual_4 = FontSizeController.GetSize((NamedSize)3, typeof(object));
			Assert.AreEqual(_extraLarge, actual_4);
			var actual_5 = FontSizeController.GetSize((NamedSize)4, typeof(object));
			Assert.AreEqual(_extraLargeWithAddition, actual_5);
		}

		[Test]
		public void GetSmallSizeTest()
		{
			FontSizeController.PlatformServices = _fontSmallMock.Object;
			var actual = FontSizeController.GetSize(0, typeof(object));
			Assert.AreEqual(_medium, actual);
		}

		[Test]
		public void GetDynamicLargeSizeTest()
		{
			FontSizeController.PlatformServices = _fontLargeMock.Object;
			var actual = FontSizeController.GetDynamicSize(_medium);
			var expected = _medium + _vwAddition;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetDynamicSmallSizeTest()
		{
			FontSizeController.PlatformServices = _fontSmallMock.Object;
			var actual = FontSizeController.GetDynamicSize(_medium);
			Assert.AreEqual(_medium, actual);
		}

		Mock<IPlatformServices> setupMock(IPlatformServices mocked)
		{
			var mock = Mock.Get(mocked);
			mock.Setup(ps => ps.Device.GetNamedSize(0, typeof(object))).Returns(_medium);
			mock.Setup(ps => ps.Device.GetNamedSize(1, typeof(object))).Returns(_small);
			mock.Setup(ps => ps.Device.GetNamedSize(2, typeof(object))).Returns(_medium);
			mock.Setup(ps => ps.Device.GetNamedSize(3, typeof(object))).Returns(_large);
			mock.Setup(ps => ps.Device.GetNamedSize(4, typeof(object))).Returns(_extraLarge);
			return mock;
		}
	}
}
