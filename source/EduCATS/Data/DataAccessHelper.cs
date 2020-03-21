using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using EduCATS.Data.Caching;
using EduCATS.Data.Interfaces;
using EduCATS.Helpers.Json;
using Xamarin.Essentials;

namespace EduCATS.Data
{
	public partial class DataAccess<T> : IDataAccess where T : new()
	{
		const string _nonJsonSuccessResponse = "\"Ok\"";

		readonly string _key;
		readonly bool _isCaching;
		readonly string _messageForError;
		readonly Func<Task<KeyValuePair<string, HttpStatusCode>>> _callback;

		public bool IsError { get; set; }
		public bool IsConnectionError { get; set; }
		public string ErrorMessage { get; set; }

		public DataAccess(
			string messageForError,
			Func<Task<KeyValuePair<string, HttpStatusCode>>> callback,
			string key = null)
		{
			_key = key;
			_callback = callback;
			_isCaching = !string.IsNullOrEmpty(_key);
			_messageForError = messageForError;
		}

		public async Task<T> GetSingle()
		{
			var singleObject = checkSingleObjectReadyForResponse();

			if (singleObject != null) {
				return singleObject;
			}

			var response = await _callback();
			singleObject = getAccess(response);

			if (singleObject == null) {
				setError(_messageForError);
				return new T();
			}

			return singleObject;
		}

		public async Task<List<T>> GetList()
		{
			var list = checkListReadyForResponse();

			if (list != null) {
				return list;
			}

			var response = await _callback();
			list = getList(response);

			if (list == null) {
				setError(_messageForError);
				return new List<T>();
			}

			return list;
		}

		T checkSingleObjectReadyForResponse()
		{
			if (checkConnectionEstablished()) {
				return default;
			}

			var data = getCacheAndSetConnectionError();
			return JsonController<T>.ConvertJsonToObject(data) ?? new T();
		}

		List<T> checkListReadyForResponse()
		{
			if (checkConnectionEstablished()) {
				return null;
			}

			var data = getCacheAndSetConnectionError();
			var list = JsonController<List<T>>.ConvertJsonToObject(data);
			return list ?? new List<T>();
		}

		string getCacheAndSetConnectionError()
		{
			setError("base_connection_error", true);
			return _key == null ? null : getDataFromCache(_key);
		}

		List<T> getList(KeyValuePair<string, HttpStatusCode> response)
		{
			switch (response.Value) {
				case HttpStatusCode.OK:
					var data = parseResponse(response, _key, _isCaching);

					if (data.Equals(_nonJsonSuccessResponse)) {
						return new List<T>();
					}

					if (!JsonController.IsJsonValid(data)) {
						return default;
					}

					return JsonController<List<T>>.ConvertJsonToObject(data);
				default:
					return default;
			}
		}

		T getAccess(KeyValuePair<string, HttpStatusCode> response)
		{
			switch (response.Value) {
				case HttpStatusCode.OK:
					var data = parseResponse(response, _key, _isCaching);

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

		void setError(string message, bool isConnectionError = false)
		{
			IsError = true;
			ErrorMessage = message;
			IsConnectionError = isConnectionError;
		}

		string parseResponse(object responseObject, string key = null, bool isCaching = true)
		{
			if (responseObject == null) {
				return null;
			}

			var response = (KeyValuePair<string, HttpStatusCode>)responseObject;

			if (response.Value != HttpStatusCode.OK && response.Key == null) {
				return null;
			}

			if (isCaching && !string.IsNullOrEmpty(key)) {
				DataCaching<string>.Save(key, response.Key);
			}

			return response.Key;
		}

		static string getDataFromCache(string key)
		{
			return DataCaching<string>.Get(key);
		}

		static bool checkConnectionEstablished()
		{
			return Connectivity.NetworkAccess == NetworkAccess.Internet;
		}
	}
}
