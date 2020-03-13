namespace EduCATS.Networking
{
	public static class Links
	{
		public static string Login => $"{Servers.Current}/RemoteApi/Login";
		public static string GetProfileInfo => $"{Servers.Current}/Profile/GetProfileInfo";
		public static string GetNews => $"{Servers.Current}/Profile/GetNews";
		public static string GetProfileInfoSubjects => $"{Servers.Current}/Profile/GetProfileInfoSubjects";
		public static string GetProfileInfoCalendar => $"{Servers.Current}/Profile/GetProfileInfoCalendar";
		public static string GetStatistics => $"{Servers.Current}/Services/Labs/LabsService.svc/GetMarksV2";
		public static string GetOnlyGroups => $"{Servers.Current}/Services/CoreService.svc/GetOnlyGroups";
		public static string GetLabs => $"{Servers.Current}/Services/Labs/LabsService.svc/GetLabsV2";
		public static string GetLectures => $"{Servers.Current}/Services/CoreService.svc/GetLecturesMarkVisitingV2";
	}
}
