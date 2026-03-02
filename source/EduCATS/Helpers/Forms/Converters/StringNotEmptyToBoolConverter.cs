using System;
using System.Globalization;
using Xamarin.Forms;

namespace EduCATS.Helpers.Forms.Converters
{
	/// <summary>
	/// String to <see cref="bool"/> converter.
	/// </summary>
	public class StringNotEmptyToBoolConverter : IValueConverter
	{
		/// <summary>
		/// Convert.
		/// </summary>
		/// <param name="value">String value.</param>
		/// <param name="targetType">Target type.</param>
		/// <param name="parameter">Parameter.</param>
		/// <param name="culture">Culture info.</param>
		/// <returns>Boolean indicating whether value is not empty.</returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return !string.IsNullOrWhiteSpace(value?.ToString());
		}

		/// <summary>
		/// Convert back.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="targetType">Target type.</param>
		/// <param name="parameter">Parameter.</param>
		/// <param name="culture">Culture info.</param>
		/// <returns>Object.</returns>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}
