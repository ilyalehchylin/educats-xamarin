namespace EduCATS.Themes.Interfaces
{
	public interface ITheme
	{
		string CommonAppColor { get; }

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
	}
}