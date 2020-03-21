using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EduCATS.Helpers.Json
{
	public static class JsonController
	{
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

		public static string ConvertObjectToJson(object contentObject)
		{
			return contentObject != null ? JsonConvert.SerializeObject(contentObject) : null;
		}
	}
}
