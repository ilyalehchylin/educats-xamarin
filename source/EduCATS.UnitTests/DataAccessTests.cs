using System.Threading.Tasks;
using EduCATS.Data;
using Moq;
using NUnit.Framework;

namespace EduCATS.UnitTests
{
	[TestFixture]
	public class DataAccessTests
	{
		const string _key = "key";
		const string _message = "error";

		[Test]
		public async Task GetSingleTest()
		{
			DataAccess<object>.IsCheckConnectionDisabled = true;
			var mock = new Mock<DataAccess<object>>(_message, null, _key);
			var actual = await mock.Object.GetSingle();
			Assert.IsNotNull(actual);
		}
	}
}
