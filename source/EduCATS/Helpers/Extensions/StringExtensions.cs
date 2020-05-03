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
			return string.IsNullOrEmpty(stringToConvert) ? 0 : parseDouble(stringToConvert);
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
			if (string.IsNullOrEmpty(input)) {
				return input;
			}

			return Regex.Replace(input, "<.*?>|&#34;", string.Empty);
		}

		/// <summary>
		/// Remove links from string.
		/// </summary>
		/// <param name="input">Input string.</param>
		/// <returns>String without links.</returns>
		public static string RemoveLinks(this string input)
		{
			if (string.IsNullOrEmpty(input)) {
				return input;
			}

			return Regex.Replace(input, @"http[^\s]+", "");
		}

		/// <summary>
		/// Try to parse <c>double</c> string.
		/// </summary>
		/// <param name="doubleString"><c>double</c> string value.</param>
		/// <returns><c>double</c> value.</returns>
		static double parseDouble(string doubleString)
		{
			double.TryParse(doubleString, out double value);
			return value;
		}
	}
}
