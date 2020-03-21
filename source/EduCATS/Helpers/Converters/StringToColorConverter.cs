using System;
using System.Globalization;
using Xamarin.Forms;

namespace EduCATS.Helpers.Converters
{
	public class StringToColorConverter : IValueConverter
	{
		const string _transparentColorString = "Transparent";

		static readonly Color _defaultColor = Color.Default;
		static readonly Color _transparentColor = Color.Transparent;

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

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
