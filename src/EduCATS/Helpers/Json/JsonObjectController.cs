using Newtonsoft.Json;

namespace EduCATS.Helpers.Json
{
	public static class JsonController<T>
	{
		public static T ConvertJsonToObject(string content)
		{
			if (string.IsNullOrEmpty(content)) {
				return default;
			}

			var items = JsonConvert.DeserializeObject<T>(content);
			return items;
		}
	}
}