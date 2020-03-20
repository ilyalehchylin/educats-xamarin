using System.Collections.Generic;
using System.Threading.Tasks;
using EduCATS.Data.Models.Statistics;

namespace EduCATS.Helpers.Pages.Interfaces
{
	public interface IPages
	{
		Task ClosePage(bool modal);
		void OpenLogin();
		void OpenMain();
		Task OpenNewsDetails(string title, string body);
		Task OpenStudentsListStats(int pageIndex, int subjectId, List<StatisticsStudentModel> students, string title);
		Task OpenDetailedStatistics(string userLogin, int subjectId, int groupId, int pageIndex, string title, string name = null);
		Task OpenTesting(string title);
		Task OpenTestPassing(int testId, bool forSelfStudy, bool fromComplexLearning = false);
		Task OpenTestResults(int testId, bool fromComplexLearning = false);
		Task OpenEemc(string title, int searchId = -1);
		Task OpenFiles(string title);
		Task OpenRecommendations(string title);
		Task OpenSettings(string title);
		Task OpenSettingsLanguage(string title);
		Task OpenSettingsServer(string title);
	}
}
