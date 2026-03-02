using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using EduCATS.Data;
using EduCATS.Data.Models;
using EduCATS.Data.Models.Calendar;
using EduCATS.Helpers.Date;
using EduCATS.Helpers.Date.Enums;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Logs;
using EduCATS.Networking;
using EduCATS.Networking.AppServices;
using EduCATS.Pages.Today.Base.Models;
using EduCATS.Themes;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Essentials;
using Xamarin.Forms;
using EduCATS.Data.User;
using EduCATS.Demo;

namespace EduCATS.Pages.Today.Base.ViewModels
{
	public class TodayPageViewModel : ViewModel
	{
		readonly IPlatformServices _services;

		readonly double _subjectHeight;
		readonly double _subjectsHeightToSubtract;
		readonly bool _isLargeFont;

		const int _minimumCalendarPosition = 0;
		const int _maximumCalendarPosition = 2;
		const double _emptySubjectsHeight = 120;
		const double _emptySubjectsHeightLarge = 130;
		const int _consultationsCount = 1000;
		const int _consultationsPage = 1;
		const string _scheduleQueryDateFormat = "dd-MM-yyyy";

		static readonly string[] _scheduleDateFormats = {
			"dd/MM/yyyy",
			"dd.MM.yyyy",
			"yyyy-MM-dd",
			"yyyy-MM-ddTHH:mm:ss",
			"yyyy-MM-ddTHH:mm:ss.fff",
			"MM/dd/yyyy"
		};

		static readonly string[] _timeFormats = {
			"HH:mm",
			"H:mm",
			"HH:mm:ss",
			"H:mm:ss"
		};

		// bool _isCreation = true;
		bool _isManualSelectedCalendarDay;
		bool _isCalendarPositionChangeInProgress;
		DateTime _manualSelectedCalendarDay;
		List<CalendarSubjectsModel> _calendarSubjectsBackup;
		readonly Dictionary<int, string> _lecturerNamesCache = new Dictionary<int, string>();

		public TodayPageViewModel(double subjectHeight, double subjectsHeaderHeight, IPlatformServices services)
		{
			_subjectHeight = services.Preferences.IsLargeFont ? (subjectHeight + 90) : subjectHeight;
			_subjectsHeightToSubtract = services.Preferences.IsLargeFont ? 95 : 85;
			_isLargeFont = services.Preferences.IsLargeFont;
			_services = services;
			Version = _services.Device.GetVersion();

			initSetup();
			update();
		}

		int _calendarPosition;
		public int CalendarPosition
		{
			get { return _calendarPosition; }
			set { SetProperty(ref _calendarPosition, value); }
		}

		ObservableCollection<CalendarViewModel> _calendarList;
		public ObservableCollection<CalendarViewModel> CalendarList
		{
			get { return _calendarList; }
			set { SetProperty(ref _calendarList, value); }
		}

		ObservableCollection<string> _calendarDaysOfWeekList;
		public ObservableCollection<string> CalendarDaysOfWeekList
		{
			get { return _calendarDaysOfWeekList; }
			set { SetProperty(ref _calendarDaysOfWeekList, value); }
		}

		List<NewsPageModel> _newsList;
		public List<NewsPageModel> NewsList
		{
			get { return _newsList; }
			set { SetProperty(ref _newsList, value); }
		}

		List<SubjectPageModel> _newsSubjectList;
		public List<SubjectPageModel> NewsSubjectList
		{
			get { return _newsSubjectList; }
			set { SetProperty(ref _newsSubjectList, value); }
		}

		List<CalendarSubjectsModel> _calendarSubjects;
		public List<CalendarSubjectsModel> CalendarSubjects
		{
			get { return _calendarSubjects; }
			set { SetProperty(ref _calendarSubjects, value); }
		}

		double _calendarSubjectsHeight;
		public double CalendarSubjectsHeight
		{
			get { return _calendarSubjectsHeight; }
			set { SetProperty(ref _calendarSubjectsHeight, value); }
		}

		string _month;
		public string Month
		{
			get { return _month; }
			set { SetProperty(ref _month, value); }
		}

		bool _isCalendarRefreshing;
		public bool IsCalendarRefreshing
		{
			get { return _isCalendarRefreshing; }
			set { SetProperty(ref _isCalendarRefreshing, value); }
		}

