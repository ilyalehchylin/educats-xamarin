using System;
using EduCATS.Constants;
using EduCATS.Data.Caching;
using MonkeyCache.FileStore;
using NUnit.Framework;

namespace EduCATS.UnitTests
{
	[TestFixture]
	public class DataCachingTests
	{
		const string _cacheKey = "TestKey";
		const string _dataToSave = "TestData";
		const double _expireDays = 1;

		[SetUp]
		public void SetUp()
		{
			Barrel.ApplicationId = GlobalConsts.AppId;
		}

		[Test]
		public void RemoveCacheTest()
		{
			Barrel.Current.Add(_cacheKey, _dataToSave, TimeSpan.FromDays(_expireDays));
			DataCaching<object>.RemoveCache();
			var actual = Barrel.Current.Get<string>(_cacheKey);
			Assert.AreEqual(null, actual);
		}

		[Test]
		public void SaveTest()
		{
			DataCaching<string>.Save(_cacheKey, _dataToSave);
			var actual = Barrel.Current.Get<string>(_cacheKey);
			Assert.AreEqual(_dataToSave, actual);
		}

		[Test]
		public void GetTest()
		{
			Barrel.Current.Add(_cacheKey, _dataToSave, TimeSpan.FromDays(_expireDays));
			var actual = DataCaching<string>.Get(_cacheKey);
			Assert.AreEqual(_dataToSave, actual);
		}
	}
}
