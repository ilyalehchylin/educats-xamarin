namespace EduCATS.Constants
{
	/// <summary>
	/// Global app constants.
	/// </summary>
	public static class GlobalConsts
	{
		/**
		 * Base constants.
		 */

		/// <summary>
		/// Application name.
		/// </summary>
		public const string AppName = "EduCATS";

		/// <summary>
		/// Application namespace (current project's name).
		/// </summary>
		public const string RunNamespace = AppName;

		/// <summary>
		/// Application bundle id.
		/// </summary>
		public const string AppId = "by.bntu.educats";

		/// <summary>
		/// Support address.
		/// </summary>
		public const string SupportAddress = "support@educats.by";

		/// <summary>
		/// Release notes link.
		/// </summary>
		public const string ReleaseNotesLink = "https://github.com/ilyalehchylin/educats-xamarin/releases";

		/// <summary>
		/// GitHub repository.
		/// </summary>
		public const string GitHubLink = "https://github.com/IlyaLehchylin/educats-xamarin";

		/// <summary>
		/// Language used on web-version of LMS.
		/// </summary>
		public const string LMSCoreLanguage = "ru";

		/// <summary>
		/// Localization path (directory inside the project
		/// where localization JSON files are stored).
		/// </summary>
		/// <example>
		/// <c>Languages.Localization</c>
		/// </example>
		public const string LocalizationDirectory = "Localization";

		/**
		 * Caching constants.
		 */

		/// <summary>
		/// News key.
		/// </summary>
		public const string DataGetNewsKey = "GET_NEWS_KEY";

		/// <summary>
		/// Labs key.
		/// </summary>
		public const string DataGetLabsKey = "GET_LABS_KEY";

		/// <summary>
		/// Practs key.
		/// </summary>
		public const string DataGetPractsKey = "GET_PRACTS_KEY";

		/// <summary>
		/// Delete key.
		/// </summary>
		public const string AccountGetDeleteKey = "GET_DELETE_KEY";

		/// <summary>
		/// Marks key.
		/// </summary>
		public const string DataGetMarksKey = "GET_MARKS_KEY";

		/// <summary>
		/// Files key.
		/// </summary>
		public const string DataGetFilesKey = "GET_FILES_KEY";

		/// <summary>
		/// Profile data key.
		/// </summary>
		public const string DataProfileKey = "PROFILE_INFO_KEY";

		/// <summary>
		/// Lectures key.
		/// </summary>
		public const string DataGetLecturesKey = "GET_LECTURES_KEY";

		/// <summary>
		/// Groups key.
		/// </summary>
		public const string DataGetGroupsKey = "GET_ONLYGROUPS_KEY";

		/// <summary>
		/// Tests key.
		/// </summary>
		public const string DataGetTestsKey = "GET_AVAILABLE_TESTS_KEY";

		/// <summary>
		/// Test answers key.
		/// </summary>
		public const string DataGetTestAnswersKey = "GET_USER_ANSWERS_KEY";

		/// <summary>
		/// Electronic Educational Methodological Complexes root concepts key.
		/// </summary>
		public const string DataGetRootConceptKey = "GET_ROOT_CONCEPT_KEY";

		/// <summary>
		/// Electronic Educational Methodological Complexes concept tree key.
		/// </summary>
		public const string DataGetConceptTreeKey = "GET_CONCEPT_TREE_KEY";

		/// <summary>
		/// Subjects key.
		/// </summary>
		public const string DataGetSubjectsKey = "GET_PROFILE_INFO_SUBJECT_KEY";

		/// <summary>
		/// Calendar subjects key.
		/// </summary>
		public const string DataGetCalendarKey = "GET_PROFILE_INFO_CALENDAR_KEY";

		/// <summary>
		/// Recommendations key.
		/// </summary>
		public const string DataGetRecommendationsKey = "GET_RECOMMENDATIONS_KEY";

		/// <summary>
		/// Cache expiration (in days).
		/// </summary>
		public const int CacheExpirationInDays = 7;

		/// <summary>
		/// Android platform.
		/// </summary>
		public const string AndroidPlatform = "Android";
	}
}
