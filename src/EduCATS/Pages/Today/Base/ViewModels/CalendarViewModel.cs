using System;
using System.Collections.ObjectModel;
using EduCATS.Helpers.Date;
using EduCATS.Pages.Today.Base.Models;
using Xamarin.Forms;

namespace EduCATS.Pages.Today.Base.ViewModels
{
	public class CalendarViewModel : ViewModel
	{
		public ObservableCollection<CalendarViewDayModel> Days { get; set; }
		public int Month { get; set; }
		public int Year { get; set; }

		TodayPageViewModel todayPageViewModel { get; set; }

		public CalendarViewModel(object bindingContext)
		{
			if (bindingContext != null && bindingContext is TodayPageViewModel) {
				todayPageViewModel = (TodayPageViewModel)bindingContext;
			}
		}

		public DateTime Date {
			get {
				if (Days != null && Days.Count > 0) {
					return new DateTime(Year, Month, Days[0].Day);
				}
				return DateTime.Now;
			}
		}

		public string MonthYear {
			get {
				return string.Format(
					"{0} {1}",
					DateHelper.GetMonthName(Month),
					Year);
			}
		}

		object calendarSelectedItem;
		public object CalendarSelectedItem {
			get { return calendarSelectedItem; }
			set { SetProperty(ref calendarSelectedItem, value); }
		}

		Command calendarSelectionChangedCommand;
		public Command CalendarSelectionChangedCommand {
			get {
				return calendarSelectionChangedCommand ?? (
					calendarSelectionChangedCommand = new Command(
						ExecuteCalendarSelectionChangedEvent));
			}
		}

		protected void ExecuteCalendarSelectionChangedEvent()
		{
			if (CalendarSelectedItem != null && CalendarSelectedItem.GetType() == typeof(CalendarViewDayModel)) {
				var calendarViewDayModel = (CalendarViewDayModel)CalendarSelectedItem;
				todayPageViewModel.ExecuteCalendarSelectionChangedEvent(calendarViewDayModel.Date);
				CalendarSelectedItem = null;
			}
		}
	}
}