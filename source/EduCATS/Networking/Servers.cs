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
		static readonly IPlatformServices _services;

		static Servers()
		{
			_services = new PlatformServices();
		}

		/// <summary>
		/// Current server.
		/// </summary>
		public static string Current => _services.Preferences.Server;

		/// <summary>
		/// Set current server.
		/// </summary>
		/// <param name="server">Server to set.</param>
		public static void SetCurrent(string server) =>
			_services.Preferences.Server = server;

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
