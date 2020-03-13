using EduCATS.Controls.RoundedListView.Enums;
using EduCATS.Controls.RoundedListView.Interfaces;

namespace EduCATS.Pages.Statistics.Results.Models
{
	public class StatisticsResultsPageLabsMarkModel : IRoundedListType
	{
		public StatisticsResultsPageLabsMarkModel(int labId, string shortName, string theme)
		{
			LabId = labId;
			ShortName = shortName;
			Theme = theme;
		}

		public int LabId { get; set; }
		public string ShortName { get; set; }
		public string Theme { get; set; }

		public RoundedListTypeEnum GetListType()
		{
			return RoundedListTypeEnum.Navigation;
		}
	}
}
