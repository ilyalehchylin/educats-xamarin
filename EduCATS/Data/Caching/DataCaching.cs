using System;
using EduCATS.Constants;
using MonkeyCache.FileStore;

namespace EduCATS.Data.Caching
{
	/// <summary>
	/// Data caching with <c>MonkeyCache</c>.
	/// </summary>
	/// <typeparam name="T">Type to cache.</typeparam>
	public static class DataCaching<T>
	{
		/// <summary>
		/// Delete all cache.
		/// </summary>
		public static void RemoveCache()
		{
			Barrel.Current.EmptyAll();
		}

		/// <summary>
		/// Save data cache for specified key.
		/// </summary>
		/// <param name="key">Key for data.</param>
		/// <param name="data">Data to cache.</param>
		public static void Save(string key, T data)
		{
			Barrel.Current.Add(key, data, TimeSpan.FromDays(GlobalConsts.CacheExpirationInDays));
		}

		/// <summary>
		/// Get data cahce for key.
		/// </summary>
		/// <param name="key">Key for data.</param>
		/// <returns>Cached data if key exists.</returns>
		public static T Get(string key)
		{
			removeExpired();
			return Barrel.Current.Get<T>(key);
		}

		/// <summary>
		/// Delete expired cache.
		/// </summary>
		/// <remarks>
		/// Expiration time is specified in
		/// <see cref="GlobalConsts.CacheExpirationInDays"/> constant.
		/// </remarks>
		static void removeExpired()
		{
			Barrel.Current.EmptyExpired();
		}
	}
}
