namespace EduCATS.Networking
{
	/// <summary>
	/// API links.
	/// </summary>
	public static class Links
	{
		/// <summary>
		/// Authorize URL.
		/// </summary>
		public static string Login => $"{Servers.Current}/RemoteApi/Login";

		/// <summary>
		/// Get profile URL.
		/// </summary>
		public static string GetProfileInfo => $"{Servers.Current}/Profile/GetProfileInfo";

		/// <summary>
		/// Get news URL.
		/// </summary>
		public static string GetNews => $"{Servers.Current}/Profile/GetNews";

		/// <summary>
		/// Get subjects URL.
		/// </summary>
		public static string GetProfileInfoSubjects => $"{Servers.Current}/Profile/GetProfileInfoSubjects";

		/// <summary>
		/// Get calendar data URL.
		/// </summary>
		public static string GetProfileInfoCalendar => $"{Servers.Current}/Profile/GetProfileInfoCalendar";

		/// <summary>
		/// Get laboratory works statistics URL.
		/// </summary>
		public static string GetStatistics => $"{Servers.Current}/Services/Labs/LabsService.svc/GetMarksV2";

		/// <summary>
		/// Get groups URL.
		/// </summary>
		public static string GetOnlyGroups => $"{Servers.Current}/Services/CoreService.svc/GetOnlyGroups";

		/// <summary>
		/// Get laboratory works URL.
		/// </summary>
		public static string GetLabs => $"{Servers.Current}/Services/Labs/LabsService.svc/GetLabsV2";

		/// <summary>
		/// Get lectures URL.
		/// </summary>
		public static string GetLectures =>
			$"{Servers.Current}/Services/CoreService.svc/GetLecturesMarkVisitingV2";

		/// <summary>
		/// Get avaiable tests URL.
		/// </summary>
		public static string GetAvailableTests => $"{Servers.Current}/TestPassing/GetAvailableTestsForMobile";

		/// <summary>
		/// Get test details URL.
		/// </summary>
		public static string GetTest => $"{Servers.Current}/Tests/GetTest";

		/// <summary>
		/// Get next test's question URL.
		/// </summary>
		public static string GetNextQuestion => $"{Servers.Current}/TestPassing/GetNextQuestionJson";

		/// <summary>
		/// Answer test's question and get next question.
		/// </summary>
		public static string AnswerQuestionAndGetNext =>
			$"{Servers.Current}/TestPassing/AnswerQuestionAndGetNextMobile";

		/// <summary>
		/// Get test answers URL.
		/// </summary>
		public static string GetUserAnswers => $"{Servers.Current}/TestPassing/GetUserAnswers";

		/// <summary>
		/// Get EEMC root concepts URL.
		/// </summary>
		public static string GetRootConcepts =>
			$"{Servers.Current}/Services/Concept/ConceptService.svc/GetRootConceptsMobile";

		/// <summary>
		/// Get EEMC concept tree URL.
		/// </summary>
		public static string GetConceptTree =>
			$"{Servers.Current}/Services/Concept/ConceptService.svc/GetConceptTreeMobile";

		/// <summary>
		/// Get files URL.
		/// </summary>
		public static string GetFiles => $"{Servers.Current}/Subject/GetFileSubjectJson";

		/// <summary>
		/// Get files URL.
		/// </summary>
		public static string GetFile => $"{Servers.Current}/api/Upload";

		/// <summary>
		/// Get recommendations URL.
		/// </summary>
		public static string GetRecomendations => $"{Servers.Current}/Tests/GetRecomendations";

		/// <summary>
		/// Get parental URL.
		/// </summary>
		public static string GetGroupInfo => $"{Servers.Current}/Services/Parental/ParentalService.svc/GetGroupSubjectsByGroupName/";

		/// <summary>
		/// Get registration URL.
		/// </summary>
		public static string Registration => $"{Servers.Current}/Account/Register/";

		/// <summary>
		/// Get verification URL.
		/// </summary>
		public static string VerifySecretQuestion => $"{Servers.Current}/Account/VerifySecretQuestion?";

		/// <summary>
		/// Get reset password URL.
		/// </summary>
		public static string ResetPassword => $"{Servers.Current}/Account/ResetPassword";
	}
}
