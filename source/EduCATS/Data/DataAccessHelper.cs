using System.Collections.Generic;
using System.Net;
using EduCATS.Data.Caching;
using EduCATS.Helpers.Json;

namespace EduCATS.Data
{
	public partial class DataAccess<T> where T : new()
	{
		const string _nonJsonSuccessResponse = "\"Ok\"";

		public T GetAccess(KeyValuePair<string, HttpStatusCode> response, string key = null, bool isCaching = true)
		{
			switch (response.Value) {
				case HttpStatusCode.OK:
					var data = parseResponse(response, key, isCaching);

					if (data.Equals(_nonJsonSuccessResponse)) {
						return new T();
					}

					if (!JsonController.IsJsonValid(data)) {
						return default;
					}

					return JsonController<T>.ConvertJsonToObject(data);
				default:
					return default;
			}
		}

		string parseResponse(object responseObject, string key = null, bool isCaching = true)
		{
			if (responseObject != null) {
				var response = (KeyValuePair<string, HttpStatusCode>)responseObject;

				if (response.Value == HttpStatusCode.OK && response.Key != null) {
					if (isCaching && !string.IsNullOrEmpty(key)) {
						DataCaching<string>.Save(key, response.Key);
					}

					return response.Key;
				}
			}

			return null;
		}
	}
}