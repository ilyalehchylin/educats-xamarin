using EduCATS.Helpers.Forms;
using EduCATS.Networking;
using Moq;
using NUnit.Framework;

namespace EduCATS.UnitTests
{
	[TestFixture]
	public class ServersTests
	{
		const string _localServerName = "172.16.11.72";
		const string _testServerName = "educats.by";
		const string _stableServerName = "educats.bntu.by";

		[SetUp]
		public void SetUp()
		{
			var mocked = Mock.Of<IPlatformServices>(m => m.Preferences.Server == _stableServerName);
			Servers.PlatformServices = mocked;
		}

		[Test]
		public void CurrentTest()
		{
			var actual = Servers.Current;
			Assert.AreEqual(_stableServerName, actual);
		}

		[Test]
		public void SetCurrentTest()
		{
			Servers.SetCurrent(_stableServerName);
			Assert.AreEqual(_stableServerName, Servers.Current);
		}

		[Test]
		public void GetServerTypeTest()
		{
			var localServerName = Servers.GetServerType(Servers.LocalAddress);
			Assert.AreEqual(_localServerName, localServerName);
			var testServerName = Servers.GetServerType(Servers.EduCatsAddress);
			Assert.AreEqual(_testServerName, testServerName);
			var stableServerName = Servers.GetServerType(Servers.EduCatsBntuAddress);
			Assert.AreEqual(_stableServerName, stableServerName);
		}

		[Test]
		public void GetNullServerTypeTest()
		{
			var actual = Servers.GetServerType(null);
			Assert.IsNull(actual);
		}
	}
}
