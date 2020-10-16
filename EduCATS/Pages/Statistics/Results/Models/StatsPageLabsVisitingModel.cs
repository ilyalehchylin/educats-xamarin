using EduCATS.Controls.RoundedListView.Enums;
using EduCATS.Controls.RoundedListView.Interfaces;

namespace EduCATS.Pages.Statistics.Results.Models
{
	public class StatsPageLabsVisitingModel : IRoundedListType
	{
		public StatsPageLabsVisitingModel(int protectionLabId, string date)
		{
			ProtectionLabId = protectionLabId;
			Date = date;
		}

		public int ProtectionLabId { get; set; }
		public string Date { get; set; }

		public RoundedListTypeEnum GetListType()
		{
			return RoundedListTypeEnum.Navigation;
		}
	}
}
