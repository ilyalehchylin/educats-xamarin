using EduCATS.Data.Models.User;
using EduCATS.Helpers.Settings;

namespace EduCATS.Data.User
{
	public class AppUserData
	{
		public static int UserId { get; set; }
		public static string Username { get; set; }
		public static UserTypeEnum UserType { get; set; }
		public static int GroupId { get; set; }
		public static string GroupName { get; set; }
		public static string Avatar { get; set; }
		public static string Name { get; set; }

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
				case "1":
					UserType = UserTypeEnum.Professor;
					break;
				case "2":
					UserType = UserTypeEnum.Student;
					break;
			}
		}

		public static void Clear()
		{
			UserId = 0;
			Username = null;
			UserType = 0;
			GroupId = 0;
			GroupName = null;
			Avatar = null;
			Name = null;
		}
	}
}