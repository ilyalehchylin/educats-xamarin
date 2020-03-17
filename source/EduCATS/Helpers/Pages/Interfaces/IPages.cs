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
	}
}
