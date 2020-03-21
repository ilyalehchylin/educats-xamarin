using EduCATS.Helpers.Settings;

namespace EduCATS.Networking
{
	public static class Servers
	{
		public const string LocalAddress = @"http://172.16.11.72";
		public const string EduCatsAddress = @"http://educats.by";
		public const string EduCatsBntuAddress = @"https://educats.bntu.by";

		const string _localString = "172.16.11.72";
		const string _eduCatsString = "educats.by";
		const string _educatsBntuString = "educats.bntu.by";

		public static string Current => AppPrefs.Server;

		public static void SetCurrent(string server) =>
			AppPrefs.Server = server;

		public static string GetServerType(string server)
		{
			return server switch
			{
				LocalAddress => _localString,
				EduCatsAddress => _eduCatsString,
				EduCatsBntuAddress => _educatsBntuString,
				_ => null,
			};
		}
	}
}
