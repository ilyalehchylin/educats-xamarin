using System;
using System.Globalization;
using System.IO;
using Xamarin.Forms;

namespace EduCATS.Helpers.Forms.Converters
{
	/// <summary>
	/// Base64 image to <see cref="ImageSource"/> converter.
	/// </summary>
	public class Base64ToImageSourceConverter : IValueConverter
	{
		/// <summary>
		/// Base64 prefix to remove before conversion.
		/// </summary>
        const string _base64Prefix = "data:image/png;base64,";

		/// <summary>
		/// Convert.
		/// </summary>
		/// <param name="value">Base64 image string.</param>
		/// <param name="targetType">Target type.</param>
		/// <param name="parameter">Parameter.</param>
		/// <param name="culture">Culture info.</param>
		/// <returns>Converted <see cref="ImageSource"/>.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
			if (value == null) {
                return null;
			}

            var base64Image = value.ToString();

            if (base64Image == null && base64Image.Length <= _base64Prefix.Length) {
                return null;
            }

            base64Image = base64Image.Replace(_base64Prefix, "");
			var imageBytes = System.Convert.FromBase64String(base64Image);
            return ImageSource.FromStream(() => { return new MemoryStream(imageBytes); });
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
