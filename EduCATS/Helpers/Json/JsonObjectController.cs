using Newtonsoft.Json;

namespace EduCATS.Helpers.Json
{
	/// <summary>
	/// Json helper with template.
	/// </summary>
	/// <typeparam name="T">Type to handle.</typeparam>
	public static class JsonController<T>
	{
		/// <summary>
		/// Convert Json string to <see cref="T"/>.
		/// </summary>
		/// <param name="content">Json string.</param>
		/// <returns>Object.</returns>
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
