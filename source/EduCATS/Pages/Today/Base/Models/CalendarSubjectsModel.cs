using System;
using EduCATS.Controls.RoundedListView.Enums;
using EduCATS.Controls.RoundedListView.Interfaces;

namespace EduCATS.Pages.Today.Base.Models
{
	public class CalendarSubjectsModel : IRoundedListType
	{
		public string Color { get; set; }
		public string Subject { get; set; }
		public DateTime Date { get; set; }

		public RoundedListTypeEnum GetListType()
		{
			return RoundedListTypeEnum.Navigation;
		}
	}
}