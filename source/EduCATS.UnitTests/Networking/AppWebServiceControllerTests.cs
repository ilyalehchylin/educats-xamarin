using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
	public class AppWebServiceControllerTests
	{
		const string _invalidApiURL = "not-a-valid-url";

		[SetUp]
		public void SetUp()
		{
			RequestController.ResetHttpClient();
			var logs = new Mock<IFileManager>();
			logs.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);
			logs.Setup(m => m.GetFileSize(It.IsAny<string>())).Returns(0);
			AppLogs.FileManager = logs.Object;
			AppLogs.Initialize(string.Empty);
		}

		[Test]
		public async Task SendRequestGetReturnsBadRequestForInvalidUrl()
		{
			var mocked = createPlatformServices(isConnected: true, server: "https://educats.by");
			var serviceController = new AppWebServiceController(mocked);
			await serviceController.SendRequest(HttpMethod.Get, _invalidApiURL);

			Assert.AreEqual(HttpStatusCode.BadRequest, serviceController.StatusCode);
			Assert.AreEqual(string.Empty, serviceController.Json);
		}

		[Test]
		public async Task SendRequestPostReturnsBadRequestForInvalidUrl()
		{
			var mocked = createPlatformServices(isConnected: true, server: "https://educats.by");
			var serviceController = new AppWebServiceController(mocked);
			await serviceController.SendRequest(HttpMethod.Post, _invalidApiURL, "{}");

			Assert.AreEqual(HttpStatusCode.BadRequest, serviceController.StatusCode);
			Assert.AreEqual(string.Empty, serviceController.Json);
		}

		[Test]
		public async Task SendRequestReturnsServiceUnavailableWhenNoConnectivity()
		{
			var mocked = createPlatformServices(isConnected: false, server: "https://educats.by");
			var serviceController = new AppWebServiceController(mocked);
			await serviceController.SendRequest(HttpMethod.Get, _invalidApiURL);

			Assert.AreEqual(HttpStatusCode.ServiceUnavailable, serviceController.StatusCode);
			Assert.IsNull(serviceController.Json);
		}

		static IPlatformServices createPlatformServices(bool isConnected, string server)
		{
			var device = new Mock<IDevice>();
			device.Setup(d => d.CheckConnectivity()).Returns(isConnected);

			var preferences = new Mock<IPreferences>();
			preferences.SetupGet(p => p.Server).Returns(server);
			preferences.SetupGet(p => p.AccessToken).Returns("token");

			return Mock.Of<IPlatformServices>(s =>
				s.Device == device.Object &&
				s.Preferences == preferences.Object);
		}
	}
}
