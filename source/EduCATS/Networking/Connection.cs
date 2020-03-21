using Xamarin.Essentials;

namespace EduCATS.Networking
{
	public static class Connection
	{
		public static bool IsConnected =>
			Connectivity.NetworkAccess == NetworkAccess.Internet ? true : false;
	}
}
