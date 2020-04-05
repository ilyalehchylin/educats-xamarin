using System;
using System.Globalization;
using Xamarin.Forms;

namespace EduCATS.Helpers.Forms.Converters
{
	/// <summary>
	/// String to <see cref="Color"/> converter.
	/// </summary>
	public class StringToColorConverter : IValueConverter
	{
		/// <summary>
		/// Transparent color name.
		/// </summary>
		const string _transparentColorString = "Transparent";

		/// <summary>
		/// Default <see cref="Color"/>.
		/// </summary>
		static readonly Color _defaultColor = Color.Default;

		/// <summary>
		/// Transparent <see cref="Color"/>.
		/// </summary>
		static readonly Color _transparentColor = Color.Transparent;

		/// <summary>
		/// Convert.
		/// </summary>
		/// <param name="value">String value.</param>
		/// <param name="targetType">Target type.</param>
		/// <param name="parameter">Parameter.</param>
		/// <param name="culture">Culture info.</param>
		/// <returns>Converter <see cref="Color"/>.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
			if (value == null || string.IsNullOrEmpty(value.ToString())) {
				return _defaultColor;
			}

            var valueAsString = value.ToString();

			return valueAsString switch
			{
				_transparentColorString => _transparentColor,
				_ => Color.FromHex(value.ToString()),
			};
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
