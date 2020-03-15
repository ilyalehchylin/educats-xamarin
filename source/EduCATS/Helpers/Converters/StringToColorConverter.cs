using System;
using System.Globalization;
using Xamarin.Forms;

namespace EduCATS.Helpers.Converters
{
	public class StringToColorConverter : IValueConverter
	{
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
			if (value == null || string.IsNullOrEmpty(value.ToString())) {
				return Color.Default;
			}

            var valueAsString = value.ToString();

			switch (valueAsString) {
				case "Transparent":
					return Color.Transparent;
				default:
					return Color.FromHex(value.ToString());
			}
		}

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}