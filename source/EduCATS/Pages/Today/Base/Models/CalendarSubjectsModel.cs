using System;
using EduCATS.Controls.RoundedListView.Enums;
using EduCATS.Controls.RoundedListView.Interfaces;
using EduCATS.Data.Models.Calendar;
using EduCATS.Helpers.Date;

namespace EduCATS.Pages.Today.Base.Models
{
	public class CalendarSubjectsModel : IRoundedListType
	{
		public CalendarSubjectsModel(CalendarSubjectModel calendarSubjectModel)
		{
			if (calendarSubjectModel == null) {
				return;
			}

			Color = calendarSubjectModel.Color;
			Subject = calendarSubjectModel.Title;
			Date = DateTime.Parse(calendarSubjectModel.Start ?? DateHelper.DefaultDateTime);
		}

		public string Color { get; set; }
		public string Subject { get; set; }
		public DateTime Date { get; set; }

		public RoundedListTypeEnum GetListType()
		{
			return RoundedListTypeEnum.Navigation;
		}
	}
}
