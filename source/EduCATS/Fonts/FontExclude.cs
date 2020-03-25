using System.Collections.Generic;
using System.Linq;

namespace EduCATS.Fonts
{
	/// <summary>
	/// Class for checking excluded font names
	/// from bold attribute setting.
	/// </summary>
	public static class FontExclude
	{
		/// <summary>
		/// Fonts which cannot be <c>Bold</c> (<c>Regular</c> should be used instead).
		/// </summary>
		/// <returns>List with font names.</returns>
		static readonly List<string> _excludeList = new List<string> {
			"Pacifico-Regular"
		};

		/// <summary>
		/// Check if font is excluded.
		/// </summary>
		/// <param name="font">Font name to check.</param>
		/// <returns>Is excluded.</returns>
		public static bool CheckNameExcluded(string font) => _excludeList.Any(f => f.Equals(font));
	}
}
