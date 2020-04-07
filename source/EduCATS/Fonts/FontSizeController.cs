using System;
using EduCATS.Helpers.Forms;
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
		const double _addition = 5;

		/// <summary>
		/// Number to add to vw font size.
		/// </summary>
		const double _vwAddition = 1;

		/// <summary>
		/// Are large preferences active.
		/// </summary>
		static bool _isLargePrefs => PlatformServices.Preferences.IsLargeFont;

		public static IPlatformServices PlatformServices;

		static FontSizeController()
		{
			if (PlatformServices == null) {
				PlatformServices = new PlatformServices();
			}
		}

		/// <summary>
		/// Get font size.
		/// </summary>
		/// <param name="namedSize">Size enumeration.</param>
		/// <param name="type">Type.</param>
		/// <returns></returns>
		public static double GetSize(NamedSize namedSize, Type type)
		{
			var namedSizeNumber = (int)namedSize;

			if (!_isLargePrefs) {
				return PlatformServices.Device.GetNamedSize(namedSizeNumber, type);
			}

			return namedSizeNumber switch
			{
				(int)NamedSize.Micro => PlatformServices.Device.GetNamedSize(2, type),
				(int)NamedSize.Small => PlatformServices.Device.GetNamedSize(3, type),
				(int)NamedSize.Default => PlatformServices.Device.GetNamedSize(3, type),
				(int)NamedSize.Medium => PlatformServices.Device.GetNamedSize(4, type),
				(int)NamedSize.Large => PlatformServices.Device.GetNamedSize(4, type) + _addition,
				_ => PlatformServices.Device.GetNamedSize((int)namedSize, type),
			};
		}

		/// <summary>
		/// Get dynamic font size (for HTML vw size).
		/// </summary>
		/// <param name="size">vw size.</param>
		/// <returns>New vw size.</returns>
		public static double GetDynamicSize(double size)
		{
			if (!_isLargePrefs) {
				return size;
			}

			return size + _vwAddition;
		}
	}
}
