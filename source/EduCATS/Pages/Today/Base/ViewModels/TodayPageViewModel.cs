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
using Xamarin.Forms;

namespace EduCATS.Pages.Today.Base.ViewModels
{
	public class TodayPageViewModel : ViewModel
	{
		/// <summary>
		/// Dialog service.
		/// </summary>
		readonly IDialogs dialogService;

		/// <summary>
		/// Navigation service.
		/// </summary>
		readonly IPages navigationService;

		readonly IAppDevice _device;

		const int minimumCalendarPosition = 0;
		const int maximumCalendarPosition = 2;
		const double subjectsHeightByCount = 60;
		const double subjectsAdditionalHeight = 50;

		bool isManualSelectedCalendarDay;
		DateTime manualSelectedCalendarDay;
		List<CalendarSubjectsModel> calendatSubjectsListBackup;

		public TodayPageViewModel(IDialogs dialogs, IPages pages, IAppDevice device)
		{
			_device = device;
			dialogService = dialogs;
			navigationService = pages;

			initSetup();
			update();
		}

		int calendarPosition;
		public int CalendarPosition {
			get { return calendarPosition; }
			set { SetProperty(ref calendarPosition, value); }
		}

		ObservableCollection<CalendarViewModel> calendarList;
		public ObservableCollection<CalendarViewModel> CalendarList {
			get { return calendarList; }
			set { SetProperty(ref calendarList, value); }
		}

		ObservableCollection<string> calendarDaysOfWeekList;
		public ObservableCollection<string> CalendarDaysOfWeekList {
			get { return calendarDaysOfWeekList; }
			set { SetProperty(ref calendarDaysOfWeekList, value); }
		}

		List<NewsPageModel> newsList;
		public List<NewsPageModel> NewsList {
			get { return newsList; }
			set { SetProperty(ref newsList, value); }
		}

		List<CalendarSubjectsModel> calendarSubjects;
		public List<CalendarSubjectsModel> CalendarSubjects {
			get { return calendarSubjects; }
			set { SetProperty(ref calendarSubjects, value); }
		}

		double calendarSubjectsHeight;
		public double CalendarSubjectsHeight {
			get { return calendarSubjectsHeight; }
			set { SetProperty(ref calendarSubjectsHeight, value); }
		}

		string month;
		public string Month {
			get { return month; }
			set { SetProperty(ref month, value); }
		}

		bool isNewsRefreshing;
		public bool IsNewsRefreshing {
			get { return isNewsRefreshing; }
			set { SetProperty(ref isNewsRefreshing, value); }
		}

		bool isNewsRefreshed;
		public bool IsNewsRefreshed {
			get { return isNewsRefreshed; }
			set { SetProperty(ref isNewsRefreshed, value); }
		}

		object selectedNewsItem;
		public object SelectedNewsItem {
			get { return selectedNewsItem; }
			set {
				SetProperty(ref selectedNewsItem, value);

				if (selectedNewsItem != null) {
					openDetailsPage(selectedNewsItem);
				}
			}
		}

		Command newsRefreshCommand;
		public Command NewsRefreshCommand {
			get {
				return newsRefreshCommand ??
					(newsRefreshCommand = new Command(update));
			}
		}

		Command calendarPositionChangedCommand;
		public Command CalendarPositionChangedCommand {
			get {
				return calendarPositionChangedCommand ?? (
					calendarPositionChangedCommand = new Command(
						executeCalendarPositionChangedEvent));
			}
		}

		void initSetup()
		{
			manualSelectedCalendarDay = new DateTime();
			CalendarPosition = 1;
			CalendarSubjects = new List<CalendarSubjectsModel>();
			CalendarDaysOfWeekList = new ObservableCollection<string>(DateHelper.GetDaysWithFirstLetters());
			setInitialCalendarState();

			NewsList = new List<NewsPageModel>();
			calendatSubjectsListBackup = new List<CalendarSubjectsModel>();
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
					Date = DateTime.Parse(c.Start) // TODO: handle null value
				});

			if (calendarSubjectsList == null) {
				return;
			}

			calendatSubjectsListBackup = new List<CalendarSubjectsModel>(calendarSubjectsList);
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
				dialogService.ShowError(DataAccess.ErrorMessage);
			}

			var subjectList = await getSubjects();
			return composeNewsWithSubjects(news, subjectList);
		}

		async Task<IList<SubjectItemModel>> getSubjects()
		{
			return await DataAccess.GetProfileInfoSubjects(AppPrefs.UserLogin);
		}

		List<NewsPageModel> composeNewsWithSubjects(IList<NewsItemModel> news, IList<SubjectItemModel> subjects)
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
			navigationService.OpenNewsDetails(newsPageModel.Title, newsPageModel.Body);
		}

		CalendarViewModel getCalendarViewModel(DateTime date, WeekEnum week)
		{
			var weekStartDate = DateHelper.GetWeekStartDate(date, week);
			var weekDates = DateHelper.GetWeekDays(weekStartDate);

			var calendarDaysModelList = weekDates
				.Select(d => new CalendarViewDayModel {
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

				if (isManualSelectedCalendarDay) {
					selectCalendarDay(manualSelectedCalendarDay);
				} else {
					selectCalendarDay(DateTime.Today);
				}

				switch (CalendarPosition) {
					case minimumCalendarPosition:
						var dateForPreviousWeek = CalendarList[minimumCalendarPosition].Date;
						var previosWeekViewModel = getCalendarViewModel(dateForPreviousWeek, WeekEnum.Previous);
						CalendarList.RemoveAt(maximumCalendarPosition);
						CalendarList.Insert(minimumCalendarPosition, previosWeekViewModel);
						CalendarPosition = minimumCalendarPosition + 1;
						break;
					case maximumCalendarPosition:
						var dateForNextWeek = CalendarList[maximumCalendarPosition].Date;
						var nextWeekViewModel = getCalendarViewModel(dateForNextWeek, WeekEnum.Next);
						CalendarList.RemoveAt(minimumCalendarPosition);
						CalendarList.Insert(maximumCalendarPosition, nextWeekViewModel);
						CalendarPosition = maximumCalendarPosition - 1;
						break;
				}
			} catch (Exception) { }
		}

		public void ExecuteCalendarSelectionChangedEvent(DateTime date)
		{
			selectTodayDateWithoutSelectedFlag();
			deselectAllCalendarDays();
			manualSelectedCalendarDay = date;
			isManualSelectedCalendarDay = true;
			selectCalendarDay(date);
			setFilteredSubjectsList();
		}

		void setFilteredSubjectsList()
		{
			var filteredList = new List<CalendarSubjectsModel>();

			if (isManualSelectedCalendarDay) {
				filteredList = calendatSubjectsListBackup.Where(
					x => x.Date.ToShortDateString() == manualSelectedCalendarDay.ToShortDateString()).ToList();
			} else {
				filteredList = calendatSubjectsListBackup.Where(
					x => x.Date.ToShortDateString() == DateTime.Today.ToShortDateString()).ToList();
			}

			CalendarSubjects = new List<CalendarSubjectsModel>(filteredList);
			setupSubjectsHeight();
		}

		void setupSubjectsHeight()
		{
			CalendarSubjectsHeight = subjectsHeightByCount * CalendarSubjects.Count;

			if (CalendarSubjectsHeight != 0) {
				CalendarSubjectsHeight += subjectsAdditionalHeight;
			}
		}
	}
}