using EduCATS.Helpers.Json;
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
		public void IsJsonValidNegativeTest()
		{
			var actual = JsonController.IsJsonValid(nonValidJson);
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
	}
}
