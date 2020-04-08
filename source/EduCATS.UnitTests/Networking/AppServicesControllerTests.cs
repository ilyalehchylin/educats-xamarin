using System.Net;
using System.Threading.Tasks;
using EduCATS.Helpers.Forms;
using EduCATS.Networking.AppServices;
using Moq;
using NUnit.Framework;

namespace EduCATS.UnitTests
{
	[TestFixture]
	public class AppServicesControllerTests
	{
		const string _apiURL = "https://educats-api-test.com/";

		[SetUp]
		public void SetUp()
		{
			var mocked = Mock.Of<IPlatformServices>(ps => ps.Device.CheckConnectivity() == true);
			AppServicesController.PlatformServices = mocked;
		}

		[Test]
		public async Task GetRequestTest()
		{
			var response = await AppServicesController.Request(_apiURL);
			Assert.AreEqual(HttpStatusCode.BadRequest, response.Value);
		}

		[Test]
		public async Task PostRequestTest()
		{
			var response = await AppServicesController.Request(_apiURL, "body");
			Assert.AreEqual(HttpStatusCode.BadRequest, response.Value);
		}
	}
}
