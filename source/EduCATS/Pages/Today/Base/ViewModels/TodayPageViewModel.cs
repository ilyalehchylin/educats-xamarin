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
using EduCATS.Helpers.Settings;
using EduCATS.Pages.Today.Base.Models;
using EduCATS.Themes;
using Xamarin.Forms;

namespace EduCATS.Pages.Today.Base.ViewModels
{
	public class TodayPageViewModel : ViewModel
	{
		const int minimumCalendarPosition = 0;
		const int maximumCalendarPosition = 2;

		bool isManualSelectedCalendarDay;
		DateTime manualSelectedCalendarDay = new DateTime();

		List<SubjectItemModel> subjects { get; set; }
		List<NewsPageModel> newsBackupList { get; set; }

		public TodayPageViewModel()
		{
			CalendarDaysOfWeekList = new ObservableCollection<string>(
				DateHelper.GetDaysOfWeekWithFirstLetters());
			CalendarSubjects = new List<CalendarSubjectsModel>();

			setInitialCalendarState();

			CalendarPosition = 1;

			subjects = new List<SubjectItemModel>();
			NewsList = new List<NewsPageModel>();
			newsBackupList = new List<NewsPageModel>();
			IsNewsRefreshing = true;
			IsNewsRefreshed = false;
			loadNews();

			ChosenDate = DateTime.Now;

			Device.BeginInvokeOnMainThread(async () => await getCalendarNotes());
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

		CalendarViewModel getCalendarViewModel(DateTime date, WeekEnum week)
		{
			var calendarDaysModelList = new ObservableCollection<CalendarViewDayModel>();
			var weekStartDate = DateHelper.GetWeekStartDate(date, week);
			var weekDates = DateHelper.GetWeekDays(weekStartDate);

			if (weekDates != null && weekDates.Count > 0) {
				foreach (var weekDate in weekDates) {
					calendarDaysModelList.Add(new CalendarViewDayModel {
						Day = weekDate.Day,
						Month = weekDate.Month,
						Year = weekDate.Year
					});
				}
			}

			return new CalendarViewModel(this) {
				Days = calendarDaysModelList,
				Month = weekStartDate.Month,
				Year = weekStartDate.Year
			};
		}

		void selectTodayDateWithoutSelectedFlag()
		{
			if (CalendarList != null && CalendarList.Count > 0) {
				foreach (var calendarModel in CalendarList) {
					var calendarDayModel = calendarModel.Days.FirstOrDefault(
						dayModel => dayModel.Date.ToShortDateString()
							.Equals(DateTime.Today.ToShortDateString()));

					if (calendarDayModel != null) {
						changeCalendarSelection(calendarModel, calendarDayModel, false);
						break;
					}
				}
			}
		}

		void selectCalendarDay(DateTime dateToSelect)
		{
			if (CalendarList != null && CalendarList.Count > 0) {
				foreach (var calendarModel in CalendarList) {
					CalendarViewDayModel calendarDayModel = calendarModel.Days.FirstOrDefault(
						dayModel => dayModel.Date.ToShortDateString()
							.Equals(dateToSelect.ToShortDateString()));

					if (calendarDayModel != null) {
						changeCalendarSelection(calendarModel, calendarDayModel, true);
						break;
					}
				}
			}
		}

		void deselectAllCalendarDays()
		{
			if (CalendarList != null && CalendarList.Count > 0) {
				foreach (var calendarModel in CalendarList) {
					var calendarDayModel = calendarModel.Days.FirstOrDefault(
						dayModel => dayModel.Selected);
					if (calendarDayModel != null) {
						changeCalendarSelection(calendarModel, calendarDayModel, false);
						break;
					}
				}
			}
		}

		void changeCalendarSelection(
			CalendarViewModel calendarModel,
			CalendarViewDayModel calendarDayModel,
			bool selected)
		{
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

			try {
				CalendarList[indexCalendarModel].Days[indexCalendarDayModel] = calendarDayModel;
			} catch (ObjectDisposedException) { }
		}

		DateTime chosenDate;
		public DateTime ChosenDate {
			get { return chosenDate; }
			set { SetProperty(ref chosenDate, value); }
		}

		void setChosenDate(DateTime chosenDateTime)
		{
			var previousWeekCalendarModel = getCalendarViewModel(chosenDateTime, WeekEnum.Previous);
			var currentWeekCalendarModel = getCalendarViewModel(chosenDateTime, WeekEnum.Current);
			var nextWeekCalendarModel = getCalendarViewModel(chosenDateTime, WeekEnum.Next);

			CalendarList = new ObservableCollection<CalendarViewModel> {
				previousWeekCalendarModel,
				currentWeekCalendarModel,
				nextWeekCalendarModel
			};

			ExecuteCalendarSelectionChangedEvent(chosenDateTime);
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
				return newsRefreshCommand ?? (newsRefreshCommand = new Command(ExecuteNewsRefreshCommand));
			}
		}

