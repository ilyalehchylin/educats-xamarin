using System.Collections.Generic;
using System.Linq;
using EduCATS.Constants;
using EduCATS.Helpers.Forms;
using Nyxbull.Plugins.CrossLocalization;

namespace EduCATS.Fonts
{
	/// <summary>
	/// App font family controller.
	/// </summary>
	public static class FontsController
	{
		/// <summary>
		/// Font list.
		/// </summary>
		static List<string> _fonts;

		/// <summary>
		/// Runtime platform (Android / iOS).
		/// </summary>
		static string _runtimePlatform;

		/// <summary>
		/// Platform services.
		/// </summary>
		static IPlatformServices _services;

		/// <summary>
		/// Default font key.
		/// </summary>
		public const string DefaultFont = "font_default";

		/// <summary>
		/// Regular font alias.
		/// </summary>
		const string _regularAlias = "-Regular";

		/// <summary>
		/// Bold font alias.
		/// </summary>
		const string _boldAlias = "-Bold";

		/// <summary>
		/// Initialize fonts.
		/// </summary>
		/// <remarks>Call this on app start.</remarks>
		/// <param name="platform">Runtime platform (e.g. Android).</param>
		public static void Initialize(IPlatformServices platformServices, string platform)
		{
			_services = platformServices;
			_runtimePlatform = platform;
			initFonts();
		}

		/// <summary>
		/// Get font list.
		/// </summary>
		/// <returns>Font list.</returns>
		public static List<string> GetFonts() => _fonts;

		/// <summary>
		/// Set current font.
		/// </summary>
		public static void SetCurrentFont() => SetFont(_services.Preferences.Font);

		/// <summary>
		/// Set font.
		/// </summary>
		/// <param name="fontName">Font family.</param>
		public static void SetFont(string fontName)
		{
			if (fontName.Equals(CrossLocalization.Translate(DefaultFont))) {
				_services.Preferences.Font = DefaultFont;
				return;
			}

			var font = _fonts.SingleOrDefault(f => f.Equals(fontName));

			if (font != null) {
				_services.Preferences.Font = font;
			}
		}

		/// <summary>
		/// Get current font.
		/// </summary>
		/// <returns>Font family.</returns>
		public static string GetCurrentFont(bool bold = false) => GetFont(_services.Preferences.Font, bold);

		/// <summary>
		/// Get font by family.
		/// </summary>
		/// <param name="font">Font family.</param>
		/// <returns>Font family.</returns>
		public static string GetFont(string font, bool bold)
		{
			if (font.Equals(DefaultFont) || font.Equals(CrossLocalization.Translate(DefaultFont))) {
				return null;
			}

			if (bold && !FontExclude.CheckNameExcluded(font)) {
				font = getBoldFont(font);
			}

			return _runtimePlatform == GlobalConsts.AndroidPlatform ?
				$"{font}.ttf#{font}" : font;
		}

		/// <summary>
		/// Get font name without alias.
		/// </summary>
		/// <param name="fontFamily">Font family.</param>
		/// <returns>Font family.</returns>
		public static string GetFontName(string fontFamily)
		{
			if (string.IsNullOrEmpty(fontFamily)) {
				return fontFamily;
			}

			if (fontFamily.Contains(_regularAlias)) {
				return fontFamily.Replace(_regularAlias, "");
			}

			return fontFamily;
		}

		/// <summary>
		/// Get bold font from regular.
		/// </summary>
		/// <param name="font">Regular font name.</param>
		/// <returns>Bold font.</returns>
		static string getBoldFont(string font)
		{
			if (string.IsNullOrEmpty(font)) {
				return font;
			}

			return font.Replace(_regularAlias, _boldAlias);
		}

		/// <summary>
		/// Initialize fonts.
		/// </summary>
		static void initFonts()
		{
			_fonts = new List<string> {
				"Roboto-Regular",
				"Oswald-Regular",
				"PTSans-Regular",
				"PTSerif-Regular",
				"Pacifico-Regular",
				"OpenSans-Regular",
				"Montserrat-Regular",
				"RobotoCondensed-Regular",
				"CormorantGaramond-Regular"
			};

			_fonts.Sort((x, y) => string.Compare(x, y));
			_fonts.Insert(0, CrossLocalization.Translate(DefaultFont));
		}
	}
}
