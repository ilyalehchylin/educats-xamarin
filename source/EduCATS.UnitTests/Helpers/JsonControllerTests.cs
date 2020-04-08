using EduCATS.Helpers.Json;
using EduCATS.Networking.Models.Login;
using Moq;
using NUnit.Framework;

namespace EduCATS.UnitTests
{
	[TestFixture]
	public class JsonControllerTests
	{
		const string _emptyJson = "{}";
		const string _nonValidJson = "Not JSON";
		const string _validJson = "{ \"data\": 25 }";
		const string _validJsonArray = "[ { \"data\": 25 }, { \"data\": 50 } ]";

		const string _username = "admin";
		const string _password = "123";

		static readonly string _userJson = $"{{ \"username\": \"{_username}\", \"password\": \"{_password}\" }}";

		[Test]
		public void IsSingleJsonValidTest()
		{
			var actual = JsonController.IsJsonValid(_validJson);
			Assert.AreEqual(true, actual);
		}

		[Test]
		public void IsArrayJsonValidTest()
		{
			var actual = JsonController.IsJsonValid(_validJsonArray);
			Assert.AreEqual(true, actual);
		}

		[Test]
		public void IsNonJsonValidTest()
		{
			var actual = JsonController.IsJsonValid(_nonValidJson);
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
			Assert.AreEqual(_emptyJson, actual);
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

			var actual = JsonController<UserCredentials>.ConvertJsonToObject(_userJson);
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
