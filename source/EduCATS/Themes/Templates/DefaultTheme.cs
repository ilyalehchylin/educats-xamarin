using EduCATS.Themes.Interfaces;

namespace EduCATS.Themes.Templates
{
	public class DefaultTheme : ITheme
	{
		virtual public string CommonAppColor => "#27AAE1";
		virtual public string CommonBlockColor => "#FFFFFF";
		virtual public string BaseArrowForwardIcon => "icon_forward";

		virtual public string AppBackgroundColor => "#F7F5F3";
		virtual public string AppStatusBarBackgroundColor => CommonAppColor;
		virtual public string AppNavigationBarBackgroundColor => "#FFFFFF";

		virtual public string RoundedListViewBackgroundColor => CommonBlockColor;

		virtual public string LoginBackground1Image => "image_background_1";
		virtual public string LoginBackground2Image => "image_background_2";
		virtual public string LoginBackground3Image => "image_background_3";
		virtual public string LoginMascotImage => "image_mascot";
		virtual public string LoginShowPasswordImage => "icon_show_password";
		virtual public string LoginEntryBackgroundColor => "#FFFFFF";
		virtual public string LoginButtonBackgroundColor => CommonAppColor;
		virtual public string LoginButtonTextColor => "#FFFFFF";

		virtual public string MainSelectedTabColor => CommonAppColor;
		virtual public string MainUnselectedTabColor => "#808080";
		virtual public string MainTodayIcon => "icon_today";
		virtual public string MainLearningIcon => "icon_learning";
		virtual public string MainStatisticsIcon => "icon_stats";
		virtual public string MainSettingsIcon => "icon_settings";

		virtual public string TodayCalendarBackgroundColor => "#FFFFFF";
		virtual public string TodaySubjectBackgroundColor => "#FFFFFF";
		virtual public string TodayNewsItemBackgroundColor => "#FFFFFF";
		virtual public string TodayNewsListBackgroundColor => "#F7F5F3";
		virtual public string TodayNewsTitleColor => "#222222";
		virtual public string TodayNewsSubjectColor => "#808080";
		virtual public string TodayNewsDateColor => "#808080";
		virtual public string TodayNewsDateIconColor => "#ADABAA";
		virtual public string TodayNewsDateIcon => "icon_clock";
		virtual public string TodaySelectedTodayDateColor => "#27AAE1";
		virtual public string TodaySelectedAnotherDateColor => "#808080";
		virtual public string TodayNotSelectedDateColor => "Transparent";
		virtual public string TodaySelectedDateTextColor => "#FFFFFF";
		virtual public string TodayNotSelectedDateTextColor => "#000000";

		virtual public string NewsTextColor => "#000000";

		virtual public string StatisticsChartLabsColor => "#2A4D69";
		virtual public string StatisticsChartTestsColor => "#4B86B4";
		virtual public string StatisticsChartVisitingColor => "#ADCBE3";
		virtual public string StatisticsBoxTextColor => "#FFFFFF";
		virtual public string StatisticsExpandableTextColor => "#808080";
		virtual public string StatisticsDetailsTitleColor => "#000000";
		virtual public string StatisticsDetailsColor => "#808080";
		virtual public string StatisticsDetailsSeparatorColor => "#FCFAF8";
		virtual public string StatisticsDetailsResultsColor => CommonAppColor;
		virtual public string StatisticsDetailsNameColor => "#000000";
		virtual public string StatisticsExpandIcon => "icon_expand";
		virtual public string StatisticsCollapseIcon => "icon_collapse";
		virtual public string StatisticsCalendarIcon => "icon_calendar";
		virtual public string StatisticsCommentIcon => "icon_comment";

		virtual public string LearningCardTextColor => "#FFFFFF";

		virtual public string LearningCardTestsImage => "image_test_square";
		virtual public string LearningCardEumcImage => "image_book_square";
		virtual public string LearningCardFilesImage => "image_file_square";
		virtual public string LearningCardAdaptiveImage => "image_circuit_square";

		virtual public string TestingTitleColor => "#000000";
		virtual public string TestingDescriptionColor => "#808080";
		virtual public string TestingHeaderImage => "image_test_rectangle";

		virtual public string TestPassingButtonTextColor => "#FFFFFF";
		virtual public string TestPassingEntryColor => "#FFFFFF";
		virtual public string TestPassingAnswerColor => "#808080";
		virtual public string TestPassingArrowUpIcon => "icon_arrow_up";
		virtual public string TestPassingArrowDownIcon => "icon_arrow_down";
	}
}
