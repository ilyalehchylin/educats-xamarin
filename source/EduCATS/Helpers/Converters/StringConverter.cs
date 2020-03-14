using System.Globalization;

namespace EduCATS.Helpers.Converters
{
	public static class StringConverter
	{
		public static double StringToDouble(string stringToConvert)
		{
			if (string.IsNullOrEmpty(stringToConvert)) {
				return 0;
			}

			return double.Parse(stringToConvert, CultureInfo.InvariantCulture);
		}
	}
}
