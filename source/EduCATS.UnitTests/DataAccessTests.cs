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

		Mock<DataAccess<object>> _mock;

		[SetUp]
		public void SetUp()
		{
			_mock = new Mock<DataAccess<object>>(_message, null, _key);
			_mock.Setup(m => m.CheckConnectionEstablished()).Returns(true);
		}

		[Test]
		public async Task GetSingleTest()
		{
			var actual = await _mock.Object.GetSingle();
			Assert.IsNotNull(actual);
		}

		[Test]
		public async Task GetListTest()
		{
			var actual = await _mock.Object.GetList();
			Assert.IsNotNull(actual);
		}

		[Test]
		public void CheckConnectionTest()
		{
			var actual = _mock.Object.CheckConnectionEstablished();
			Assert.AreEqual(true, actual);
		}
	}
}
