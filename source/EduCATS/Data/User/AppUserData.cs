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

		public static void SetProfileData(int groupId, string groupName, string userType, string avatar, string name)
		{
			GroupId = groupId;
			GroupName = groupName;
			Avatar = avatar;
			Name = name;

			switch (userType) {
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