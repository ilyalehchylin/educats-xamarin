using EduCATS.Helpers.Themes.Interfaces;

namespace EduCATS.Helpers.Themes.Templates
{
	public class DefaultTheme : ITheme
	{
		virtual public string AppStatusBarBackgroundColor { get { return "#27AAE1"; } }
	}
}