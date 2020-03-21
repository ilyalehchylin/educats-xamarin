using EduCATS.Networking;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Essentials;

namespace EduCATS.Helpers.Settings
{
	public static class AppPrefs
	{
		const string _languageCodeKey = "APP_LANG_CODE";
		static readonly string _languageCodeDefault = Languages.SYSTEM.LangCode;

		public static string LanguageCode {
			get {
				return Preferences.Get(_languageCodeKey, _languageCodeDefault);
			} set {
				Preferences.Set(_languageCodeKey, value);
			}
		}

		const string _themeKey = "APP_THEME";
		const string _themeDefault = Themes.AppTheme.ThemeDefault;

		public static string Theme {
			get {
				return Preferences.Get(_themeKey, _themeDefault);
			} set {
				Preferences.Set(_themeKey, value);
			}
		}

		const string _serverKey = "APP_SERVER";
		const string _serverDefault = Servers.EduCatsBntuAddress;

		public static string Server {
			get {
				return Preferences.Get(_serverKey, _serverDefault);
			} set {
				Preferences.Set(_serverKey, value);
			}
		}

		const string _isLoggedInKey = "IS_LOGGED_IN";
		const bool _isLoggedInDefault = false;

		public static bool IsLoggedIn {
			get {
				return Preferences.Get(_isLoggedInKey, _isLoggedInDefault);
			}
			set {
				Preferences.Set(_isLoggedInKey, value);
			}
		}

		const string _userLoginKey = "USER_LOGIN";
		const string _userLoginDefault = null;

		public static string UserLogin {
			get {
				return Preferences.Get(_userLoginKey, _userLoginDefault);
			}
			set {
				Preferences.Set(_userLoginKey, value);
			}
		}

		const string _userIdKey = "USER_ID";
		const int _userIdDefault = 0;

		public static int UserId {
			get {
				return Preferences.Get(_userIdKey, _userIdDefault);
			}
			set {
				Preferences.Set(_userIdKey, value);
			}
		}

		const string _chosenSubjectIdKey = "CHOSEN_SUBJECT_ID";
		static readonly int _chosenSubjectIdDefault = 0;

		public static int ChosenSubjectId {
			get {
				return Preferences.Get(_chosenSubjectIdKey, _chosenSubjectIdDefault);
			}
			set {
				Preferences.Set(_chosenSubjectIdKey, value);
			}
		}

		const string _groupIdKey = "USER_GROUP_ID";
		static readonly int _groupIdDefault = -1;

		public static int GroupId {
			get {
				return Preferences.Get(_groupIdKey, _groupIdDefault);
			}
			set {
				Preferences.Set(_groupIdKey, value);
			}
		}

		const string _groupNameKey = "USER_GROUP_NAME";
		static readonly string _groupNameDefault = "";

		public static string GroupName {
			get {
				return Preferences.Get(_groupNameKey, _groupNameDefault);
			}
			set {
				Preferences.Set(_groupNameKey, value);
			}
		}

		const string _avatarKey = "USER_AVATAR";
		static readonly string _avatarDefault = "";

		public static string Avatar {
			get {
				return Preferences.Get(_avatarKey, _avatarDefault);
			}
			set {
				Preferences.Set(_avatarKey, value);
			}
		}

		const string _chosenGroupIdKey = "CHOSEN_GROUP_ID";
		static readonly int _chosenGroupIdDefault = 0;

		public static int ChosenGroupId {
			get {
				return Preferences.Get(_chosenGroupIdKey, _chosenGroupIdDefault);
			}
			set {
				Preferences.Set(_chosenGroupIdKey, value);
			}
		}

		public static void ResetPrefs()
		{
			Preferences.Clear();
		}
	}
}
