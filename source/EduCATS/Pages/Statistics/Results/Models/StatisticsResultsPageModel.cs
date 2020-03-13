using EduCATS.Controls.RoundedListView.Enums;
using EduCATS.Controls.RoundedListView.Interfaces;

namespace EduCATS.Pages.Statistics.Results.Models
{
	public class StatisticsResultsPageModel : IRoundedListType
	{
		public string Title { get; set; }
		public string Date { get; set; }
		public string Comment { get; set; }
		public string Result { get; set; }


		public bool IsTitle { get { return !string.IsNullOrEmpty(Title); } }
		public bool IsDate { get { return !string.IsNullOrEmpty(Date); } }
		public bool IsComment { get { return !string.IsNullOrEmpty(Comment); } }

		public RoundedListTypeEnum GetListType()
		{
			return RoundedListTypeEnum.Navigation;
		}
	}
}
