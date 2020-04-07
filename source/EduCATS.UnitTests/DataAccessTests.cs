using System;
using System.Reflection;
using System.Threading.Tasks;
using EduCATS.Constants;
using EduCATS.Data;
using Moq;
using NUnit.Framework;
using Nyxbull.Plugins.CrossLocalization;

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

			var assembly = typeof(App).GetTypeInfo().Assembly;
			CrossLocalization.Initialize(
				assembly,
				GlobalConsts.RunNamespace,
				GlobalConsts.LocalizationDirectory);

			CrossLocalization.AddLanguageSupport(Languages.EN);
			CrossLocalization.SetDefaultLanguage(Languages.EN.LangCode);
			CrossLocalization.SetLanguage(Languages.EN.LangCode);
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

		[Test]
		public void ResetDataTest()
		{
			try {
				DataAccess.ResetData();
				return;
			} catch (Exception ex) {
				Assert.Fail(ex.Message);
			}
		}

		[Test]
		public async Task GetDataSingleObjectTest()
		{
			var data = await DataAccess.GetDataObject(_mock.Object, false);
			Assert.NotNull(data);
		}

		[Test]
		public async Task GetDataListObjectTest()
		{
			var data = await DataAccess.GetDataObject(_mock.Object, true);
			Assert.NotNull(data);
		}

		[Test]
		public void GetComplexKeyTest()
		{
			var id_1 = 1;
			var id_2 = 2;
			var actual = DataAccess.GetKey(_key, id_1, id_2);
			Assert.AreEqual($"{_key}/{id_1}/{id_2}", actual);
		}

		[Test]
		public void GetKeyTest()
		{
			var id = 123;
			var actual = DataAccess.GetKey(_key, id);
			Assert.AreEqual($"{_key}/{id}", actual);
		}

		[Test]
		public void SetErrorMessageNullTest()
		{
			DataAccess.SetError(null, true);
			Assert.AreEqual(false, DataAccess.IsError);
			Assert.AreEqual(false, DataAccess.IsConnectionError);

			DataAccess.SetError(null, false);
			Assert.AreEqual(false, DataAccess.IsConnectionError);
		}

		[Test]
		public void SetErrorTest()
		{
			var message = "Error message";
			DataAccess.SetError(message, true);
			Assert.AreEqual(message, DataAccess.ErrorMessage);
			Assert.AreEqual(true, DataAccess.IsError);
			Assert.AreEqual(true, DataAccess.IsConnectionError);

			DataAccess.SetError(message, false);
			Assert.AreEqual(false, DataAccess.IsConnectionError);
		}
	}
}
