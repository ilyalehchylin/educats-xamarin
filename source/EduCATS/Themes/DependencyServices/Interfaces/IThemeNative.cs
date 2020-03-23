namespace EduCATS.Themes.DependencyServices.Interfaces
{
	/// <summary>
	/// Platform-specific themes interface.
	/// </summary>
	public interface IThemeNative
	{
		/// <summary>
		/// Set status & navigation bar colors.
		/// </summary>
		/// <param name="colorHex">Hex color.</param>
		void SetColors(string colorHex);
	}
}