		bool _isNewsRefreshing;
		public bool IsNewsRefreshing
		{
			get { return _isNewsRefreshing; }
			set { SetProperty(ref _isNewsRefreshing, value); }
		}

		bool _isNewsRefreshed;
		public bool IsNewsRefreshed
		{
			get { return _isNewsRefreshed; }
			set { SetProperty(ref _isNewsRefreshed, value); }
		}

		object _selectedNewsItem;
		public object SelectedNewsItem
		{
			get { return _selectedNewsItem; }
			set
			{
				SetProperty(ref _selectedNewsItem, value);

				if (_selectedNewsItem != null)
				{
					openDetailsPage(_selectedNewsItem);
				}
			}
		}
		string _version;
		public string Version
		{
			get { return _version; }
			set { SetProperty(ref _version, value); }
		}

		Command _newsRefreshCommand;
		public Command NewsRefreshCommand
		{
			get
			{
				return _newsRefreshCommand ??
					(_newsRefreshCommand = new Command(update));
			}
		}

		Command<CarouselView.FormsPlugin.Abstractions.PositionSelectedEventArgs> _calendarPositionChangedCommand;
		public Command<CarouselView.FormsPlugin.Abstractions.PositionSelectedEventArgs> PositionSelectedCommandProperty
		{
			get
			{
				return _calendarPositionChangedCommand ?? (
					_calendarPositionChangedCommand = new Command<CarouselView.FormsPlugin.Abstractions.PositionSelectedEventArgs>(
						calendarPositionChangedEvent));
			}
		}

		void initSetup()
		{
			try
			{
				_manualSelectedCalendarDay = new DateTime();
				CalendarSubjects = new List<CalendarSubjectsModel>();
				NewsSubjectList = new List<SubjectPageModel>();
				CalendarDaysOfWeekList = new ObservableCollection<string>(DateHelper.GetDaysWithFirstLetters());
				setInitialCalendarState();
				NewsList = new List<NewsPageModel>();
				_calendarSubjectsBackup = new List<CalendarSubjectsModel>();
			}
			catch (Exception ex)
			{
				AppLogs.Log(ex);
			}
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
			CalendarPosition = 1;
			selectCalendarDay(todayDateTime);
		}
		void update()
		{
			_services.Device.MainThread(async () => {
				try
				{
					IsNewsRefreshing = true;
					await getAndSetCalendarNotes();
					await getAndSetNews();
					await getUpdateMessage();
					IsNewsRefreshing = false;
				}
				catch (Exception ex)
				{
					AppLogs.Log(ex);
				}
			});
		}

		async Task getAndSetCalendarNotes()
		{
			DateTime date;
			if (_isManualSelectedCalendarDay)
			{
				date = _manualSelectedCalendarDay;
			}
			else
			{
				date = DateTime.Today;
			}

			await setNewSubjectList(date);
		}

		async Task getAndSetNews()
		{
			var news = await getNews();

			if (news != null)
			{
				NewsList = new List<NewsPageModel>(news);
			}
		}

		async Task getUpdateMessage()
		{
			string version = await AppServices.GerVersionStore();
			string[] a = version.Split('.');
			string[] b = _version.Split('.');
			if ((Convert.ToInt32(a[0]) > Convert.ToInt32(b[0])) ||
				(Convert.ToInt32(a[1]) > Convert.ToInt32(b[1]) && Convert.ToInt32(a[0]) == Convert.ToInt32(b[0])) ||
				(Convert.ToInt32(a[2]) > Convert.ToInt32(b[2]) && Convert.ToInt32(a[0]) == Convert.ToInt32(b[0]) &&
				Convert.ToInt32(a[1]) == Convert.ToInt32(b[1])))
			{
				string title = CrossLocalization.Translate("update_title");
				string message = CrossLocalization.Translate("update_message");
				string linkButton = CrossLocalization.Translate("update_link_button");
				string cancelButton = CrossLocalization.Translate("update_cancel_button");

				var result = await _services.Dialogs.ShowMessageUpdate(title, message + version, linkButton, cancelButton);
				if (result)
				{
					if (Device.RuntimePlatform == Device.Android)
						await _services.Device.OpenUri(Servers.EducatsBntuAndroidMarketString);
					else if (Device.RuntimePlatform == Device.iOS)
						await Launcher.OpenAsync(new Uri(Servers.EducatsBntuIOSMarketString));
				}
			}
		}

