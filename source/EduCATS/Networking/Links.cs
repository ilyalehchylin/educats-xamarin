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
		public static string GetAvailableTests => $"{Servers.Current}/TestPassing/GetAvailableTestsForMobile";
		public static string GetTest => $"{Servers.Current}/Tests/GetTest";
		public static string GetNextQuestion => $"{Servers.Current}/TestPassing/GetNextQuestionJson";
		public static string AnswerQuestionAndGetNext => $"{Servers.Current}/TestPassing/AnswerQuestionAndGetNextMobile";
		public static string GetUserAnswers => $"{Servers.Current}/TestPassing/GetUserAnswers";
		public static string GetRootConcepts => $"{Servers.Current}/Services/Concept/ConceptService.svc/GetRootConceptsMobile";
		public static string GetConceptTree => $"{Servers.Current}/Services/Concept/ConceptService.svc/GetConceptTreeMobile";
	}
}
