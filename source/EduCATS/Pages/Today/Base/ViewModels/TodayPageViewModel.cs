using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using EduCATS.Data;
using EduCATS.Data.Models.Calendar;
using EduCATS.Data.Models.News;
using EduCATS.Data.Models.Subjects;
using EduCATS.Helpers.Date;
using EduCATS.Helpers.Date.Enums;
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

		const int minimumCalendarPosition = 0;
		const int maximumCalendarPosition = 2;
		const double subjectsHeightByCount = 60;
		const double subjectsAdditionalHeight = 50;

		bool isManualSelectedCalendarDay;
		DateTime manualSelectedCalendarDay;
		List<CalendarSubjectsModel> calendatSubjectsListBackup;

		public TodayPageViewModel(IDialogs dialogs, IPages pages)
		{
			dialogService = dialogs;
			navigationService = pages;

			initSetup();

			Task.Run(async () => await startRetrievingData());
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
				return newsRefreshCommand ?? (newsRefreshCommand = new Command(
					async () => await executeNewsRefreshCommand()));
			}
		}

		Command calendarPositionChangedCommand;
		public Command CalendarPositionChangedCommand {
			get {
				return calendarPositionChangedCommand ?? (
					calendarPositionChangedCommand = new Command(
						ExecuteCalendarPositionChangedEvent));
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

		protected async Task executeNewsRefreshCommand()
		{
			await startRetrievingData();
		}

		async Task startRetrievingData()
		{
			await getAndSetNews();
			await getAndSetCalendarNotes();
		}

		async Task getAndSetCalendarNotes()
		{
			var calendarNotesModel = await DataAccess.GetProfileInfoCalendar(AppPrefs.UserLogin);

			if (calendarNotesModel.IsError) {
				await dialogService.ShowError(calendarNotesModel.ErrorMessage);
			}

			var calendarSubjectsList = calendarNotesModel.Labs.Select(
				c => new CalendarSubjectsModel {
					Color = c.Color,
					Subject = c.Title,
					Date = DateTime.Parse(c.Start)
				});

			calendatSubjectsListBackup = new List<CalendarSubjectsModel>(calendarSubjectsList);
			setFilteredSubjectsList();
		}

		async Task getAndSetNews()
		{
			setRefreshing(true);
			var newsPageList = await getNews();
			NewsList = newsPageList;
			setRefreshing(false);
		}

		async Task<List<NewsPageModel>> getNews()
		{
			var newsModel = await DataAccess.GetNews(AppPrefs.UserLogin);

			if (newsModel.IsError) {
				await dialogService.ShowError(newsModel.ErrorMessage);
				return null;
			}

			var subjectList = await getSubjects();

			if (subjectList == null) {
				return null;
			}

			return composeNewsWithSubjects(newsModel.News, subjectList);
		}

		async Task<IList<SubjectItemModel>> getSubjects()
		{
			var subjects = await DataAccess.GetProfileInfoSubjects(AppPrefs.UserLogin);

			if (subjects.IsError) {
				await dialogService.ShowError(subjects.ErrorMessage);
				return null;
			}

			return subjects.SubjectsList;
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

		protected void ExecuteCalendarPositionChangedEvent()
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

		void setRefreshing(bool isRefreshing)
		{
			IsNewsRefreshing = isRefreshing;
			IsNewsRefreshed = !isRefreshing;
		}
	}
}