namespace EduCATS.Themes.Templates
{
	/// <summary>
	/// Dark theme to override the <see cref="DefaultTheme"/>.
	/// </summary>
	public class DarkTheme : DefaultTheme
	{
		const string _baseDarkColor = "#333333";
		const string _whiteColor = "#FFFFFF";
		const string _blackColor = "#000000";
		const string _greyColor = "#ADABAA";

		override public string BaseAppColor => _whiteColor;
		override public string BaseBlockColor => _baseDarkColor;
		override public string BaseSectionTextColor => _whiteColor;
		override public string BaseActivityIndicatorColor => _whiteColor;
		override public string BasePickerTextColor => _whiteColor;

		override public string AppBackgroundColor => _blackColor;
		override public string AppStatusBarBackgroundColor => _blackColor;
		override public string AppNavigationBarBackgroundColor => _baseDarkColor;

		override public string RoundedListViewBackgroundColor => _baseDarkColor;

		override public string LoginBackground1Image => "image_background_1_dark";
		override public string LoginBackground2Image => "image_background_2_dark";
		override public string LoginBackground3Image => "image_background_3_dark";
		override public string LoginShowPasswordImage => "icon_show_password_dark";
		override public string LoginButtonBackgroundColor => _baseDarkColor;

		override public string MainSelectedTabColor => "#27AEE1";
		override public string MainUnselectedTabColor => _whiteColor;

		override public string TodayCalendarBackgroundColor => _baseDarkColor;
		override public string TodaySubjectBackgroundColor => _baseDarkColor;
		override public string TodayNewsItemBackgroundColor => _baseDarkColor;
		override public string TodayNewsListBackgroundColor => _blackColor;
		override public string TodayNewsTitleColor => _whiteColor;
		override public string TodayNewsSubjectColor => _greyColor;
		override public string TodayNewsDateColor => _greyColor;
		override public string TodayNewsDateIconColor => _greyColor;
		override public string TodayNotSelectedDateTextColor => _whiteColor;
		override public string TodayCalendarBaseTextColor => _whiteColor;
		override public string TodayCalendarSubjectTextColor => _whiteColor;

		override public string NewsTextColor => _whiteColor;

		override public string TestingTitleColor => _whiteColor;
		override public string TestingDescriptionColor => _greyColor;

		override public string TestPassingButtonTextColor => _whiteColor;
		override public string TestPassingEntryColor => _whiteColor;
		override public string TestPassingAnswerColor => _greyColor;
		override public string TestPassingQuestionColor => _whiteColor;
		override public string TestPassingUnselectedColor => _whiteColor;

		override public string TestResultsAnswerTextColor => _whiteColor;
		override public string TestResultsRatingColor => _whiteColor;

		override public string FilesTitleColor => _whiteColor;

		override public string RecommendationsTitleColor => _whiteColor;

		override public string StatisticsBaseTitleColor => _whiteColor;
		override public string StatisticsExpandableTextColor => _whiteColor;
		override public string StatisticsDetailsTitleColor => _whiteColor;
		override public string StatisticsBaseRatingTextColor => _whiteColor;
		override public string StatisticsDetailsSeparatorColor => _blackColor;
		override public string StatisticsDetailsNameColor => _whiteColor;
		override public string SettingsTitleColor => _whiteColor;

		override public string EemcBackButtonColor => _baseDarkColor;
		override public string EemcBackButtonTextColor => _whiteColor;
		override public string EemcItemTitleColor => _whiteColor;

		override public string SwitchFrameTextColor => _whiteColor;
	}
}
