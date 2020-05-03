namespace EduCATS.Themes.Interfaces
{
	/// <summary>
	/// App theme interface.
	/// </summary>
	/// <remarks>
	/// Implemented in <see cref="EduCATS.Themes.Templates.DefaultTheme"/>.
	/// Must be defined here before using in <see cref="EduCATS.Themes.Templates.DefaultTheme"/>.
	/// </remarks>
	public interface ITheme
	{
		string BaseAppColor { get; }
		string BaseBlockColor { get; }
		string BaseArrowForwardIcon { get; }
		string BaseCloseIcon { get; }
		string BaseSectionTextColor { get; }
		string BaseActivityIndicatorColorIOS { get; }
		string BaseActivityIndicatorColorAndroid { get; }
		string BasePickerTextColor { get; }
		string BaseLogoImage { get; }
		string BaseHeadphonesIcon { get; }
		string BaseHeadphonesCancelIcon { get; }
		string BaseNoDataTextColor { get; }
		string BaseLinksColor { get; }

		string AppStatusBarBackgroundColor { get; }
		string AppBackgroundColor { get; }
		string AppNavigationBarBackgroundColor { get; }

		string RoundedListViewBackgroundColor { get; }

		string LoginBackground1Image { get; }
		string LoginBackground2Image { get; }
		string LoginBackground3Image { get; }
		string LoginMascotImage { get; }
		string LoginMascotTailImage { get; }
		string LoginShowPasswordImage { get; }
		string LoginEntryBackgroundColor { get; }
		string LoginButtonBackgroundColor { get; }
		string LoginButtonTextColor { get; }
		string LoginSettingsColor { get; }

		string MainSelectedTabColor { get; }
		string MainUnselectedTabColor { get; }
		string MainTodayIcon { get; }
		string MainLearningIcon { get; }
		string MainStatisticsIcon { get; }
		string MainSettingsIcon { get; }

		string TodayCalendarBackgroundColor { get; }
		string TodaySubjectBackgroundColor { get; }
		string TodayNewsItemBackgroundColor { get; }
		string TodayNewsListBackgroundColor { get; }
		string TodayNewsTitleColor { get; }
		string TodayNewsSubjectColor { get; }
		string TodayNewsDateColor { get; }
		string TodayNewsDateIconColor { get; }
		string TodayNewsDateIcon { get; }
		string TodaySelectedTodayDateColor { get; }
		string TodaySelectedAnotherDateColor { get; }
		string TodayNotSelectedDateColor { get; }
		string TodaySelectedDateTextColor { get; }
		string TodayNotSelectedDateTextColor { get; }
		string TodayCalendarBaseTextColor { get; }
		string TodayCalendarSubjectTextColor { get; }

		string NewsTextColor { get; }

		string StatisticsChartLabsColor { get; }
		string StatisticsChartTestsColor { get; }
		string StatisticsChartRatingColor { get; }
		string StatisticsBoxTextColor { get; }
		string StatisticsExpandableTextColor { get; }
		string StatisticsDetailsTitleColor { get; }
		string StatisticsDetailsColor { get; }
		string StatisticsDetailsSeparatorColor { get; }
		string StatisticsDetailsResultsColor { get; }
		string StatisticsDetailsNameColor { get; }
		string StatisticsExpandIcon { get; }
		string StatisticsCollapseIcon { get; }
		string StatisticsCalendarIcon { get; }
		string StatisticsCommentIcon { get; }
		string StatisticsBaseTitleColor { get; }
		string StatisticsBaseRatingTextColor { get; }

		string LearningCardTextColor { get; }
		string LearningCardTestsImage { get; }
		string LearningCardEemcImage { get; }
		string LearningCardFilesImage { get; }
		string LearningCardAdaptiveImage { get; }

		string TestingTitleColor { get; }
		string TestingDescriptionColor { get; }
		string TestingHeaderImage { get; }

		string TestPassingButtonTextColor { get; }
		string TestPassingEntryColor { get; }
		string TestPassingAnswerColor { get; }
		string TestPassingArrowUpIcon { get; }
		string TestPassingArrowDownIcon { get; }
		string TestPassingQuestionColor { get; }
		string TestPassingSelectionColor { get; }
		string TestPassingUnselectedColor { get; }

		string TestResultsAnswerTextColor { get; }
		string TestResultsCorrectAnswerColor { get; }
		string TestResultsNotCorrectAnswerColor { get; }
		string TestResultsRatingColor { get; }

		string EemcHeaderImage { get; }
		string EemcBackButtonColor { get; }
		string EemcBackButtonTextColor { get; }
		string EemcDirectoryActiveIcon { get; }
		string EemcDirectoryInactiveIcon { get; }
		string EemcDocumentActiveIcon { get; }
		string EemcDocumentInactiveIcon { get; }
		string EemcDocumentTestActiveIcon { get; }
		string EemcDocumentTestInactiveIcon { get; }
		string EemcItemTitleColor { get; }

		string FilesHeaderImage { get; }
		string FilesTitleColor { get; }
		string FilesSizeColor { get; }
		string FilesDownloadedIcon { get; }

		string RecommendationsHeaderImage { get; }
		string RecommendationsTitleColor { get; }

		string SettingsServerIcon { get; }
		string SettingsLanguageIcon { get; }
		string SettingsThemeIcon { get; }
		string SettingsAboutIcon { get; }
		string SettingsFontIcon { get; }
		string SettingsLogoutIcon { get; }
		string SettingsGroupUserColor { get; }
		string SettingsTitleColor { get; }

		string CheckboxIcon { get; }
		string CheckboxDescriptionColor { get; }

		string SwitchFrameTextColor { get; }
		string SwitchFrameDescriptionColor { get; }

		string AboutTextColor { get; }
		string AboutButtonTextColor { get; }
		string AboutButtonBackgroundColor { get; }
	}
}
