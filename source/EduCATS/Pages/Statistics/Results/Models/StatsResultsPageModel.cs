using EduCATS.Controls.RoundedListView.Enums;
using EduCATS.Controls.RoundedListView.Interfaces;

namespace EduCATS.Pages.Statistics.Results.Models
{
	public class StatsResultsPageModel : IRoundedListType
	{
		public string Title { get; set; }
		public string Date { get; set; }
		public string Comment { get; set; }
		public string Result { get; set; }

		public bool IsTitle { get { return !string.IsNullOrEmpty(Title); } }
		public bool IsDate { get { return !string.IsNullOrEmpty(Date); } }
		public bool IsComment { get { return !string.IsNullOrEmpty(Comment); } }

		public StatsResultsPageModel(string title, string date, string comment, string result)
		{
			Title = title;
			Date = date;
			Comment = comment;
			Result = result;
		}

		public RoundedListTypeEnum GetListType()
		{
			return RoundedListTypeEnum.Navigation;
		}
	}
}
