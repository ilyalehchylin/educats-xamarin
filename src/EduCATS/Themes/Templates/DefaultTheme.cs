using EduCATS.Themes.Interfaces;

namespace EduCATS.Themes.Templates
{
	public class DefaultTheme : ITheme
	{
		virtual public string AppStatusBarBackgroundColor => "#27AAE1";

		virtual public string LoginBackground1Image => "image_background_1";
		virtual public string LoginBackground2Image => "image_background_2";
		virtual public string LoginBackground3Image => "image_background_3";
		virtual public string LoginMascotImage => "image_mascot";
		virtual public string LoginShowPasswordImage => "icon_show_password";
		virtual public string LoginEntryBackgroundColor => "#FFFFFF";
		virtual public string LoginButtonBackgroundColor => "#27AAE1";
		virtual public string LoginButtonTextColor => "#FFFFFF";
	}
}