using Xamarin.Essentials;

namespace EduCATS.Networking
{
	/// <summary>
	/// Connection helper.
	/// </summary>
	public static class Connection
	{
		/// <summary>
		/// Is connected to the Internet or to the local network.
		/// </summary>
		public static bool IsConnected =>
			Connectivity.NetworkAccess == NetworkAccess.Internet ? true : false;
	}
}
