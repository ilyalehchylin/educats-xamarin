using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using EduCATS.Data;
using EduCATS.Data.Models.News;
using EduCATS.Data.Models.Subjects;
using EduCATS.Helpers.Date;
using EduCATS.Helpers.Date.Enums;
using EduCATS.Helpers.Devices.Interfaces;
using EduCATS.Helpers.Dialogs.Interfaces;
using EduCATS.Helpers.Pages.Interfaces;
using EduCATS.Helpers.Settings;
using EduCATS.Pages.Today.Base.Models;
using EduCATS.Themes;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS.Pages.Today.Base.ViewModels
{
	public class TodayPageViewModel : ViewModel
	{
		/// <summary>
		/// Dialog service.
		/// </summary>
		readonly IDialogs _dialogs;

		/// <summary>
		/// Navigation service.
		/// </summary>
		readonly IPages _pages;

		/// <summary>
		/// Device service.
		/// </summary>
		readonly IDevice _device;

		readonly double _subjectHeight;
		readonly double _subjectsHeaderHeight;

		const int _minimumCalendarPosition = 0;
		const int _maximumCalendarPosition = 2;
		const double _subjectsHeightToAdd = 55;

		bool _isManualSelectedCalendarDay;
		DateTime _manualSelectedCalendarDay;
		List<CalendarSubjectsModel> _calendarSubjectsBackup;

		public TodayPageViewModel(double subjectHeight, double subjectsHeaderHeight,
			IDialogs dialogs, IPages pages, IDevice device)
		{
			_subjectHeight = subjectHeight;
			_subjectsHeaderHeight = subjectsHeaderHeight;
			_device = device;
			_dialogs = dialogs;
			_pages = pages;

			initSetup();
			update();
		}

		int _calendarPosition;
		public int CalendarPosition {
			get { return _calendarPosition; }
			set { SetProperty(ref _calendarPosition, value); }
		}

		ObservableCollection<CalendarViewModel> _calendarList;
		public ObservableCollection<CalendarViewModel> CalendarList {
			get { return _calendarList; }
			set { SetProperty(ref _calendarList, value); }
		}

		ObservableCollection<string> _calendarDaysOfWeekList;
		public ObservableCollection<string> CalendarDaysOfWeekList {
			get { return _calendarDaysOfWeekList; }
			set { SetProperty(ref _calendarDaysOfWeekList, value); }
		}

		List<NewsPageModel> _newsList;
		public List<NewsPageModel> NewsList {
			get { return _newsList; }
			set { SetProperty(ref _newsList, value); }
		}

		List<CalendarSubjectsModel> _calendarSubjects;
		public List<CalendarSubjectsModel> CalendarSubjects {
			get { return _calendarSubjects; }
			set { SetProperty(ref _calendarSubjects, value); }
		}

		double _calendarSubjectsHeight;
		public double CalendarSubjectsHeight {
			get { return _calendarSubjectsHeight; }
			set { SetProperty(ref _calendarSubjectsHeight, value); }
		}

		string _month;
		public string Month {
			get { return _month; }
			set { SetProperty(ref _month, value); }
		}

		bool _isNewsRefreshing;
		public bool IsNewsRefreshing {
			get { return _isNewsRefreshing; }
			set { SetProperty(ref _isNewsRefreshing, value); }
		}

		bool _isNewsRefreshed;
		public bool IsNewsRefreshed {
			get { return _isNewsRefreshed; }
			set { SetProperty(ref _isNewsRefreshed, value); }
		}

		object _selectedNewsItem;
		public object SelectedNewsItem {
			get { return _selectedNewsItem; }
			set {
				SetProperty(ref _selectedNewsItem, value);

				if (_selectedNewsItem != null) {
					openDetailsPage(_selectedNewsItem);
				}
			}
		}

		Command _newsRefreshCommand;
		public Command NewsRefreshCommand {
			get {
				return _newsRefreshCommand ??
					(_newsRefreshCommand = new Command(update));
			}
		}

		Command _calendarPositionChangedCommand;
		public Command CalendarPositionChangedCommand {
			get {
				return _calendarPositionChangedCommand ?? (
					_calendarPositionChangedCommand = new Command(
						executeCalendarPositionChangedEvent));
			}
		}

		void initSetup()
		{
			_manualSelectedCalendarDay = new DateTime();
			CalendarPosition = 1;
			CalendarSubjects = new List<CalendarSubjectsModel>();
			CalendarDaysOfWeekList = new ObservableCollection<string>(DateHelper.GetDaysWithFirstLetters());
			setInitialCalendarState();

			NewsList = new List<NewsPageModel>();
			_calendarSubjectsBackup = new List<CalendarSubjectsModel>();
		}

		void setInitialCalendarState()
		{
			var todayDateTime = DateTime.Today;
			var previousWeekCalendarModel = getCalendarViewModel(todayDateTime, WeekEnum.Previous);
			var currentWeekCalendarModel = getCalendarViewModel(todayDateTime, WeekEnum.Current);
			var nextWeekCalendarModel = getCalendarViewModel(todayDateTime, WeekEnum.Next);

			CalendarList = new ObservableCollection<CalendarViewModel> {
				previousWeekCalendarModel,
				currentWeekCalendarModel,
				nextWeekCalendarModel
			};

			selectCalendarDay(todayDateTime);
		}

		void update()
		{
			_device.MainThread(async () => {
				IsNewsRefreshing = true;
				await getAndSetNews();
				await getAndSetCalendarNotes();
				IsNewsRefreshing = false;
			});
		}

		async Task getAndSetCalendarNotes()
		{
			var calendar = await DataAccess.GetProfileInfoCalendar(AppPrefs.UserLogin);

			if (calendar == null) {
				return;
			}

			var calendarSubjectsList = calendar.Labs?.Select(
				c => new CalendarSubjectsModel {
					Color = c.Color,
					Subject = c.Title,
					Date = DateTime.Parse(c.Start ?? DateHelper.DefaultDateTime)
				});

			if (calendarSubjectsList == null) {
				return;
			}

			_calendarSubjectsBackup = new List<CalendarSubjectsModel>(calendarSubjectsList);
			setFilteredSubjectsList();
		}

		async Task getAndSetNews()
		{
			var news = await getNews();

			if (news != null) {
				NewsList = new List<NewsPageModel>(news);
			}
		}

		async Task<List<NewsPageModel>> getNews()
		{
			var news = await DataAccess.GetNews(AppPrefs.UserLogin);

			if (DataAccess.IsError && !DataAccess.IsConnectionError) {
				_dialogs.ShowError(DataAccess.ErrorMessage);
			}

			var subjectList = await getSubjects();
			return composeNewsWithSubjects(news, subjectList);
		}

		async Task<IList<SubjectModel>> getSubjects()
		{
			return await DataAccess.GetProfileInfoSubjects(AppPrefs.UserLogin);
		}

		List<NewsPageModel> composeNewsWithSubjects(IList<NewsModel> news, IList<SubjectModel> subjects)
		{
			if (news == null || subjects == null) {
				return null;
			}

			return news.Select(n => {
				var subject = subjects.FirstOrDefault(s => s.Id == n.SubjectId);
				return new NewsPageModel(n, subject?.Color);
			}).ToList();
		}

		void openDetailsPage(object obj)
		{
			var newsPageModel = (NewsPageModel)obj;
			_pages.OpenNewsDetails(
				CrossLocalization.Translate("news_details_title"),
				newsPageModel.Title,
				newsPageModel.Body);
		}

		CalendarViewModel getCalendarViewModel(DateTime date, WeekEnum week)
		{
			var weekStartDate = DateHelper.GetWeekStartDate(date, week);
			var weekDates = DateHelper.GetWeekDays(weekStartDate);

			var calendarDaysModelList = weekDates
				.Select(d => new CalendarViewDayModel {
					TextColor = Theme.Current.TodayCalendarBaseTextColor,
					Day = d.Day,
					Month = d.Month,
					Year = d.Year
				});

			return new CalendarViewModel(this) {
				Days = new ObservableCollection<CalendarViewDayModel>(calendarDaysModelList),
				Month = weekStartDate.Month,
				Year = weekStartDate.Year
			};
		}

		void selectTodayDateWithoutSelectedFlag()
		{
			selectDay(DateTime.Today, false);
		}

		void selectCalendarDay(DateTime dateToSelect)
		{
			selectDay(dateToSelect, true);
		}

		void deselectAllCalendarDays()
		{
			selectDay(deselect: true);
		}

		void selectDay(DateTime dateToCheck = default, bool selected = false, bool deselect = false)
		{
			if (CalendarList == null) {
				return;
			}

			foreach (var calendarModel in CalendarList) {
				var calendarDayModel = calendarModel.Days.FirstOrDefault(d => {
					return deselect ?
					d.Selected :
					d.Date.ToShortDateString().Equals(dateToCheck.ToShortDateString());

				});

				if (calendarDayModel != null) {
					changeCalendarSelection(calendarModel, calendarDayModel, selected);
					break;
				}
			}
		}

		void changeCalendarSelection(
			CalendarViewModel calendarModel,
			CalendarViewDayModel calendarDayModel,
			bool selected)
		{
			try {
				var indexCalendarModel = CalendarList.IndexOf(calendarModel);
				var indexCalendarDayModel = calendarModel.Days.IndexOf(calendarDayModel);

				if (DateTime.Today == calendarDayModel.Date) {
					calendarDayModel.SelectionColor = Theme.Current.TodaySelectedTodayDateColor;
				} else {
					if (selected) {
						calendarDayModel.SelectionColor = Theme.Current.TodaySelectedAnotherDateColor;
					} else {
						calendarDayModel.SelectionColor = Theme.Current.TodayNotSelectedDateColor;
					}
				}

				calendarDayModel.Selected = selected;

				if (selected) {
					calendarDayModel.TextColor = Theme.Current.TodaySelectedDateTextColor;
				} else {
					calendarDayModel.TextColor = Theme.Current.TodayNotSelectedDateTextColor;
				}

				CalendarList[indexCalendarModel].Days[indexCalendarDayModel] = calendarDayModel;
			} catch (ObjectDisposedException) { }
		}

		protected void executeCalendarPositionChangedEvent()
		{
			try {
				selectTodayDateWithoutSelectedFlag();
				deselectAllCalendarDays();

				if (_isManualSelectedCalendarDay) {
					selectCalendarDay(_manualSelectedCalendarDay);
				} else {
					selectCalendarDay(DateTime.Today);
				}

				switch (CalendarPosition) {
					case _minimumCalendarPosition:
						var dateForPreviousWeek = CalendarList[_minimumCalendarPosition].Date;
						var previosWeekViewModel = getCalendarViewModel(dateForPreviousWeek, WeekEnum.Previous);
						CalendarList.RemoveAt(_maximumCalendarPosition);
						CalendarList.Insert(_minimumCalendarPosition, previosWeekViewModel);
						CalendarPosition = _minimumCalendarPosition + 1;
						break;
					case _maximumCalendarPosition:
						var dateForNextWeek = CalendarList[_maximumCalendarPosition].Date;
						var nextWeekViewModel = getCalendarViewModel(dateForNextWeek, WeekEnum.Next);
						CalendarList.RemoveAt(_minimumCalendarPosition);
						CalendarList.Insert(_maximumCalendarPosition, nextWeekViewModel);
						CalendarPosition = _maximumCalendarPosition - 1;
						break;
				}
			} catch (Exception) { }
		}

		public void ExecuteCalendarSelectionChangedEvent(DateTime date)
		{
			selectTodayDateWithoutSelectedFlag();
			deselectAllCalendarDays();
			_manualSelectedCalendarDay = date;
			_isManualSelectedCalendarDay = true;
			selectCalendarDay(date);
			setFilteredSubjectsList();
		}

		void setFilteredSubjectsList()
		{
			var filteredList = new List<CalendarSubjectsModel>();

			if (_isManualSelectedCalendarDay) {
				filteredList = _calendarSubjectsBackup.Where(
					x => x.Date.ToShortDateString() == _manualSelectedCalendarDay.ToShortDateString()).ToList();
			} else {
				filteredList = _calendarSubjectsBackup.Where(
					x => x.Date.ToShortDateString() == DateTime.Today.ToShortDateString()).ToList();
			}

			CalendarSubjects = new List<CalendarSubjectsModel>(filteredList);
			setupSubjectsHeight();
		}

		void setupSubjectsHeight()
		{
			if (CalendarSubjects.Count == 0) {
				CalendarSubjectsHeight = 0;
				return;
			}

			CalendarSubjectsHeight =
				(_subjectHeight * CalendarSubjects.Count) +
				(_subjectsHeaderHeight * 2) + _subjectsHeightToAdd;
		}
	}
}
