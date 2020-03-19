using System;
using System.Globalization;
using System.IO;
using Xamarin.Forms;

namespace EduCATS.Helpers.Converters
{
	public class Base64ToImageSourceConverter : IValueConverter
	{
        const string _base64Prefix = "data:image/png;base64,";

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

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
