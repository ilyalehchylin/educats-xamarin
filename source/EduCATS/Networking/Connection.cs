using Xamarin.Essentials;

namespace EduCATS.Networking
{
	public static class Connection
	{
		public static bool IsConnected {
			get {
				var access = Connectivity.NetworkAccess;
				return access == NetworkAccess.Internet ? true : false;
			}
		}
	}
}