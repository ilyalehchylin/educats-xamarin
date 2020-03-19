using EduCATS.Networking;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Essentials;

namespace EduCATS.Helpers.Settings
{
	public static class AppPrefs
	{
		const string languageCodeKey = "APP_LANG_CODE";
		static readonly string languageCodeDefault = Languages.SYSTEM.LangCode;

		public static string LanguageCode {
			get {
				return Preferences.Get(languageCodeKey, languageCodeDefault);
			} set {
				Preferences.Set(languageCodeKey, value);
			}
		}

		const string themeKey = "APP_THEME";
		const string themeDefault = Themes.AppTheme.ThemeDefault;

		public static string Theme {
			get {
				return Preferences.Get(themeKey, themeDefault);
			} set {
				Preferences.Set(themeKey, value);
			}
		}

		const string serverKey = "APP_SERVER";
		const string serverDefault = Servers.EduCatsBntuAddress;

		public static string Server {
			get {
				return Preferences.Get(serverKey, serverDefault);
			} set {
				Preferences.Set(serverKey, value);
			}
		}

		const string isLoggedInKey = "IS_LOGGED_IN";
		const bool isLoggedInDefault = false;

		public static bool IsLoggedIn {
			get {
				return Preferences.Get(isLoggedInKey, isLoggedInDefault);
			}
			set {
				Preferences.Set(isLoggedInKey, value);
			}
		}

		const string userLoginKey = "USER_LOGIN";
		const string userLoginDefault = null;

		public static string UserLogin {
			get {
				return Preferences.Get(userLoginKey, userLoginDefault);
			}
			set {
				Preferences.Set(userLoginKey, value);
			}
		}

		const string userIdKey = "USER_ID";
		const int userIdDefault = 0;

		public static int UserId {
			get {
				return Preferences.Get(userIdKey, userIdDefault);
			}
			set {
				Preferences.Set(userIdKey, value);
			}
		}

		const string chosenSubjectIdKey = "CHOSEN_SUBJECT_ID";
		static readonly int chosenSubjectIdDefault = 0;

		public static int ChosenSubjectId {
			get {
				return Preferences.Get(chosenSubjectIdKey, chosenSubjectIdDefault);
			}
			set {
				Preferences.Set(chosenSubjectIdKey, value);
			}
		}

		const string groupIdKey = "USER_GROUP_ID";
		static readonly int groupIdDefault = -1;

		public static int GroupId {
			get {
				return Preferences.Get(groupIdKey, groupIdDefault);
			}
			set {
				Preferences.Set(groupIdKey, value);
			}
		}

		const string groupNameKey = "USER_GROUP_NAME";
		static readonly string groupNameDefault = "";

		public static string GroupName {
			get {
				return Preferences.Get(groupNameKey, groupNameDefault);
			}
			set {
				Preferences.Set(groupNameKey, value);
			}
		}

		const string avatarKey = "USER_AVATAR";
		static readonly string avatarDefault = "";

		public static string Avatar {
			get {
				return Preferences.Get(avatarKey, avatarDefault);
			}
			set {
				Preferences.Set(avatarKey, value);
			}
		}

		const string chosenGroupIdKey = "CHOSEN_GROUP_ID";
		static readonly int chosenGroupIdDefault = 0;

		public static int ChosenGroupId {
			get {
				return Preferences.Get(chosenGroupIdKey, chosenGroupIdDefault);
			}
			set {
				Preferences.Set(chosenGroupIdKey, value);
			}
		}

		public static void ResetPrefs()
		{
			Preferences.Clear();
		}
	}
}