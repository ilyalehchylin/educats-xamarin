using EduCATS.Themes.Interfaces;

namespace EduCATS.Themes.Templates
{
	/// <summary>
	/// <see cref="ITheme"/> implementation (Default light theme).
	/// </summary>
	/// <remarks>
	/// Used to set values for a theme.
	/// </remarks>
	public class DefaultTheme : ITheme
	{
		const string _greyColor = "#808080";
		const string _whiteColor = "#FFFFFF";
		const string _baseBlueColor = "#27AAE1";
		const string _lightGreyColor = "#F7F5F3";
		const string _blackColor = "#000000";

		virtual public string BaseAppColor => _baseBlueColor;
		virtual public string BaseBlockColor => _whiteColor;
		virtual public string BaseArrowForwardIcon => "icon_forward";
		virtual public string BaseCloseIcon => "icon_close";
		virtual public string BaseSectionTextColor => _blackColor;
		virtual public string BaseActivityIndicatorColor => _blackColor;
		virtual public string BasePickerTextColor => _blackColor;

		virtual public string AppBackgroundColor => _lightGreyColor;
		virtual public string AppStatusBarBackgroundColor => _baseBlueColor;
		virtual public string AppNavigationBarBackgroundColor => _whiteColor;

		virtual public string RoundedListViewBackgroundColor => _whiteColor;

		virtual public string LoginBackground1Image => "image_background_1";
		virtual public string LoginBackground2Image => "image_background_2";
		virtual public string LoginBackground3Image => "image_background_3";
		virtual public string LoginMascotImage => "image_mascot";
		virtual public string LoginShowPasswordImage => "icon_show_password";
		virtual public string LoginEntryBackgroundColor => _whiteColor;
		virtual public string LoginButtonBackgroundColor => _baseBlueColor;
		virtual public string LoginButtonTextColor => _whiteColor;
		virtual public string LoginSettingsColor => _whiteColor;

		virtual public string MainSelectedTabColor => _baseBlueColor;
		virtual public string MainUnselectedTabColor => _greyColor;
		virtual public string MainTodayIcon => "icon_today";
		virtual public string MainLearningIcon => "icon_learning";
		virtual public string MainStatisticsIcon => "icon_stats";
		virtual public string MainSettingsIcon => "icon_settings";

		virtual public string TodayCalendarBackgroundColor => _whiteColor;
		virtual public string TodaySubjectBackgroundColor => _whiteColor;
		virtual public string TodayNewsItemBackgroundColor => _whiteColor;
		virtual public string TodayNewsListBackgroundColor => _lightGreyColor;
		virtual public string TodayNewsTitleColor => "#222222";
		virtual public string TodayNewsSubjectColor => _greyColor;
		virtual public string TodayNewsDateColor => _greyColor;
		virtual public string TodayNewsDateIconColor => "#ADABAA";
		virtual public string TodayNewsDateIcon => "icon_clock";
		virtual public string TodaySelectedTodayDateColor => _baseBlueColor;
		virtual public string TodaySelectedAnotherDateColor => _greyColor;
		virtual public string TodayNotSelectedDateColor => "Transparent";
		virtual public string TodaySelectedDateTextColor => _whiteColor;
		virtual public string TodayNotSelectedDateTextColor => _blackColor;
		virtual public string TodayCalendarBaseTextColor => _blackColor;
		virtual public string TodayCalendarSubjectTextColor => _blackColor;

		virtual public string NewsTextColor => _blackColor;

		virtual public string StatisticsChartLabsColor => "#2A4D69";
		virtual public string StatisticsChartTestsColor => "#4B86B4";
		virtual public string StatisticsChartVisitingColor => "#ADCBE3";
		virtual public string StatisticsBoxTextColor => _whiteColor;
		virtual public string StatisticsExpandableTextColor => _greyColor;
		virtual public string StatisticsDetailsTitleColor => _blackColor;
		virtual public string StatisticsDetailsColor => _greyColor;
		virtual public string StatisticsDetailsSeparatorColor => "#FCFAF8";
		virtual public string StatisticsDetailsResultsColor => _baseBlueColor;
		virtual public string StatisticsDetailsNameColor => _blackColor;
		virtual public string StatisticsExpandIcon => "icon_expand";
		virtual public string StatisticsCollapseIcon => "icon_collapse";
		virtual public string StatisticsCalendarIcon => "icon_calendar";
		virtual public string StatisticsCommentIcon => "icon_comment";
		virtual public string StatisticsBaseTitleColor => _blackColor;
		virtual public string StatisticsBaseRatingTextColor => _blackColor;

		virtual public string LearningCardTextColor => _whiteColor;

		virtual public string LearningCardTestsImage => "image_test_square";
		virtual public string LearningCardEemcImage => "image_book_square";
		virtual public string LearningCardFilesImage => "image_file_square";
		virtual public string LearningCardAdaptiveImage => "image_circuit_square";

		virtual public string TestingTitleColor => _blackColor;
		virtual public string TestingDescriptionColor => _greyColor;
		virtual public string TestingHeaderImage => "image_test_rectangle";

		virtual public string TestPassingButtonTextColor => _whiteColor;
		virtual public string TestPassingEntryColor => _whiteColor;
		virtual public string TestPassingAnswerColor => _greyColor;
		virtual public string TestPassingArrowUpIcon => "icon_arrow_up";
		virtual public string TestPassingArrowDownIcon => "icon_arrow_down";
		virtual public string TestPassingQuestionColor => _blackColor;
		virtual public string TestPassingSelectionColor => _baseBlueColor;
		virtual public string TestPassingUnselectedColor => _blackColor;

		virtual public string TestResultsAnswerTextColor => _blackColor;
		virtual public string TestResultsCorrectAnswerColor => "#00AA55";
		virtual public string TestResultsNotCorrectAnswerColor => "#E63022";
		virtual public string TestResultsRatingColor => _blackColor;

		virtual public string EemcHeaderImage => "image_book_rectangle";
		virtual public string EemcBackButtonColor => _baseBlueColor;
		virtual public string EemcBackButtonTextColor => _whiteColor;
		virtual public string EemcDirectoryActiveIcon => "icon_directory_active";
		virtual public string EemcDirectoryInactiveIcon => "icon_directory_inactive";
		virtual public string EemcDocumentActiveIcon => "icon_document_pdf_active";
		virtual public string EemcDocumentInactiveIcon => "icon_document_pdf_inactive";
		virtual public string EemcDocumentTestActiveIcon => "icon_document_test_active";
		virtual public string EemcDocumentTestInactiveIcon => "icon_document_test_active";
		virtual public string EemcItemTitleColor => _blackColor;

		virtual public string FilesHeaderImage => "image_file_rectangle";
		virtual public string FilesTitleColor => _blackColor;

		virtual public string RecommendationsHeaderImage => "image_circuit_rectangle";
		virtual public string RecommendationsTitleColor => _blackColor;

		virtual public string SettingsServerIcon => "icon_settings_server";
		virtual public string SettingsLanguageIcon => "icon_settings_language";
		virtual public string SettingsThemeIcon => "icon_settings_themes";
		virtual public string SettingsFontIcon => "icon_settings_font";
		virtual public string SettingsLogoutIcon => "icon_settings_logout";
		virtual public string SettingsGroupUserColor => _greyColor;
		virtual public string SettingsTitleColor => _blackColor;

		virtual public string CheckboxIcon => "icon_checkmark";
		virtual public string CheckboxDescriptionColor => _greyColor;

		virtual public string SwitchFrameTextColor => _blackColor;
		virtual public string SwitchFrameDescriptionColor => _greyColor;
	}
}
