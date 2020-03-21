namespace EduCATS.Themes.Interfaces
{
	public interface ITheme
	{
		string BaseAppColor { get; }
		string BaseBlockColor { get; }
		string BaseArrowForwardIcon { get; }
		string BaseCloseIcon { get; }

		string AppStatusBarBackgroundColor { get; }
		string AppBackgroundColor { get; }
		string AppNavigationBarBackgroundColor { get; }

		string RoundedListViewBackgroundColor { get; }

		string LoginBackground1Image { get; }
		string LoginBackground2Image { get; }
		string LoginBackground3Image { get; }
		string LoginMascotImage { get; }
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

		string NewsTextColor { get; }

		string StatisticsChartLabsColor { get; }
		string StatisticsChartTestsColor { get; }
		string StatisticsChartVisitingColor { get; }
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

		string TestResultsAnswerTextColor { get; }
		string TestResultsCorrectAnswerColor { get; }
		string TestResultsNotCorrectAnswerColor { get; }

		string EemcHeaderImage { get; }
		string EemcBackButtonColor { get; }
		string EemcBackButtonTextColor { get; }
		string EemcDirectoryActiveIcon { get; }
		string EemcDirectoryInactiveIcon { get; }
		string EemcDocumentActiveIcon { get; }
		string EemcDocumentInactiveIcon { get; }
		string EemcDocumentTestActiveIcon { get; }
		string EemcDocumentTestInactiveIcon { get; }

		string FilesHeaderImage { get; }

		string RecommendationsHeaderImage { get; }

		string SettingsServerIcon { get; }
		string SettingsLanguageIcon { get; }
		string SettingsThemeIcon { get; }
		string SettingsFontIcon { get; }
		string SettingsLogoutIcon { get; }
		string SettingsUserColor { get; }

		string CheckboxIcon { get; }
		string CheckboxDescriptionColor { get; }

		string SwitchFrameDescriptionColor { get; }
	}
}
