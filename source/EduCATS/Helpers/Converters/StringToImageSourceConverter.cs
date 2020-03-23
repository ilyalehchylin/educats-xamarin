using System;
using System.Globalization;
using Xamarin.Forms;

namespace EduCATS.Helpers.Converters
{
	/// <summary>
	/// String to <see cref="ImageSource"/> converter.
	/// </summary>
	public class StringToImageSourceConverter : IValueConverter
	{
		/// <summary>
		/// Convert.
		/// </summary>
		/// <param name="value">String value.</param>
		/// <param name="targetType">Target type.</param>
		/// <param name="parameter">Parameter.</param>
		/// <param name="culture">Culture info.</param>
		/// <returns>Converter <see cref="ImageSource"/>.</returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var imageString = value?.ToString();
			return imageString == null ?
				null : ImageSource.FromFile(imageString);
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
