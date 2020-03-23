using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EduCATS.Helpers.Json
{
	/// <summary>
	/// Json helper.
	/// </summary>
	public static class JsonController
	{
		/// <summary>
		/// Is JSON valid.
		/// </summary>
		/// <param name="json">Json to check.</param>
		/// <returns>Result.</returns>
		public static bool IsJsonValid(string json)
		{
			if (string.IsNullOrEmpty(json)) {
				return false;
			}

			json = json.Trim();
			if ((json.StartsWith("{", StringComparison.Ordinal) &&
				json.EndsWith("}", StringComparison.Ordinal)) ||
				(json.StartsWith("[", StringComparison.Ordinal) &&
				json.EndsWith("]", StringComparison.Ordinal))) {
				try {
					var obj = JToken.Parse(json);
					return true;
				} catch {
					return false;
				}
			}

			return false;
		}

		/// <summary>
		/// Convert <c>object</c> to Json string.
		/// </summary>
		/// <param name="contentObject">Object to convert.</param>
		/// <returns></returns>
		public static string ConvertObjectToJson(object contentObject)
		{
			return contentObject != null ? JsonConvert.SerializeObject(contentObject) : null;
		}
	}
}
