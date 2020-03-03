namespace EduCATS.Networking
{
	public static class Links
	{
		public static string Login { get { return Servers.Current + "/RemoteApi/Login"; } }
		public static string GetProfileInfo { get { return Servers.Current + "/Profile/GetProfileInfo"; } }
	}
}