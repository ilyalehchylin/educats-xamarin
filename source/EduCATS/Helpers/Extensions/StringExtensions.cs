using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace EduCATS.Helpers.Extensions
{
	/// <summary>
	/// String extensions helper.
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		/// Convert string to double.
		/// </summary>
		/// <param name="stringToConvert">String to convert.</param>
		/// <returns>Double value.</returns>
		public static double StringToDouble(this string stringToConvert)
		{
			return string.IsNullOrEmpty(stringToConvert) ?
				0 : double.Parse(stringToConvert, CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Convert first char of string to uppercase.
		/// </summary>
		/// <param name="input">Input string.</param>
		/// <returns>Converted string.</returns>
		public static string FirstCharToUpper(this string input)
        {
			return string.IsNullOrEmpty(input) ?
				input : input.First().ToString().ToUpper() + input.Substring(1);
		}

		/// <summary>
		/// Remove HTML tags from string.
		/// </summary>
		/// <param name="input">Input string.</param>
		/// <returns>String without HTML tags.</returns>
		public static string RemoveHTMLTags(this string input)
		{
			return Regex.Replace(input, "<.*?>|&#34;", string.Empty);
		}
	}
}
