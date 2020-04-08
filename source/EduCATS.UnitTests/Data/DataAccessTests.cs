using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using EduCATS.Constants;
using EduCATS.Data;
using EduCATS.Helpers.Forms;
using MonkeyCache.FileStore;
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
		const string _nonJsonSuccessResponse = "\"Ok\"";

		IPlatformServices _mockedOffline;
		IPlatformServices _mockedConnected;

		[SetUp]
		public void SetUp()
		{
			_mockedOffline = Mock.Of<IPlatformServices>(ps => ps.Device.CheckConnectivity() == false);
			_mockedConnected = Mock.Of<IPlatformServices>(ps => ps.Device.CheckConnectivity() == true);

			var assembly = typeof(App).GetTypeInfo().Assembly;
			CrossLocalization.Initialize(
				assembly,
				GlobalConsts.RunNamespace,
				GlobalConsts.LocalizationDirectory);

			CrossLocalization.AddLanguageSupport(Languages.EN);
			CrossLocalization.SetDefaultLanguage(Languages.EN.LangCode);
			CrossLocalization.SetLanguage(Languages.EN.LangCode);

			Barrel.ApplicationId = GlobalConsts.AppId;
		}

		[Test]
		public async Task GetSingleTest()
		{
			var mockedConnected = Mock.Of<IPlatformServices>(ps => ps.Device.CheckConnectivity() == true);
			var dataAccess = new DataAccess<object>(_message, null, "1", mockedConnected);
			var actual = await dataAccess.GetSingle();
			Assert.IsNotNull(actual);
		}

		[Test]
		public async Task GetListTest()
		{
			var dataAccess = new DataAccess<object>(_message, null, "2", _mockedConnected);
			var actual = await dataAccess.GetList();
			Assert.IsNotNull(actual);
		}

		[Test]
		public void CheckConnectionTest()
		{
			var dataAccess = new DataAccess<object>(_message, null, "3", _mockedConnected);
			var actual = dataAccess.CheckConnectionEstablished();
			Assert.AreEqual(true, actual);
		}

		[Test]
		public async Task GetSingleNoConnectionTest()
		{
			var dataAccess = new DataAccess<object>(_message, null, "4", _mockedOffline);
			var actual = await dataAccess.GetSingle();
			Assert.IsNotNull(actual);
		}

		[Test]
		public async Task GetListNoConnectionTest()
		{
			var dataAccess = new DataAccess<object>(_message, null, "5", _mockedOffline);
			var actual = await dataAccess.GetList();
			Assert.IsNotNull(actual);
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
			var dataAccess = new DataAccess<object>(_message, null, "6", _mockedConnected);
			var data = await DataAccess.GetDataObject(dataAccess, false);
			Assert.NotNull(data);
		}

		[Test]
		public async Task GetDataListObjectTest()
		{
			var dataAccess = new DataAccess<object>(_message, null, "7", _mockedConnected);
			var data = await DataAccess.GetDataObject(dataAccess, true);
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

		[Test]
		public void GetListAccessValidJsonTest()
		{
			var dataAccess = new DataAccess<object>(_message, null, "8", _mockedConnected);
			var kvp = new KeyValuePair<string, HttpStatusCode>("[ { \"data\": \"test\" } ]", HttpStatusCode.OK);
			var actual = dataAccess.GetListAccess(kvp);
			Assert.NotNull(actual);
		}

		[Test]
		public void GetListAccessNonValidJsonTest()
		{
			var dataAccess = new DataAccess<object>(_message, null, "9", _mockedConnected);
			var kvp = new KeyValuePair<string, HttpStatusCode>("response", HttpStatusCode.OK);
			var actual = dataAccess.GetListAccess(kvp);
			Assert.AreEqual(null, actual);
		}

		[Test]
		public void GetListAccessSuccessJsonTest()
		{
			var dataAccess = new DataAccess<object>(_message, null, "10", _mockedConnected);
			var kvp = new KeyValuePair<string, HttpStatusCode>(_nonJsonSuccessResponse, HttpStatusCode.OK);
			var actual = dataAccess.GetListAccess(kvp);
			Assert.AreEqual(string.Empty, actual);
		}

		[Test]
		public void GetSingleAccessValidJsonTest()
		{
			var dataAccess = new DataAccess<object>(_message, null, "11", _mockedConnected);
			var kvp = new KeyValuePair<string, HttpStatusCode>("{ \"data\": \"test\" }", HttpStatusCode.OK);
			var actual = dataAccess.GetAccess(kvp);
			Assert.NotNull(actual);
		}

		[Test]
		public void GetSingleAccessNonValidJsonTest()
		{
			var dataAccess = new DataAccess<object>(_message, null, "12", _mockedConnected);
			var kvp = new KeyValuePair<string, HttpStatusCode>("response", HttpStatusCode.OK);
			var actual = dataAccess.GetAccess(kvp);
			Assert.AreEqual(null, actual);
		}

		[Test]
		public void GetSingleAccessSuccessJsonTest()
		{
			var dataAccess = new DataAccess<object>(_message, null, "13", _mockedConnected);
			var kvp = new KeyValuePair<string, HttpStatusCode>(_nonJsonSuccessResponse, HttpStatusCode.OK);
			var actual = dataAccess.GetAccess(kvp);
			Assert.NotNull(actual);
		}
	}
}
