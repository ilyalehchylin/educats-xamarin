using System;

namespace EduCATS.Pages.Today.Base.Models
{
	public class CalendarViewDayModel
	{
		public int Day { get; set; }
		public int Month { get; set; }
		public int Year { get; set; }

		public DateTime Date {
			get {
				return new DateTime(Year, Month, Day);
			}
		}

		public string TextColor { get; set; }
		public string SelectionColor { get; set; }
		public bool Selected { get; set; }
	}
}