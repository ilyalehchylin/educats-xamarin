using EduCATS.Controls.RoundedListView.Enums;
using EduCATS.Controls.RoundedListView.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCATS.Pages.Statistics.Results.Models
{
	public class StatsPagePractiseVisitingModel : IRoundedListType
	{
		public StatsPagePractiseVisitingModel(int prId, string shortName, string theme)
		{
			PrId = prId;
			ShortName = shortName;
			Theme = theme;
		}

		public int PrId { get; set; }
		public string ShortName { get; set; }
		public string Theme { get; set; }

		public RoundedListTypeEnum GetListType()
		{
			return RoundedListTypeEnum.Navigation;
		}
	}
}
