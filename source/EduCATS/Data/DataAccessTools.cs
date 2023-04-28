using System.Threading.Tasks;
using EduCATS.Data.Caching;
using EduCATS.Data.Interfaces;
using Nyxbull.Plugins.CrossLocalization;

namespace EduCATS.Data
{
	/// <summary>
	/// Helper partial class for <see cref="DataAccess"/>.
	/// </summary>
	public static partial class DataAccess
	{
		/// <summary>
		/// Is error occurred.
		/// </summary>
		public static bool IsError { get; set; }

		/// <summary>
		/// Is network connection issue.
		/// </summary>
		public static bool IsConnectionError { get; set; }

		/// <summary>
		/// Error message.
		/// </summary>
		public static string ErrorMessage { get; set; }

		/// <summary>
		/// Delete data cache.
		/// </summary>
		public static void ResetData()
		{
			DataCaching<object>.RemoveCache();
		}

		/// <summary>
		/// Get data object and set error details.
		/// </summary>
		/// <typeparam name="T">Object type.</typeparam>
		/// <param name="dataAccess">Data Access instance.</param>
		/// <param name="isList">Is object a list or a single object.</param>
		/// <returns>Object.</returns>
		public async static Task<object> GetDataObject<T>(IDataAccess<T> dataAccess, bool isList)
		{
			object objectToGet;

			if (isList) {
				objectToGet = await dataAccess.GetList();
			} else {
				objectToGet = await dataAccess.GetSingle();
			}

			SetError(dataAccess.ErrorMessageKey, dataAccess.IsConnectionError);
			return objectToGet;
		}

		/// <summary>
		/// Get complex key with identifiers.
		/// </summary>
		/// <param name="key">Basic key.</param>
		/// <param name="firstId">First ID.</param>
		/// <param name="secondId">Second ID.</param>
		/// <returns></returns>
		public static string GetKey(string key, object firstId, object secondId)
		{
			return $"{GetKey(key, firstId)}/{secondId}";
		}

		/// <summary>
		/// Get complex key with identifier.
		/// </summary>
		/// <param name="key">Basic key.</param>
		/// <param name="id"><ID./param>
		/// <returns></returns>
		public static string GetKey(string key, object id)
		{
			return $"{key}/{id}";
		}

		/// <summary>
		/// Set error details.
		/// </summary>
		/// <param name="message">Error message.</param>
		/// <param name="isConnectionError">Is network connection issue.</param>
		/// <remarks>
		/// Can be <c>null</c> (if no error occurred).
		/// </remarks>
		public static void SetError(string message, bool isConnectionError)
		{
			if (message == null) {
				IsError = false;
				IsConnectionError = false;
				return;
			}

			IsError = true;
			IsConnectionError = isConnectionError;
			ErrorMessage = CrossLocalization.Translate(message);
		}
	}
}
