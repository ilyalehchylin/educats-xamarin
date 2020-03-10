using EduCATS.Themes.Interfaces;

namespace EduCATS.Themes.Templates
{
	public class DefaultTheme : ITheme
	{
		virtual public string CommonAppColor => "#27AAE1";

		virtual public string AppStatusBarBackgroundColor => CommonAppColor;

		virtual public string LoginBackground1Image => "image_background_1";
		virtual public string LoginBackground2Image => "image_background_2";
		virtual public string LoginBackground3Image => "image_background_3";
		virtual public string LoginMascotImage => "image_mascot";
		virtual public string LoginShowPasswordImage => "icon_show_password";
		virtual public string LoginEntryBackgroundColor => "#FFFFFF";
		virtual public string LoginButtonBackgroundColor => "#27AAE1";
		virtual public string LoginButtonTextColor => "#FFFFFF";

		virtual public string MainSelectedTabColor => CommonAppColor;
		virtual public string MainUnselectedTabColor => "#808080";
		virtual public string MainTodayIcon => "icon_today";
		virtual public string MainLearningIcon => "icon_learning";
		virtual public string MainStatisticsIcon => "icon_stats";
		virtual public string MainSettingsIcon => "icon_settings";
	}
}