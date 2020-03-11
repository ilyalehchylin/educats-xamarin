using EduCATS.Helpers.Settings;

namespace EduCATS.Networking
{
	public static class Servers
	{
		public const string LocalAddress = @"http://172.16.11.72";
		public const string EduCatsAddress = @"http://educats.by";
		public const string EduCatsBntuAddress = @"https://educats.bntu.by";

		public static string Current { get { return AppPrefs.Server; } }

		public static void SetCurrent(string server)
		{
			AppPrefs.Server = server;
		}

		public static string GetServerType(string server)
		{
			if (string.Compare(server, LocalAddress) == 0) {
				return "172.16.11.72";
			}

			if (string.Compare(server, EduCatsAddress) == 0) {
				return "educats.by";
			}

			if (string.Compare(server, EduCatsBntuAddress) == 0) {
				return "educats.bntu.by";
			}

			return null;
		}
	}
}