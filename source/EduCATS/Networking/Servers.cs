using EduCATS.Helpers.Forms;

namespace EduCATS.Networking
{
	/// <summary>
	/// Servers helper.
	/// </summary>
	public static class Servers
	{
		/// <summary>
		/// Local server address.
		/// </summary>
		public const string LocalAddress = @"http://172.16.11.72";

		/// <summary>
		/// Test server address.
		/// </summary>
		public const string EduCatsAddress = @"http://educats.by";

		/// <summary>
		/// Stable server address.
		/// </summary>
		public const string EduCatsBntuAddress = @"https://educats.bntu.by";

		/// <summary>
		/// Local server name string.
		/// </summary>
		const string _localString = "172.16.11.72";

		/// <summary>
		/// EduCATS test server name string.
		/// </summary>
		const string _eduCatsString = "educats.by";

		/// <summary>
		/// EduCATS stable server name string.
		/// </summary>
		const string _educatsBntuString = "educats.bntu.by";

		/// <summary>
		/// Platform services.
		/// </summary>
		public static IPlatformServices PlatformServices;

		static Servers()
		{
			if (PlatformServices == null) {
				PlatformServices = new PlatformServices();
			}
		}

		/// <summary>
		/// Current server.
		/// </summary>
		public static string Current => PlatformServices.Preferences.Server;

		/// <summary>
		/// Set current server.
		/// </summary>
		/// <param name="server">Server to set.</param>
		public static void SetCurrent(string server) =>
			PlatformServices.Preferences.Server = server;

		/// <summary>
		/// Get server name string by address.
		/// </summary>
		/// <param name="server">Server address.</param>
		/// <returns>Server name.</returns>
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
