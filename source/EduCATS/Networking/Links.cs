namespace EduCATS.Networking
{
	public static class Links
	{
		public static string Login { get { return $"{Servers.Current}/RemoteApi/Login"; } }
		public static string GetProfileInfo { get { return $"{Servers.Current}/Profile/GetProfileInfo"; } }
		public static string GetNews { get { return $"{Servers.Current}/Profile/GetNews"; } }
		public static string GetProfileInfoSubjects { get { return $"{Servers.Current}/Profile/GetProfileInfoSubjects"; } }
		public static string GetProfileInfoCalendar { get { return $"{Servers.Current}/Profile/GetProfileInfoCalendar"; } }
		public static string GetStatistics { get { return $"{Servers.Current}/Services/Labs/LabsService.svc/GetMarksV2"; } }
	}
}