using System;
using System.Globalization;
using Xamarin.Forms;

namespace EduCATS.Helpers.Converters
{
	public class StringToImageSourceConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var imageString = value?.ToString();
			return imageString == null ?
				null : ImageSource.FromFile(imageString);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}
