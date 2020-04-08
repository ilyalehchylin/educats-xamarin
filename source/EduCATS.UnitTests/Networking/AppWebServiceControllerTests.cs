using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using EduCATS.Helpers.Forms;
using EduCATS.Networking.AppServices;
using Moq;
using NUnit.Framework;

namespace EduCATS.UnitTests
{
	[TestFixture]
	public class AppWebServiceControllerTests
	{
		const string _apiURL = "https://educats-api-test.com/";

		[Test]
		public async Task SendRequestTest()
		{
			var mocked = Mock.Of<IPlatformServices>(ps => ps.Device.CheckConnectivity() == true);
			var serviceController = new AppWebServiceController(mocked);
			await serviceController.SendRequest(HttpMethod.Get, _apiURL);
			Assert.AreEqual(HttpStatusCode.BadRequest, serviceController.StatusCode);
			Assert.AreEqual(string.Empty, serviceController.Json);
		}

		[Test]
		public async Task SendNoConnectionRequestTest()
		{
			var mocked = Mock.Of<IPlatformServices>(ps => ps.Device.CheckConnectivity() == false);
			var serviceController = new AppWebServiceController(mocked);
			await serviceController.SendRequest(HttpMethod.Get, _apiURL);
			Assert.AreEqual(HttpStatusCode.ServiceUnavailable, serviceController.StatusCode);
			Assert.AreEqual(null, serviceController.Json);
		}
	}
}
