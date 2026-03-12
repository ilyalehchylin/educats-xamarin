using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using EduCATS.Data;
using EduCATS.Data.Models;
using EduCATS.Data.User;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Logs;
using EduCATS.Pages.Pickers;
using EduCATS.Pages.Statistics.Base.Models;
using EduCATS.Pages.Statistics.Enums;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS.Pages.Statistics.Base.ViewModels
{
	public class StatsPageViewModel : SubjectsViewModel
	{
		const string _doubleStringFormat = "0.0";
		const double _defaultChartValue = 0;
		private List<StatsStudentModel> _students;
		private IPlatformServices _service;
		bool _isUpdating;

		public StatsPageViewModel(IPlatformServices services) : base(services)
		{
			_service = services;
		}

		public void Init()
		{
			setCollapsedDetails();

			_service.Device.MainThread(async () => await Update(true));

			SubjectChanged += async (id, name) =>
			{
				await Update(true, false);
			};

		}

		bool _isLoading;
		public bool IsLoading
		{
			get { return _isLoading; }
			set { SetProperty(ref _isLoading, value); }
		}

		bool _isExpandedStatistics;
		public bool IsExpandedStatistics
		{
			get { return _isExpandedStatistics; }
			set { SetProperty(ref _isExpandedStatistics, value); }
		}

		bool _isCollapsedStatistics;
		public bool IsCollapsedStatistics
		{
			get { return _isCollapsedStatistics; }
			set { SetProperty(ref _isCollapsedStatistics, value); }
		}

		bool _isNotEnoughDetails;
		public bool IsNotEnoughDetails
		{
			get { return _isNotEnoughDetails; }
			set { SetProperty(ref _isNotEnoughDetails, value); }
		}

		bool _isEnoughDetails;
		public bool IsEnoughDetails
		{
			get { return _isEnoughDetails; }
			set { SetProperty(ref _isEnoughDetails, value); }
		}

		bool _isStudent;
		public bool IsStudent
		{
			get { return _isStudent; }
			set { SetProperty(ref _isStudent, value); }
		}

		string _averageLabs;
		public string AverageLabs
		{
			get { return _averageLabs; }
			set { SetProperty(ref _averageLabs, value); }
		}

		string _averagePract;
		public string AveragePract
		{
			get { return _averagePract; }
			set { SetProperty(ref _averagePract, value); }
		}

		string _averageTests;
		public string AverageTests
		{
			get { return _averageTests; }
			set { SetProperty(ref _averageTests, value); }
		}

		string _averageCourse;
		public string AverageCourse
		{
			get { return _averageCourse; }
			set { SetProperty(ref _averageCourse, value); }
		}

		string _rating;
		public string Rating
		{
			get { return _rating; }
			set { SetProperty(ref _rating, value); }
		}

		List<double> _chartEntries;
		public List<double> ChartEntries
		{
			get { return _chartEntries; }
			set { SetProperty(ref _chartEntries, value); }
		}

		List<StatsPageModel> _pagesList;
		public List<StatsPageModel> PagesList
		{
			get { return _pagesList; }
			set { SetProperty(ref _pagesList, value); }
		}

		object _selectedItem;
		public object SelectedItem
		{
			get { return _selectedItem; }
			set
			{
				SetProperty(ref _selectedItem, value);

				if (_selectedItem != null)
				{
					openPage(_selectedItem);
				}
			}
		}

		bool _isLabs;
		public bool IsLabs
		{
			get { return _isLabs; }
			set
			{
				SetProperty(ref _isLabs, value);
			}
		}

		bool _isPract;
		public bool IsPract
		{
			get { return _isPract; }
			set
			{
				SetProperty(ref _isPract, value);
			}
		}

		bool _isCourse;
		public bool IsCourse
		{
			get { return _isCourse; }
			set { SetProperty(ref _isCourse, value); }
		}

		Command _refreshCommand;
		public Command RefreshCommand
		{
			get
			{
				return _refreshCommand ?? (_refreshCommand = new Command(
					async () => await executeRefreshCommand()));
			}
		}

		public async Task Update(bool showLoadingDialog, bool reloadSubjects = true)
		{
			if (_isUpdating)
			{
				return;
			}

			_isUpdating = true;

			try
			{
				if (showLoadingDialog)
				{
					PlatformServices.Device.MainThread(() => PlatformServices.Dialogs.ShowLoading());
				}
				else
				{
					PlatformServices.Device.MainThread(() => IsLoading = true);
				}

				checkStudent();

				if (reloadSubjects)
				{
					await SetupSubjects();
				}

				await setButtonsList();
				await getAndSetStatistics();
			}
			catch (Exception ex)
			{
				AppLogs.Log(ex);
			}
			finally
			{
				if (showLoadingDialog)
				{
					PlatformServices.Device.MainThread(() => PlatformServices.Dialogs.HideLoading());
				}

				PlatformServices.Device.MainThread(() => IsLoading = false);
				_isUpdating = false;
			}
		}

		Command _expandCommand;
		public Command ExpandCommand
		{
			get
			{
				return _expandCommand ?? (_expandCommand = new Command(executeExpandCommand));
			}
		}

		public async Task setButtonsList()
		{
			IsPract = IsLabs = IsCourse = false;

			if (CurrentSubject == null)
			{
				setPagesList();
				return;
			}

			var dataPract = await DataAccess.GetPracticals(CurrentSubject.Id);
			if (dataPract.Practicals.Count != 0)
			{
				IsPract = true;
			}

			var dataLabs = await DataAccess.GetLabs(CurrentSubject.Id);
			if (dataLabs.Labs.Count != 0)
			{
				IsLabs = true;
			}

			setPagesList();
		}

		public void setPagesList()
		{
			if (_isPract && _isLabs)
			{
				PagesList = new List<StatsPageModel> {
				getPage("stats_page_lectures_visiting"),
				getPage("practiсe_visiting"),
				getPage("practice_mark"),
				getPage("stats_page_labs_visiting"),
				getPage("stats_page_labs_rating"),
				};
			}
			else if (_isPract)
			{
				PagesList = new List<StatsPageModel> {
				getPage("stats_page_lectures_visiting"),
				getPage("practiсe_visiting"),
				getPage("practice_mark"),
				};
			}
			else if (_isLabs)
			{
				PagesList = new List<StatsPageModel> {
				getPage("stats_page_lectures_visiting"),
				getPage("stats_page_labs_visiting"),
				getPage("stats_page_labs_rating"),
				};
			}
			else
			{
				PagesList = new List<StatsPageModel> {
				getPage("stats_page_lectures_visiting") };
			}
		}

		public void setCollapsedDetails(bool isCollapsed = true)
		{
			IsCollapsedStatistics = isCollapsed;
			IsExpandedStatistics = !isCollapsed;
		}

		protected StatsPageEnum getPageToOpen(string pageString)
		{
			var labsRatingString = CrossLocalization.Translate("stats_page_labs_rating");
			var labsVisitingString = CrossLocalization.Translate("stats_page_labs_visiting");
			var practiseVisitingString = CrossLocalization.Translate("practiсe_visiting");
			var practiseRatingString = CrossLocalization.Translate("practice_mark");

			if (pageString.Equals(labsRatingString))
			{
				return StatsPageEnum.LabsRating;
			}
			else if (pageString.Equals(labsVisitingString))
			{
				return StatsPageEnum.LabsVisiting;
			}
			else if (pageString.Equals(practiseVisitingString))
			{
				return StatsPageEnum.PractiseVisiting;
			}
			else if (pageString.Equals(practiseRatingString))
			{
				return StatsPageEnum.PractiseMarks;
			}
			else
			{
				return StatsPageEnum.LecturesVisiting;
			}
		}


		protected virtual async Task executeRefreshCommand()
		{
			await Update(false);
		}

		protected virtual void openPage(object selectedObject)
		{
			try
			{
				if (selectedObject == null || selectedObject.GetType() != typeof(StatsPageModel))
				{
					return;
				}

				var page = selectedObject as StatsPageModel;
				var pageType = getPageToOpen(page.Title);

				if (AppUserData.UserType == UserTypeEnum.Professor)
				{
					PlatformServices.Navigation.OpenStudentsListStats(
						(int)pageType, CurrentSubject.Id, _students, page.Title);
					return;
				}
				else
				{
					if (_students == null)
					{
						return;
					}

					var user = _students.SingleOrDefault(s => s.StudentId == PlatformServices.Preferences.UserId);
					if (user == null)
					{
						return;
					}
					PlatformServices.Navigation.OpenDetailedStatistics(
					user.Login, CurrentSubject.Id, PlatformServices.Preferences.GroupId, (int)pageType, page.Title, user.Name);
				}

			}
			catch (Exception ex)
			{
				AppLogs.Log(ex);
			}
		}

		protected virtual async Task<List<StatsStudentModel>> getStatistics()
		{
			try
			{
				if (!AppUserData.IsProfileLoaded)
				{
					await getProfile();
				}

				var groupId = PlatformServices.Preferences.GroupId;

				if (CurrentSubject == null || groupId == -1)
				{
					return null;
				}

				var statisticsModel = await DataAccess.GetStudentsStatistics(
					CurrentSubject.Id, PlatformServices.Preferences.GroupId);

				if (DataAccess.IsError && !DataAccess.IsConnectionError)
				{
					PlatformServices.Dialogs.ShowError(DataAccess.ErrorMessage);
				}

				return statisticsModel.Students?.ToList();
			}
			catch (Exception ex)
			{
				AppLogs.Log(ex);
				return null;
			}
		}

		protected virtual async Task getAndSetStatistics()
		{
			try
			{
				if (CurrentSubject == null)
				{
					resetChartData();
					return;
				}

				var studentsStatistics = await getStatistics();
				_students = studentsStatistics;

				if (_isStudent)
				{
					await setStudentChartData();
				}
				else
				{
					await setTeacherChartData();
				}
			}
			catch (Exception ex)
			{
				AppLogs.Log(ex);
			}
		}


		void executeExpandCommand()
		{
			try
			{
				if (IsCollapsedStatistics)
				{
					setCollapsedDetails(false);
				}
				else
				{
					setCollapsedDetails();
				}
			}
			catch (Exception ex)
			{
				AppLogs.Log(ex);
			}
		}

		async Task setStudentChartData()
		{
			var summary = await DataAccess.GetStudentStatisticsSummary();
			showDataAccessErrorIfNeeded();

			var studentSummary =
				summary?.Students?.FirstOrDefault(s => s.StudentId == PlatformServices.Preferences.UserId) ??
				summary?.Students?.FirstOrDefault();

			if (studentSummary == null)
			{
				resetChartData();
				return;
			}

			var subjectId = CurrentSubject.Id;
			var averageLabs = getSubjectMetricById(studentSummary.UserAvgLabMarks, subjectId);
			var averageTests = getSubjectMetricById(studentSummary.UserAvgTestMarks, subjectId);
			var averagePract = getSubjectMetricById(studentSummary.UserAvgPracticalMarks, subjectId);
			var averageCourse = getSubjectMetricById(studentSummary.UserAvgCourseMark, subjectId);
			var courseCount = getSubjectMetricById(studentSummary.UserCourseCount, subjectId);

			IsPract = IsPract || getSubjectMetricById(studentSummary.UserPracticalCount, subjectId) > 0 || averagePract > 0;
			IsCourse = courseCount > 0 ||
				(hasSubjectMetric(studentSummary.UserAvgCourseMark, subjectId) && averageCourse > 0);

			setChartData(averageLabs, averageTests, averagePract, averageCourse, includeCourseInRating: true);
		}

		async Task setTeacherChartData()
		{
			var summary = await DataAccess.GetTeacherStatisticsSummary();
			showDataAccessErrorIfNeeded();

			var subjectSummary = summary?.SubjectStatistics?.FirstOrDefault(s => s.SubjectId == CurrentSubject.Id);
			if (subjectSummary == null)
			{
				resetChartData();
				return;
			}

			IsPract = IsPract || subjectSummary.AveragePracticalsMark > 0;
			IsCourse = subjectSummary.AverageCourseProjectMark > 0;

			setChartData(
				subjectSummary.AverageLabsMark,
				subjectSummary.AverageTestsMark,
				subjectSummary.AveragePracticalsMark,
				subjectSummary.AverageCourseProjectMark,
				includeCourseInRating: true);
		}

		void setChartData(
			double averageLabs,
			double averageTests,
			double averagePract,
			double averageCourse,
			bool includeCourseInRating)
		{
			try
			{
				AverageLabs = formatChartValue(averageLabs);
				AverageTests = formatChartValue(averageTests);
				AveragePract = formatChartValue(averagePract);
				AverageCourse = formatChartValue(averageCourse);

				var rating = calculateRating(averageLabs, averageTests, averagePract, averageCourse, includeCourseInRating);
				Rating = formatChartValue(rating);

				ChartEntries = new List<double> {
					averagePract,
					averageLabs,
					averageTests
				};

				if (IsCourse)
				{
					ChartEntries.Add(averageCourse);
				}

				ChartEntries.Add(rating);

				setNotEnoughDetails(ChartEntries.All(v => v == _defaultChartValue));
			}
			catch (Exception ex)
			{
				AppLogs.Log(ex);
			}
		}

		void resetChartData()
		{
			IsCourse = false;
			setChartData(
				_defaultChartValue,
				_defaultChartValue,
				_defaultChartValue,
				_defaultChartValue,
				includeCourseInRating: _isStudent);
		}

		double calculateRating(
			double averageLabs,
			double averageTests,
			double averagePract,
			double averageCourse,
			bool includeCourseInRating)
		{
			var values = new List<double>();

			if (IsLabs)
			{
				values.Add(averageLabs);
			}

			values.Add(averageTests);

			if (IsPract)
			{
				values.Add(averagePract);
			}

			if (includeCourseInRating && IsCourse)
			{
				values.Add(averageCourse);
			}

			if (values.Count == 0)
			{
				return _defaultChartValue;
			}

			return values.Average();
		}

		string formatChartValue(double value) =>
			value.ToString(_doubleStringFormat, CultureInfo.InvariantCulture);

		static double getSubjectMetricById(IList<StudentStatisticsSubjectValueModel> metrics, int subjectId) =>
			metrics?.FirstOrDefault(m => m.SubjectId == subjectId)?.Value ?? _defaultChartValue;

		static bool hasSubjectMetric(IList<StudentStatisticsSubjectValueModel> metrics, int subjectId) =>
			metrics?.Any(m => m.SubjectId == subjectId) == true;

		void showDataAccessErrorIfNeeded()
		{
			if (DataAccess.IsError && !DataAccess.IsConnectionError)
			{
				PlatformServices.Dialogs.ShowError(DataAccess.ErrorMessage);
			}
		}



		/// <summary>
		/// Get profile if <see cref="App.getProfileInfo" didn't have time to load./>
		/// </summary>
		/// <returns>Task.</returns>
		async Task getProfile()
		{
			var profile = await DataAccess.GetProfileInfo(PlatformServices.Preferences.UserLogin);
			AppUserData.SetProfileData(PlatformServices, profile);
			IsStudent = AppUserData.UserType == UserTypeEnum.Student;
		}


		StatsPageModel getPage(string text)
		{
			return new StatsPageModel
			{
				Title = CrossLocalization.Translate(text)
			};
		}


		void setNotEnoughDetails(bool isNotEnough = true)
		{
			IsEnoughDetails = !isNotEnough;
			IsNotEnoughDetails = isNotEnough;
		}

		void checkStudent()
		{
			IsStudent = AppUserData.UserType == UserTypeEnum.Student;
		}

	}
}
