using EduCATS.Controls.RoundedListView.Enums;
using EduCATS.Controls.RoundedListView.Interfaces;

namespace EduCATS.Pages.Statistics.Results.Models
{
	public class StatsPageLabsRatingModel : IRoundedListType
	{
		public StatsPageLabsRatingModel(int labId, string shortName, string theme, int subGroup)
		{
			LabId = labId;
			ShortName = shortName;
			Theme = theme;
			SubGroup = subGroup;
		}

		public int LabId { get; set; }
		public string ShortName { get; set; }
		public string Theme { get; set; }
		public int SubGroup { get; set; }

		public RoundedListTypeEnum GetListType()
		{
			return RoundedListTypeEnum.Navigation;
		}
	}
}