		protected void ExecuteNewsRefreshCommand()
		{
			IsNewsRefreshed = false;
			IsNewsRefreshing = true;
			loadNews();
		}

		void loadNews()
		{
			Device.BeginInvokeOnMainThread(
				async () => {
					var newsModel = await DataAccess.GetNews(AppPrefs.UserLogin) as NewsModel;
					var subjectsModel = await DataAccess.GetProfileInfoSubjects(AppPrefs.UserLogin) as SubjectModel;
					subjects = new List<SubjectItemModel>(subjectsModel.SubjectsList);

					var newsPageList = new List<NewsPageModel>();

					if (subjects != null) {
						if (subjects.Count > 0) {
							foreach (var news in newsModel.News) {
								var subject = subjects.FirstOrDefault(x => x.Id == news.SubjectId);
								if (subject != null) {
									newsPageList.Add(new NewsPageModel(news, subject.Color));
								} else {
									newsPageList.Add(new NewsPageModel(news, null));
								}
							}
						}
					}

					NewsList = newsPageList;
					newsBackupList = newsPageList;
					IsNewsRefreshing = false;
					IsNewsRefreshed = true;
				});
		}

		void openDetailsPage(object obj)
		{
			var newsPageModel = (NewsPageModel)obj;
			//AppPage.NewsDetails(newsPageModel);
		}

		Command calendarPositionChangedCommand;
		public Command CalendarPositionChangedCommand {
			get {
				return calendarPositionChangedCommand ?? (
					calendarPositionChangedCommand = new Command(
						ExecuteCalendarPositionChangedEvent));
			}
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
			} catch (Exception ex) {

			}
		}

		public void ExecuteCalendarSelectionChangedEvent(DateTime date)
		{
			selectTodayDateWithoutSelectedFlag();
			deselectAllCalendarDays();
			manualSelectedCalendarDay = date;
			isManualSelectedCalendarDay = true;
			selectCalendarDay(date);

			var filteredList = new List<CalendarSubjectsModel>();

			if (isManualSelectedCalendarDay) {
				filteredList = cnListBackup.Where(x => x.Date.ToShortDateString() == manualSelectedCalendarDay.ToShortDateString()).ToList();
			} else {
				filteredList = cnListBackup.Where(x => x.Date.ToShortDateString() == DateTime.Today.ToShortDateString()).ToList();
			}

			CalendarSubjects = new List<CalendarSubjectsModel>(filteredList);
			CalendarSubjectsHeight = 60 * CalendarSubjects.Count;
			if (CalendarSubjectsHeight != 0) {
				CalendarSubjectsHeight += 50;
			}
		}

		List<CalendarSubjectsModel> cnListBackup = new List<CalendarSubjectsModel>();

		async Task getCalendarNotes()
		{
			var calendarNotesModel = await DataAccess.GetProfileInfoCalendar(AppPrefs.UserLogin) as CalendarModel;
			var cnList = calendarNotesModel.Labs.Select(c => new CalendarSubjectsModel {
				Color = c.Color,
				Subject = c.Title,
				Date = DateTime.Parse(c.Start)
			});

			cnListBackup = new List<CalendarSubjectsModel>(cnList);

			var filteredList = new List<CalendarSubjectsModel>();

			if (isManualSelectedCalendarDay) {
				filteredList = cnList.Where(x => x.Date.ToShortDateString() == manualSelectedCalendarDay.ToShortDateString()).ToList();
			} else {
				filteredList = cnList.Where(x => x.Date.ToShortDateString() == DateTime.Today.ToShortDateString()).ToList();
			}

			CalendarSubjects = new List<CalendarSubjectsModel>(filteredList);
			CalendarSubjectsHeight = 60 * CalendarSubjects.Count;
			if (CalendarSubjectsHeight != 0) {
				CalendarSubjectsHeight += 50;
			}
		}
	}
}