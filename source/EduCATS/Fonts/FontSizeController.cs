using System;
using EduCATS.Helpers.Settings;
using Xamarin.Forms;

namespace EduCATS.Fonts
{
	/// <summary>
	/// Font size controller.
	/// </summary>
	public static class FontSizeController
	{
		/// <summary>
		/// Number to add to the largest size.
		/// </summary>
		const double _largestAddition = 5;

		/// <summary>
		/// Are large preferences active.
		/// </summary>
		static bool _isLargePrefs => AppPrefs.IsLargeFont;

		/// <summary>
		/// Get font size.
		/// </summary>
		/// <param name="namedSize">Size enumeration.</param>
		/// <param name="type">Type.</param>
		/// <returns></returns>
		public static double GetSize(NamedSize namedSize, Type type)
		{
			if (!_isLargePrefs) {
				return Device.GetNamedSize(namedSize, type);
			}

			return namedSize switch
			{
				NamedSize.Micro => Device.GetNamedSize(NamedSize.Small, type),
				NamedSize.Small => Device.GetNamedSize(NamedSize.Medium, type),
				NamedSize.Default => Device.GetNamedSize(NamedSize.Medium, type),
				NamedSize.Medium => Device.GetNamedSize(NamedSize.Large, type),
				NamedSize.Large => Device.GetNamedSize(NamedSize.Large, type) + _largestAddition,
				_ => Device.GetNamedSize(namedSize, type),
			};
		}
	}
}
