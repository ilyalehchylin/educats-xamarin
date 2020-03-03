using System.Collections.Generic;
using System.Net;
using EduCATS.Data.Caching;
using Xamarin.Essentials;

namespace EduCATS.Data
{
	public static partial class DataAccess
	{
		static object parseResponse(object responseObject, bool isCaching = true)
		{
			if (responseObject != null) {
				var response = (KeyValuePair<object, HttpStatusCode>)responseObject;

				if (response.Value == HttpStatusCode.OK && response.Key != null) {
					if (isCaching) {
						DataCaching<object>.Save(profileInfoKey, response.Key);
					}

					return response.Key;
				}
			}

			return null;
		}

		static object getDataFromCache(string key)
		{
			return DataCaching<object>.Get(key);
		}

		static bool checkConnection()
		{
			return Connectivity.NetworkAccess == NetworkAccess.Internet;
		}
	}
}