using System;
using System.Collections.ObjectModel;
using EduCATS.Helpers.Date;
using EduCATS.Helpers.Logs;
using EduCATS.Pages.Today.Base.Models;
using Xamarin.Forms;

namespace EduCATS.Pages.Today.Base.ViewModels
{
	public class CalendarViewModel : ViewModel
	{
		public ObservableCollection<CalendarViewDayModel> Days { get; set; }
		public int Month { get; set; }
		public int Year { get; set; }

		TodayPageViewModel _todayPageViewModel { get; set; }

		public CalendarViewModel(object bindingContext)
		{
			try {
				if (bindingContext != null && bindingContext is TodayPageViewModel) {
					_todayPageViewModel = (TodayPageViewModel)bindingContext;
				}
			} catch (Exception ex) {
				AppLogs.Log(ex, nameof(CalendarViewModel));
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

		object _calendarSelectedItem;
		public object CalendarSelectedItem {
			get { return _calendarSelectedItem; }
			set { SetProperty(ref _calendarSelectedItem, value); }
		}

		Command _calendarSelectionChangedCommand;
		public Command CalendarSelectionChangedCommand {
			get {
				return _calendarSelectionChangedCommand ?? (
					_calendarSelectionChangedCommand = new Command(
						ExecuteCalendarSelectionChangedEvent));
			}
		}

		protected void ExecuteCalendarSelectionChangedEvent()
		{
			try {
				if (CalendarSelectedItem != null && CalendarSelectedItem.GetType() == typeof(CalendarViewDayModel)) {
					var calendarViewDayModel = (CalendarViewDayModel)CalendarSelectedItem;
					_todayPageViewModel.ExecuteCalendarSelectionChangedEvent(calendarViewDayModel.Date);
					CalendarSelectedItem = null;
				}
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		public static CalendarViewModel Clone(CalendarViewModel calendarViewModel, object context)
		{
			var cloneObject = new CalendarViewModel(context);
			cloneObject.Month = calendarViewModel.Month;
			cloneObject.Year = calendarViewModel.Year;
			cloneObject.Days = new ObservableCollection<CalendarViewDayModel>(calendarViewModel.Days);

			return cloneObject;
		}
	}
}
