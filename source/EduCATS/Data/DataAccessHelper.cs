using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using EduCATS.Data.Caching;
using EduCATS.Data.Interfaces;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Json;

namespace EduCATS.Data
{
	/// <summary>
	/// Data Access helper.
	/// </summary>
	/// <typeparam name="T">Type for data.</typeparam>
	public partial class DataAccess<T> : IDataAccess<T> where T : new()
	{
		/// <summary>
		/// Success string response.
		/// </summary>
		const string _nonJsonSuccessResponse = "\"Ok\"";

		/// <summary>
		/// Caching key.
		/// </summary>
		readonly string _key;

		/// <summary>
		/// Is caching enabled.
		/// </summary>
		readonly bool _isCaching;

		/// <summary>
		/// Error message.
		/// </summary>
		readonly string _messageForError;

		/// <summary>
		/// Callback to invoke.
		/// </summary>
		static Func<Task<KeyValuePair<string, HttpStatusCode>>> _callback;

		/// <summary>
		/// Is error occurred.
		/// </summary>
		public bool IsError { get; set; }

		/// <summary>
		/// Is network connection issue.
		/// </summary>
		public bool IsConnectionError { get; set; }

		/// <summary>
		/// Error message localized key.
		/// </summary>
		public string ErrorMessageKey { get; set; }

		/// <summary>
		/// Platform services.
		/// </summary>
		readonly IPlatformServices _services;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="messageForError">Error message.</param>
		/// <param name="callback">Callback to invoke.</param>
		/// <param name="key">Caching key.</param>
		public DataAccess(
			string messageForError,
			Task<object> callback,
			string key = null)
		{
			_key = key;
			_messageForError = messageForError;
			_services = new PlatformServices();
			_isCaching = !string.IsNullOrEmpty(_key);
			setCallback(callback);
		}

		/// <summary>
		/// Get single object.
		/// </summary>
		/// <returns>Single object.</returns>
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

		/// <summary>
		/// Get objects list.
		/// </summary>
		/// <returns>Objects list.</returns>
		public async Task<List<T>> GetList()
		{
			var list = checkListReadyForResponse();

			if (list != null) {
				return list;
			}

			var response = await _callback();
			list = getListAccess(response);

			if (list == null) {
				setError(_messageForError);
				return new List<T>();
			}

			return list;
		}

		/// <summary>
		/// Get data from cache
		/// and set connection error.
		/// </summary>
		/// <returns>Data object.</returns>
		T checkSingleObjectReadyForResponse()
		{
			if (checkConnectionEstablished()) {
				return default;
			}

			var data = getCacheAndSetConnectionError();
			return JsonController<T>.ConvertJsonToObject(data) ?? new T();
		}

		/// <summary>
		/// Get data list from cache
		/// and set connection error.
		/// </summary>
		/// <returns>List of data.</returns>
		List<T> checkListReadyForResponse()
		{
			if (checkConnectionEstablished()) {
				return null;
			}

			var data = getCacheAndSetConnectionError();
			var list = JsonController<List<T>>.ConvertJsonToObject(data);
			return list ?? new List<T>();
		}

		/// <summary>
		/// Get cache and set connection error.
		/// </summary>
		/// <returns>Data in string format (<c>json</c>).</returns>
		string getCacheAndSetConnectionError()
		{
			setError("base_connection_error", true);
			return _key == null ? null : getDataFromCache(_key);
		}

		/// <summary>
		/// Parse response and get list of objects.
		/// </summary>
		/// <param name="response">Response.</param>
		/// <returns>List of objects.</returns>
		List<T> getListAccess(KeyValuePair<string, HttpStatusCode> response)
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

		/// <summary>
		/// Parse response and get object.
		/// </summary>
		/// <param name="response">Response.</param>
		/// <returns>Object.</returns>
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

		/// <summary>
		/// Set error details.
		/// </summary>
		/// <param name="message">Error message.</param>
		/// <param name="isConnectionError">Is connection error.</param>
		void setError(string message, bool isConnectionError = false)
		{
			IsError = true;
			ErrorMessageKey = message;
			IsConnectionError = isConnectionError;
		}

		/// <summary>
		/// Parse response.
		/// </summary>
		/// <param name="responseObject">Response object.</param>
		/// <param name="key">Caching key.</param>
		/// <param name="isCaching">Is caching enabled.</param>
		/// <returns>Json string.</returns>
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

		/// <summary>
		/// Get data from cache.
		/// </summary>
		/// <param name="key">Caching key.</param>
		/// <returns>Json string.</returns>
		static string getDataFromCache(string key)
		{
			return DataCaching<string>.Get(key);
		}

		/// <summary>
		/// Set callback variable.
		/// </summary>
		/// <param name="callback">Callback object.</param>
		static void setCallback(Task<object> callback)
		{
			if (callback == null) {
				_callback = async () => {
					await Task.Run(() => { });
					return new KeyValuePair<string, HttpStatusCode>();
				};
				return;
			}

			_callback = async () => {
				var result = await callback;
				return (KeyValuePair<string, HttpStatusCode>)result;
			};
		}

		/// <summary>
		/// Check network connection.
		/// </summary>
		/// <returns><c>True</c> if established.</returns>
		public virtual bool checkConnectionEstablished()
		{
			return _services.Device.CheckConnectivity();
		}
	}
}