		async Task<List<NewsPageModel>> getNews()
		{
			var news = await DataAccess.GetNews(_services.Preferences.UserLogin);

			if (DataAccess.IsError && DataAccess.IsSessionExpiredError)
			{
				_services.Dialogs.ShowError(DataAccess.ErrorMessage);
				AppDemo.Instance.IsDemoAccount = false;
				_services.Preferences.ResetPrefs();
				AppUserData.Clear();
				DataAccess.ResetData();
				_services.Navigation.OpenLogin();
				return null;
			}

			if (DataAccess.IsError && !DataAccess.IsConnectionError)
			{
				_services.Dialogs.ShowError(DataAccess.ErrorMessage);
			}

			var subjectList = await getSubjects();

			if (subjectList == null)
			{
				return composeNewsWithSubjects(news, null);
			}

			return composeNewsWithSubjects(news, subjectList.OrderBy(x => x.Name).ToList());
		}

		async Task<IList<SubjectModel>> getSubjects()
		{
			return await DataAccess.GetProfileInfoSubjects(_services.Preferences.UserLogin);
		}

		List<NewsPageModel> composeNewsWithSubjects(IList<NewsModel> news, IList<SubjectModel> subjects)
		{
			if (news == null || subjects == null)
			{
				return null;
			}

			return news.Select(n => {
				var subject = subjects.FirstOrDefault(s => s.Id == n.SubjectId);
				return new NewsPageModel(n, subject?.Color);
			}).ToList();
		}

		void openDetailsPage(object obj)
		{
			try
			{
				var newsPageModel = (NewsPageModel)obj;
				_services.Navigation.OpenNewsDetails(
					newsPageModel.Title,
					newsPageModel.Body);
			}
			catch (Exception ex)
			{
				AppLogs.Log(ex);
			}
		}

