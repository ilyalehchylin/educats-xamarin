using EduCATS.Networking;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Essentials;

namespace EduCATS.Helpers.Settings
{
	/// <summary>
	/// Application preferences (settings/saved variables).
	/// </summary>
	public static class AppPrefs
	{
		/// <summary>
		/// Language code key.
		/// </summary>
		const string _languageCodeKey = "APP_LANG_CODE";

		/// <summary>
		/// Default language code.
		/// </summary>
		static readonly string _languageCodeDefault = Languages.SYSTEM.LangCode;

		/// <summary>
		/// Language code.
		/// </summary>
		public static string LanguageCode {
			get => Preferences.Get(_languageCodeKey, _languageCodeDefault);
			set => Preferences.Set(_languageCodeKey, value);
		}

		/// <summary>
		/// Theme key.
		/// </summary>
		const string _themeKey = "APP_THEME";

		/// <summary>
		/// Default theme.
		/// </summary>
		const string _themeDefault = Themes.AppTheme.ThemeDefault;

		/// <summary>
		/// Theme.
		/// </summary>
		public static string Theme {
			get => Preferences.Get(_themeKey, _themeDefault);
			set => Preferences.Set(_themeKey, value);
		}

		/// <summary>
		/// Server key.
		/// </summary>
		const string _serverKey = "APP_SERVER";

		/// <summary>
		/// Default server.
		/// </summary>
		const string _serverDefault = Servers.EduCatsBntuAddress;

		/// <summary>
		/// Current server.
		/// </summary>
		public static string Server {
			get => Preferences.Get(_serverKey, _serverDefault);
			set => Preferences.Set(_serverKey, value);
		}

		/// <summary>
		/// Is authorized key.
		/// </summary>
		const string _isLoggedInKey = "IS_LOGGED_IN";

		/// <summary>
		/// Default authorized value.
		/// </summary>
		const bool _isLoggedInDefault = false;

		/// <summary>
		/// Is authorized.
		/// </summary>
		public static bool IsLoggedIn {
			get => Preferences.Get(_isLoggedInKey, _isLoggedInDefault);
			set => Preferences.Set(_isLoggedInKey, value);
		}

		/// <summary>
		/// Username key.
		/// </summary>
		const string _userLoginKey = "USER_LOGIN";

		/// <summary>
		/// Default username.
		/// </summary>
		const string _userLoginDefault = null;

		/// <summary>
		/// Username.
		/// </summary>
		public static string UserLogin {
			get => Preferences.Get(_userLoginKey, _userLoginDefault);
			set => Preferences.Set(_userLoginKey, value);
		}

		/// <summary>
		/// User ID key.
		/// </summary>
		const string _userIdKey = "USER_ID";

		/// <summary>
		/// Default user ID.
		/// </summary>
		const int _userIdDefault = 0;

		/// <summary>
		/// User ID.
		/// </summary>
		public static int UserId {
			get => Preferences.Get(_userIdKey, _userIdDefault);
			set => Preferences.Set(_userIdKey, value);
		}

		/// <summary>
		/// Chosen subject ID key.
		/// </summary>
		const string _chosenSubjectIdKey = "CHOSEN_SUBJECT_ID";

		/// <summary>
		/// Default chosen ID.
		/// </summary>
		static readonly int _chosenSubjectIdDefault = 0;

		/// <summary>
		/// Chosen subject ID.
		/// </summary>
		public static int ChosenSubjectId {
			get => Preferences.Get(_chosenSubjectIdKey, _chosenSubjectIdDefault);
			set => Preferences.Set(_chosenSubjectIdKey, value);
		}

		/// <summary>
		/// Group ID key.
		/// </summary>
		const string _groupIdKey = "USER_GROUP_ID";

		/// <summary>
		/// Default group ID.
		/// </summary>
		static readonly int _groupIdDefault = -1;

		/// <summary>
		/// Group ID.
		/// </summary>
		public static int GroupId {
			get => Preferences.Get(_groupIdKey, _groupIdDefault);
			set => Preferences.Set(_groupIdKey, value);
		}

		/// <summary>
		/// Group name key.
		/// </summary>
		const string _groupNameKey = "USER_GROUP_NAME";

		/// <summary>
		/// Default group name.
		/// </summary>
		static readonly string _groupNameDefault = "";

		/// <summary>
		/// Group name.
		/// </summary>
		public static string GroupName {
			get => Preferences.Get(_groupNameKey, _groupNameDefault);
			set => Preferences.Set(_groupNameKey, value);
		}

		/// <summary>
		/// Group avatar key.
		/// </summary>
		const string _avatarKey = "USER_AVATAR";

		/// <summary>
		/// Default avatar string.
		/// </summary>
		static readonly string _avatarDefault = "";

		/// <summary>
		/// Avatar.
		/// </summary>
		public static string Avatar {
			get => Preferences.Get(_avatarKey, _avatarDefault);
			set => Preferences.Set(_avatarKey, value);
		}

		/// <summary>
		/// Chosen group ID key.
		/// </summary>
		const string _chosenGroupIdKey = "CHOSEN_GROUP_ID";

		/// <summary>
		/// Default chosen group ID.
		/// </summary>
		static readonly int _chosenGroupIdDefault = 0;

		/// <summary>
		/// Chosen group ID.
		/// </summary>
		public static int ChosenGroupId {
			get => Preferences.Get(_chosenGroupIdKey, _chosenGroupIdDefault);
			set => Preferences.Set(_chosenGroupIdKey, value);
		}

		/// <summary>
		/// Delete all preferences.
		/// </summary>
		public static void ResetPrefs()
		{
			Preferences.Clear();
		}
	}
}
