using EduCATS.Helpers.Json;
using EduCATS.Networking.Models.Login;
using Moq;
using NUnit.Framework;

namespace EduCATS.UnitTests
{
	[TestFixture]
	public class JsonControllerTests
	{
		const string emptyJson = "{}";
		const string nonValidJson = "Not JSON";
		const string validJson = "{ \"data\": 25 }";
		const string validJsonArray = "[ { \"data\": 25 }, { \"data\": 50 } ]";

		const string username = "admin";
		const string password = "123";

		static string userJson = $"{{ \"username\": \"{username}\", \"password\": \"{password}\" }}";

		[Test]
		public void IsSingleJsonValidTest()
		{
			var actual = JsonController.IsJsonValid(validJson);
			Assert.AreEqual(true, actual);
		}

		[Test]
		public void IsArrayJsonValidTest()
		{
			var actual = JsonController.IsJsonValid(validJsonArray);
			Assert.AreEqual(true, actual);
		}

		[Test]
		public void IsNonJsonValidTest()
		{
			var actual = JsonController.IsJsonValid(nonValidJson);
			Assert.AreEqual(false, actual);
		}

		[Test]
		public void IsNullJsonValid()
		{
			var actual = JsonController.IsJsonValid(null);
			Assert.AreEqual(false, actual);
		}

		[Test]
		public void ConvertObjectToJsonNegativeTest()
		{
			var incorrectObject = new object();
			var actual = JsonController.ConvertObjectToJson(incorrectObject);
			Assert.AreEqual(emptyJson, actual);
		}

		[Test]
		public void ConvertNullObjectToJsonTest()
		{
			var actual = JsonController.ConvertObjectToJson(null);
			Assert.AreEqual(null, actual);
		}

		[Test]
		public void ConvertJsonToObjectTest()
		{
			var user = Mock.Of<UserCredentials>(
				u => u.Username == "admin" && u.Password == "123");

			var actual = JsonController<UserCredentials>.ConvertJsonToObject(userJson);
			Assert.AreEqual(user.Username, actual.Username);
			Assert.AreEqual(user.Password, actual.Password);
		}

		[Test]
		public void ConvertNullJsonToObjectTest()
		{
			var actual = JsonController<object>.ConvertJsonToObject(null);
			Assert.AreEqual(default, actual);
		}

		[Test]
		public void ConvertEmptyJsonToObjectTest()
		{
			var actual = JsonController<object>.ConvertJsonToObject("");
			Assert.AreEqual(default, actual);
		}
	}
}