		CalendarViewModel getCalendarViewModel(DateTime date, WeekEnum week)
		{
			var weekStartDate = DateHelper.GetWeekStartDate(date, week);
			var weekDates = DateHelper.GetWeekDays(weekStartDate);

			var calendarDaysModelList = weekDates
				.Select(d => new CalendarViewDayModel
				{
					TextColor = Theme.Current.TodayCalendarBaseTextColor,
					Day = d.Day,
					Month = d.Month,
					Year = d.Year
				});

			return new CalendarViewModel(this)
			{
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
			if (CalendarList == null)
			{
				return;
			}

			for (var calendarModelIndex = 0; calendarModelIndex < CalendarList.Count; calendarModelIndex++)
			{
				var calendarModel = CalendarList[calendarModelIndex];
				if (calendarModel?.Days == null)
				{
					continue;
				}

				for (var calendarDayModelIndex = 0; calendarDayModelIndex < calendarModel.Days.Count; calendarDayModelIndex++)
				{
					var calendarDayModel = calendarModel.Days[calendarDayModelIndex];
					var isSelectedDay = deselect
						? calendarDayModel.Selected
						: calendarDayModel.Date.Date == dateToCheck.Date;

					if (!isSelectedDay)
					{
						continue;
					}

					changeCalendarSelection(calendarModelIndex, calendarDayModelIndex, selected);
					return;
				}
			}
		}

		void changeCalendarSelection(
			int indexCalendarModel,
			int indexCalendarDayModel,
			bool selected)
		{
			try
			{
				if (CalendarList == null ||
					indexCalendarModel < 0 ||
					indexCalendarModel >= CalendarList.Count)
				{
					return;
				}

				var calendarModel = CalendarList[indexCalendarModel];
				if (calendarModel?.Days == null ||
					indexCalendarDayModel < 0 ||
					indexCalendarDayModel >= calendarModel.Days.Count)
				{
					return;
				}

				var calendarDayModel = calendarModel.Days[indexCalendarDayModel];

				if (DateTime.Today == calendarDayModel.Date)
				{
					calendarDayModel.SelectionColor = Theme.Current.TodaySelectedTodayDateColor;
				}
				else
				{
					if (selected)
					{
						calendarDayModel.SelectionColor = Theme.Current.TodaySelectedAnotherDateColor;
					}
					else
					{
						calendarDayModel.SelectionColor = Theme.Current.TodayNotSelectedDateColor;
					}
				}

				calendarDayModel.Selected = selected;

				if (selected)
				{
					calendarDayModel.TextColor = Theme.Current.TodaySelectedDateTextColor;
				}
				else
				{
					calendarDayModel.TextColor = Theme.Current.TodayNotSelectedDateTextColor;
				}

				var temp = CalendarViewModel.Clone(CalendarList[indexCalendarModel], this);
				temp.Days[indexCalendarDayModel] = calendarDayModel;

				CalendarList[indexCalendarModel] = temp;

			}
			catch (Exception ex)
			{
				AppLogs.Log(ex);
			}
		}

		protected void calendarPositionChangedEvent(CarouselView.FormsPlugin.Abstractions.PositionSelectedEventArgs e)
		{
			if (_isCalendarPositionChangeInProgress)
			{
				return;
			}

			try
			{
				if (CalendarList == null || CalendarList.Count <= _maximumCalendarPosition)
				{
					return;
				}

				_isCalendarPositionChangeInProgress = true;

				selectTodayDateWithoutSelectedFlag();
				deselectAllCalendarDays();

				if (_isManualSelectedCalendarDay)
				{
					selectCalendarDay(_manualSelectedCalendarDay);
				}
				else
				{
					selectCalendarDay(DateTime.Today);
				}

				switch (CalendarPosition)
				{
					case _minimumCalendarPosition:
						CalendarPosition = getCalendarPosition(_minimumCalendarPosition, WeekEnum.Previous);
						break;
					case _maximumCalendarPosition:
						CalendarPosition = getCalendarPosition(_maximumCalendarPosition, WeekEnum.Next);
						break;
				}
			}
			catch (Exception ex)
			{
				AppLogs.Log(ex);
			}
			finally
			{
				_isCalendarPositionChangeInProgress = false;
			}
		}
		int getCalendarPosition(int boundaryPosition, WeekEnum week)
		{
			int removePosition;
			int calculatedPosition;

			if (boundaryPosition == _maximumCalendarPosition)
			{
				removePosition = _minimumCalendarPosition;
				calculatedPosition = _maximumCalendarPosition - 1;
			}
			else
			{
				removePosition = _maximumCalendarPosition;
				calculatedPosition = _minimumCalendarPosition + 1;
			}

			var date = CalendarList[boundaryPosition].Date;
			var weekViewModel = getCalendarViewModel(date, week);

			var updatedCalendarList = CalendarList.ToList();
			updatedCalendarList.RemoveAt(removePosition);

			if (boundaryPosition == _maximumCalendarPosition)
			{
				updatedCalendarList.Add(weekViewModel);
			}
			else
			{
				updatedCalendarList.Insert(_minimumCalendarPosition, weekViewModel);
			}

			CalendarList = new ObservableCollection<CalendarViewModel>(updatedCalendarList);

			return calculatedPosition;
		}

		public void ExecuteCalendarSelectionChangedEvent(DateTime date)
		{
			try
			{
				selectTodayDateWithoutSelectedFlag();
				deselectAllCalendarDays();
				_manualSelectedCalendarDay = date;
				_isManualSelectedCalendarDay = true;
				selectCalendarDay(date);

				_services.Device.MainThread(async () => {
					await setNewSubjectList(date);
				});

			}
			catch (Exception ex)
			{
				AppLogs.Log(ex);
			}
		}

		async Task setNewSubjectList(DateTime dateTime)
		{
			try
			{
				var selectedDate = dateTime.Date;

				var scheduleItemsTask = getScheduleItemsForDate(selectedDate);
				var consultationItemsTask = getConsultationItemsForDate(selectedDate);
				await Task.WhenAll(scheduleItemsTask, consultationItemsTask);

				var scheduleItems = scheduleItemsTask.Result;
				var consultationItems = consultationItemsTask.Result;

				var mergedItems = scheduleItems
					.Concat(consultationItems)
					.OrderBy(item => getStartTimeSortKey(item.Start))
					.ThenBy(item => item.Name)
					.Select(item => new SubjectPageModel(item))
					.ToList();

				NewsSubjectList = mergedItems;
				setupNewsSubjectsHeight();
			}
			catch (Exception ex)
			{
				AppLogs.Log(ex);
				NewsSubjectList = new List<SubjectPageModel>();
				setupNewsSubjectsHeight();
			}
		}

		async Task<List<Schedule>> getScheduleItemsForDate(DateTime selectedDate)
		{
			var weekStartDate = DateHelper.GetWeekStartDate(selectedDate, WeekEnum.Current);
			var weekEndDate = weekStartDate.AddDays(6);

			var dateStart = weekStartDate.ToString(_scheduleQueryDateFormat, CultureInfo.InvariantCulture);
			var dateEnd = weekEndDate.ToString(_scheduleQueryDateFormat, CultureInfo.InvariantCulture);

			var subjects = await DataAccess.GetSchedule(dateStart, dateEnd);
			var schedule = subjects?.Schedule ?? new List<Schedule>();

			return schedule
				.Where(item => isDateForSelectedDay(item.Date, selectedDate))
				.ToList();
		}

		async Task<List<Schedule>> getConsultationItemsForDate(DateTime selectedDate)
		{
			var courseConsultationsTask =
				DataAccess.GetCourseProjectConsultation(_consultationsCount, _consultationsPage);
			var diplomaConsultationsTask =
				DataAccess.GetDiplomProjectConsultation(_consultationsCount, _consultationsPage);

			var courseConsultations = await courseConsultationsTask;
			var diplomaConsultations = await diplomaConsultationsTask;

			var result = mapCourseProjectConsultations(courseConsultations, selectedDate);

			var lecturerNames = await getLecturerNames(
				diplomaConsultations?.DiplomProjectConsultationDates?
					.Select(item => item.LecturerId));

			result.AddRange(mapDiplomProjectConsultations(
				diplomaConsultations, lecturerNames, selectedDate));

			return result;
		}

		List<Schedule> mapCourseProjectConsultations(
			CourseProjectConsultationModel consultations, DateTime selectedDate)
		{
			var result = new List<Schedule>();
			foreach (var consultation in consultations?.Consultations
				?? new List<CourseProjectConsultationDetailsModel>())
			{
				if (!tryParseDate(consultation.Day, out var consultationDate) ||
					consultationDate.Date != selectedDate.Date)
				{
					continue;
				}

				var teacher = consultation.Teacher ?? new Teacher();
				teacher.FullName = teacher.FullName ?? string.Empty;

				result.Add(new Schedule
				{
					Id = consultation.Id,
					Date = consultationDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
					Name = consultation.Subject?.Name,
					Color = consultation.Subject?.Color,
					Start = normalizeTime(consultation.StartTime),
					End = normalizeTime(consultation.EndTime),
					Building = consultation.Building,
					Audience = consultation.Audience,
					Teacher = teacher,
					Type = 3
				});
			}

			return result;
		}

		List<Schedule> mapDiplomProjectConsultations(
			DiplomProjectConsultationModel consultations,
			IDictionary<int, string> lecturerNames,
			DateTime selectedDate)
		{
			var result = new List<Schedule>();
			var diplomProjectTitle = CrossLocalization.Translate("today_diplom_projecting");
			foreach (var consultation in consultations?.DiplomProjectConsultationDates
				?? new List<DiplomProjectConsultationDateModel>())
			{
				if (!tryParseDate(consultation.Day, out var consultationDate) ||
					consultationDate.Date != selectedDate.Date)
				{
					continue;
				}

				lecturerNames.TryGetValue(consultation.LecturerId, out var lecturerName);

				result.Add(new Schedule
				{
					Id = consultation.Id,
					Date = consultationDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
					Name = diplomProjectTitle,
					Start = normalizeTime(consultation.StartTime),
					End = normalizeTime(consultation.EndTime),
					Building = consultation.Building,
					Audience = consultation.Audience,
					Teacher = new Teacher
					{
						LectorId = consultation.LecturerId,
						FullName = lecturerName ?? string.Empty
					},
					Type = 4
				});
			}

			return result;
		}

		async Task<Dictionary<int, string>> getLecturerNames(IEnumerable<int> lecturerIds)
		{
			var ids = (lecturerIds ?? Enumerable.Empty<int>())
				.Where(id => id > 0)
				.Distinct()
				.ToList();

			if (ids.Count == 0)
			{
				return new Dictionary<int, string>();
			}

			var missingIds = ids
				.Where(id => !_lecturerNamesCache.ContainsKey(id))
				.ToList();

			var profileTasks = missingIds.Select(async id => new
			{
				Id = id,
				Profile = await DataAccess.GetProfileInfoById(id)
			});

			var profiles = await Task.WhenAll(profileTasks);
			foreach (var profile in profiles)
			{
				if (!string.IsNullOrWhiteSpace(profile.Profile?.Name))
				{
					_lecturerNamesCache[profile.Id] = profile.Profile.Name;
				}
			}

			return ids
				.Where(id => _lecturerNamesCache.ContainsKey(id))
				.ToDictionary(id => id, id => _lecturerNamesCache[id]);
		}

		bool isDateForSelectedDay(string rawDate, DateTime selectedDate)
		{
			if (string.IsNullOrWhiteSpace(rawDate))
			{
				return true;
			}

			return tryParseDate(rawDate, out var parsedDate) &&
				parsedDate.Date == selectedDate.Date;
		}

		bool tryParseDate(string rawDate, out DateTime parsedDate)
		{
			if (DateTime.TryParseExact(
				rawDate,
				_scheduleDateFormats,
				CultureInfo.InvariantCulture,
				DateTimeStyles.AllowWhiteSpaces,
				out parsedDate))
			{
				return true;
			}

			if (DateTime.TryParse(
				rawDate,
				CultureInfo.InvariantCulture,
				DateTimeStyles.AllowWhiteSpaces,
				out parsedDate))
			{
				return true;
			}

			return DateTime.TryParse(rawDate, out parsedDate);
		}

		string normalizeTime(string rawTime)
		{
			if (string.IsNullOrWhiteSpace(rawTime))
			{
				return string.Empty;
			}

			if (DateTime.TryParseExact(
				rawTime,
				_timeFormats,
				CultureInfo.InvariantCulture,
				DateTimeStyles.None,
				out var parsedDate))
			{
				return parsedDate.ToString("HH:mm", CultureInfo.InvariantCulture);
			}

			if (TimeSpan.TryParse(rawTime, CultureInfo.InvariantCulture, out var time))
			{
				return $"{time.Hours:00}:{time.Minutes:00}";
			}

			return rawTime;
		}

		int getStartTimeSortKey(string startTime)
		{
			var normalizedStartTime = normalizeTime(startTime);
			if (DateTime.TryParseExact(
				normalizedStartTime,
				"HH:mm",
				CultureInfo.InvariantCulture,
				DateTimeStyles.None,
				out var parsedDate))
			{
				return parsedDate.Hour * 60 + parsedDate.Minute;
			}

			return int.MaxValue;
		}


		void setFilteredSubjectsList()
		{
			var filteredList = new List<CalendarSubjectsModel>();

			if (_isManualSelectedCalendarDay)
			{
				filteredList = _calendarSubjectsBackup.Where(
					x => x.Date.ToShortDateString() == _manualSelectedCalendarDay.ToShortDateString()).ToList();
			}
			else
			{
				filteredList = _calendarSubjectsBackup.Where(
					x => x.Date.ToShortDateString() == DateTime.Today.ToShortDateString()).ToList();
			}

			CalendarSubjects = new List<CalendarSubjectsModel>(filteredList);
			setupSubjectsHeight();
		}

		void setupSubjectsHeight()
		{
			try
			{
				if (CalendarSubjects.Count == 0)
				{
					CalendarSubjectsHeight = _emptySubjectsHeight;
					return;
				}

				CalendarSubjectsHeight =
					_subjectHeight * CalendarSubjects.Count;
			}
			catch (Exception ex)
			{
				AppLogs.Log(ex);
			}
		}

		void setupNewsSubjectsHeight()
		{
			try
			{
				if (NewsSubjectList.Count == 0)
				{
					CalendarSubjectsHeight = _isLargeFont ? _emptySubjectsHeightLarge : _emptySubjectsHeight;
					return;
				}

				CalendarSubjectsHeight =
					_subjectHeight * NewsSubjectList.Count - _subjectsHeightToSubtract * (NewsSubjectList.Count - 1);
			}
			catch (Exception ex)
			{
				AppLogs.Log(ex);
			}
		}
	}
}
