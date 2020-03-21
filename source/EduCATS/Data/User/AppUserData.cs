using EduCATS.Data.Models.User;
using EduCATS.Helpers.Settings;

namespace EduCATS.Data.User
{
	public class AppUserData
	{
		const string _professorType = "1";
		const string _studentType = "2";

		public static int UserId { get; set; }
		public static string Name { get; set; }
		public static int GroupId { get; set; }
		public static string Avatar { get; set; }
		public static string Username { get; set; }
		public static string GroupName { get; set; }
		public static UserTypeEnum UserType { get; set; }

		public static void SetLoginData(int userId, string username)
		{
			AppPrefs.UserId = userId;
			AppPrefs.UserLogin = username;

			UserId = userId;
			Username = username;
		}

		public static void SetProfileData(UserProfileModel profile)
		{
			if (profile == null) {
				return;
			}

			GroupId = profile.GroupId;
			GroupName = profile.GroupName;
			Avatar = profile.Avatar;
			Name = profile.Name;

			switch (profile.UserType) {
				case _professorType:
					UserType = UserTypeEnum.Professor;
					break;
				case _studentType:
					UserType = UserTypeEnum.Student;
					break;
			}
		}

		public static void Clear()
		{
			UserId = 0;
			GroupId = 0;
			UserType = 0;
			Name = null;
			Avatar = null;
			Username = null;
			GroupName = null;
		}
	}
}
