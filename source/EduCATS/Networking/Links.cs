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
		public static string LoginTest => $"{Servers.Current}/Account/UserSessionCheck";

		public static string Login => $"{Servers.Current}/RemoteApi/Login";

		/// <summary>
		/// Authorize URL.
		/// </summary>
		public static string LoginTestServer => $"{Servers.EduCatsAddress}/Account/LoginJWT";

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
		/// Get subjects test URL.
		/// </summary>
		public static string GetProfileInfoSubjectsTest => $"{Servers.Current}/Services/Subjects/SubjectsService.svc/GetUserSubjects";

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
		public static string GetLabsTest => $"{Servers.Current}/Services/Labs/LabsService.svc/GetLabsV2?";

		public static string GetPracticialsTest => $"{Servers.EduCatsByAddress}/subject/Services/Practicals/PracticalService.svc/GetPracticalsV2?";

		/// <summary>
		/// Get lectures URL.
		/// </summary>
		public static string GetLectures =>$"{Servers.Current}/Services/CoreService.svc/GetLecturesMarkVisitingV2";

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
		/// Get test's result URL.
		/// </summary>
		public static string GetResultTest => $"{Servers.Current}/TestPassing/CloseTestAndGetResult";

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
		/// Get labs URL
		/// </summary>
		public static string GetLabs => $"{Servers.Current}/Services/Labs/LabsService.svc/GetLabs/";

		/// <summary>
		/// Get practicals URL
		/// </summary>
		public static string GetPracticals => $"{Servers.Current}/Services/Practicals/PracticalService.svc/GetPracticals/";

		/// <summary>
		/// Get EEMC root concepts URL.
		/// </summary>
		public static string GetRootConcepts =>
			$"{Servers.Current}/Services/Concept/ConceptService.svc/GetRootConceptsMobile";

		/// <summary>
		/// Get EEMC root concepts test URL.
		/// </summary>
		public static string GetRootConceptsTest =>
			$"{Servers.Current}/Services/Concept/ConceptService.svc/GetRootConcepts";

		/// <summary>
		/// Get EEMC concept tree URL.
		/// </summary>
		public static string GetConceptTree =>
			$"{Servers.Current}/Services/Concept/ConceptService.svc/GetConceptTreeMobile";

		/// <summary>
		/// Get EEMC concept cascade URL.
		/// </summary>
		public static string GetConceptCascade =>
			$"{Servers.Current}/Services/Concept/ConceptService.svc/GetConceptCascade";

		/// <summary>
		/// Get files URL.
		/// </summary>
		public static string GetFiles => $"{Servers.Current}/Subject/GetFileSubjectJson";

		/// <summary>
		/// Get files test URL.
		/// </summary>
		public static string GetFilesTest => $"{Servers.Current}/subject/Subject/GetFileSubjectV2";
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

		public static string VerifyUserExists => $"{Servers.EduCatsByAddress}/Account/UserExists?";
		/// <summary>
		/// Get verification URL.
		/// </summary>
		public static string VerifySecretQuestion => $"{Servers.Current}/Account/VerifySecretQuestion?";

		/// <summary>
		/// Get reset password URL.
		/// </summary>
		public static string ResetPassword => $"{Servers.Current}/Account/ResetPassword";

		/// <summary>
		/// Get delete account URL.
		/// </summary>
		public static string DeleteAccount => $"{Servers.Current}/Account/DeleteAccount";

		/// <summary>
		/// Get save marks calendar data lectures URL.
		/// </summary>
		public static string GetLecturesCalendarData =>
			"/subject/Services/Lectures/LecturesService.svc/GetLecturesMarkVisitingV3?";

		/// <summary>
		/// Save marks calendar data lectures URL.
		/// </summary>
		public static string SaveLecturesCalendarData =>
			$"{Servers.Current}/Services/Lectures/LecturesService.svc/SaveMarksCalendarData";

		/// <summary>
		/// Get save marks calendar data lectures URL.
		/// </summary>
		public static string GetLabsCalendarData =>
			$"/subject/Services/Labs/LabsService.svc/GetMarksV3?";

		public static string GetParticialsMarks => $"/subject/Services/Practicals/PracticalService.svc/GetMarks";

		public static string SaveLabsMark => $"{Servers.Current}/Services/Labs/LabsService.svc/SaveLabsVisitingData";

		public static string SaveStudentLabsMark => $"{Servers.Current}/Services/Labs/LabsService.svc/SaveStudentLabsMark";

		public static string SaveStudentPracticalMark => $"{Servers.EduCatsByAddress}/subject/Services/Practicals/PracticalService.svc/SavePracticalsVisitingData";

		public static string SaveSingleLab => $"{Servers.EduCatsByAddress}/subject/Services/Labs/LabsService.svc/SaveStudentLabsMark";

		public static string SaveSinglePract => $"{Servers.EduCatsByAddress}/subject/Services/Practicals/PracticalService.svc/SaveStudentPracticalMark";
		/// <summary>
		/// Get save marks calendar data lectures URL.
		/// </summary>
		//public static string SavesCalendarData =>
		//$"{Servers.Current}/subject/Services/Lectures/LecturesService.svc/SaveMarksCalendarData";
	}
}
