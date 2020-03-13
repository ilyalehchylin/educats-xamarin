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
		Task OpenStudentsListStats(int pageIndex, int subjectId, List<StatisticsStudentModel> students);
		Task OpenLabsRatingStats();
		Task OpenLabsVisitingStats();
		Task OpenLecturesVisitingStats();
	}
}
