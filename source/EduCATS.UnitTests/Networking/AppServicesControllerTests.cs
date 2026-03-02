using System.Net;
using System.Threading.Tasks;
using EduCATS.Demo;
using EduCATS.Helpers.Files;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Forms.Devices;
using EduCATS.Helpers.Forms.Settings;
using EduCATS.Helpers.Logs;
using EduCATS.Networking;
using EduCATS.Networking.AppServices;
using Moq;
using NUnit.Framework;

namespace EduCATS.UnitTests
{
	[TestFixture]
	public class AppServicesControllerTests
	{
		const string _invalidApiURL = "not-a-valid-url";

		[SetUp]
		public void SetUp()
		{
			RequestController.ResetHttpClient();
			AppDemo.Instance.IsDemoAccount = false;
			var logs = new Mock<IFileManager>();
			logs.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);
			logs.Setup(m => m.GetFileSize(It.IsAny<string>())).Returns(0);
			AppLogs.FileManager = logs.Object;
			AppLogs.Initialize(string.Empty);
		}

		[TearDown]
		public void TearDown()
		{
			AppDemo.Instance.IsDemoAccount = false;
		}

		[Test]
		public async Task RequestGetReturnsBadRequestForInvalidUrl()
		{
			AppServicesController.PlatformServices = createPlatformServices(isConnected: true);
			var response = await AppServicesController.Request(_invalidApiURL);

			Assert.AreEqual(HttpStatusCode.BadRequest, response.Value);
			Assert.AreEqual(string.Empty, response.Key);
		}

		[Test]
		public async Task RequestPostReturnsBadRequestForInvalidUrl()
		{
			AppServicesController.PlatformServices = createPlatformServices(isConnected: true);
			var response = await AppServicesController.Request(_invalidApiURL, "{}");

			Assert.AreEqual(HttpStatusCode.BadRequest, response.Value);
			Assert.AreEqual(string.Empty, response.Key);
		}

		[Test]
		public async Task RequestGetReturnsServiceUnavailableWhenOffline()
		{
			AppServicesController.PlatformServices = createPlatformServices(isConnected: false);
			var response = await AppServicesController.Request(_invalidApiURL);

			Assert.AreEqual(HttpStatusCode.ServiceUnavailable, response.Value);
			Assert.IsNull(response.Key);
		}

		[Test]
		public async Task RequestGetReturnsDemoInvalidResponseForNoneType()
		{
			AppDemo.Instance.IsDemoAccount = true;
			var response = await AppServicesController.Request(_invalidApiURL);

			Assert.AreEqual(HttpStatusCode.BadRequest, response.Value);
			Assert.AreEqual(string.Empty, response.Key);
		}

		[Test]
		public async Task RequestPostReturnsDemoContentForProvidedType()
		{
			AppDemo.Instance.IsDemoAccount = true;
			var response = await AppServicesController.Request(
				_invalidApiURL,
				"{}",
				AppDemoType.ProfileInfo);

			Assert.AreEqual(HttpStatusCode.OK, response.Value);
			Assert.IsFalse(string.IsNullOrWhiteSpace(response.Key));
		}

		static IPlatformServices createPlatformServices(bool isConnected)
		{
			var device = new Mock<IDevice>();
			device.Setup(d => d.CheckConnectivity()).Returns(isConnected);

			var preferences = new Mock<IPreferences>();
			preferences.SetupGet(p => p.Server).Returns("https://educats.by");
			preferences.SetupGet(p => p.AccessToken).Returns("token");

			return Mock.Of<IPlatformServices>(s =>
				s.Device == device.Object &&
				s.Preferences == preferences.Object);
		}
	}
}
