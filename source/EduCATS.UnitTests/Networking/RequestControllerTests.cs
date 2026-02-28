using System.Net.Http;
using System.Reflection;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Forms.Settings;
using EduCATS.Networking;
using Moq;
using NUnit.Framework;

namespace EduCATS.UnitTests
{
	[TestFixture]
	public class RequestControllerTests
	{
		static readonly BindingFlags _privateInstance =
			BindingFlags.Instance | BindingFlags.NonPublic;

		[Test]
		public void UriNullTest()
		{
			var controller = new RequestController(null);
			Assert.IsNull(controller.Uri);
		}

		[Test]
		public void UriTest()
		{
			const string url = "https://educats-api-test.com/";
			var controller = new RequestController(url);
			Assert.AreEqual(url, controller.Uri.ToString());
		}

		[Test]
		public void TimeoutConstantTest()
		{
			Assert.GreaterOrEqual(RequestController.RequestTimeoutSeconds, 30);
			Assert.AreEqual(
				RequestController.RequestTimeoutSeconds * 1000,
				RequestController.RequestTimeoutMilliseconds);
		}

		[Test]
		public void ShouldAttachAuthorizationHeaderReturnsFalseForLoginEndpoint()
		{
			var controller = createController("https://educats.by/Account/LoginJWT", "token");
			var shouldAttach = invokeShouldAttachAuthorizationHeader(controller);

			Assert.IsFalse(shouldAttach);
		}

		[Test]
		public void ShouldAttachAuthorizationHeaderReturnsFalseForRemoteApiEndpoint()
		{
			var controller = createController("https://educats.by/RemoteApi/Login", "token");
			var shouldAttach = invokeShouldAttachAuthorizationHeader(controller);

			Assert.IsFalse(shouldAttach);
		}

		[Test]
		public void ShouldAttachAuthorizationHeaderReturnsTrueForAuthorizedEndpoint()
		{
			var controller = createController("https://educats.by/Profile/GetProfileInfo", "token");
			var shouldAttach = invokeShouldAttachAuthorizationHeader(controller);

			Assert.IsTrue(shouldAttach);
		}

		[Test]
		public void SetAuthorizationHeaderSetsHeaderForAuthorizedEndpoint()
		{
			var controller = createController("https://educats.by/Profile/GetProfileInfo", "token");
			invokeSetAuthorizationHeader(controller);

			var client = getHttpClient(controller);
			Assert.NotNull(client.DefaultRequestHeaders.Authorization);
			Assert.AreEqual("token", client.DefaultRequestHeaders.Authorization.Scheme);
		}

		[Test]
		public void SetAuthorizationHeaderClearsHeaderForLoginEndpoint()
		{
			var controller = createController("https://educats.by/Account/LoginJWT", "token");
			invokeSetAuthorizationHeader(controller);

			var client = getHttpClient(controller);
			Assert.IsNull(client.DefaultRequestHeaders.Authorization);
		}

		RequestController createController(string url, string accessToken)
		{
			var preferences = Mock.Of<IPreferences>(p => p.AccessToken == accessToken);
			var services = Mock.Of<IPlatformServices>(s => s.Preferences == preferences);

			var controller = new RequestController(url) {
				_services = services
			};

			return controller;
		}

		static bool invokeShouldAttachAuthorizationHeader(RequestController controller)
		{
			var method = typeof(RequestController).GetMethod(
				"shouldAttachAuthorizationHeader",
				_privateInstance);
			return (bool)method.Invoke(controller, null);
		}

		static void invokeSetAuthorizationHeader(RequestController controller)
		{
			var method = typeof(RequestController).GetMethod(
				"setAuthorizationHeader",
				_privateInstance);
			method.Invoke(controller, null);
		}

		static HttpClient getHttpClient(RequestController controller)
		{
			var field = typeof(RequestController).GetField("_client", _privateInstance);
			return (HttpClient)field.GetValue(controller);
		}
	}
}
