using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EduCATS.Networking;
using Moq;
using NUnit.Framework;

namespace EduCATS.UnitTests
{
	[TestFixture]
	public class RequestControllerTests
	{
		/*const string _apiURL = "https://educats-api-test.com/";

		Mock<RequestController> _mock;

		[SetUp]
		public void SetUp()
		{
			_mock = new Mock<RequestController>(_apiURL);
		}

		[Test]
		public void UriNullTest()
		{
			var mock = new Mock<RequestController>(null);
			Assert.IsNull(mock.Object.Uri);
		}

		[Test]
		public void UriTest()
		{
			Assert.AreEqual(new Uri(_apiURL), _mock.Object.Uri);
		}

		[Test]
		public void SetPostContentTest()
		{
			try {
				_mock.Object.SetPostContent("content", Encoding.UTF8, "application/json");
				return;
			} catch (Exception ex) {
				Assert.Fail(ex.Message);
			}
		}

		[Test]
		public async Task SendGetRequestTest()
		{
			var response = await _mock.Object.SendRequest(HttpMethod.Get);
			Assert.NotNull(response);
		}

		[Test]
		public async Task SendPostRequestTest()
		{
			var response = await _mock.Object.SendRequest(HttpMethod.Post);
			Assert.NotNull(response);
		}

		[Test]
		public async Task SendNullRequestTest()
		{
			var response = await _mock.Object.SendRequest(null);
			Assert.IsNull(response);
		}*/
	}
}
