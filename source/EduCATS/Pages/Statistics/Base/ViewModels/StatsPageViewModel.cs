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
		const int _labsModuleType = 3;
		const int _courseModuleType = 4;
		const int _testsModuleType = 8;
		const int _practicalsModuleType = 10;
		private List<StatsStudentModel> _students;
		private IPlatformServices _service;
		bool _hasModuleVisibilityRules;
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

		List<StatsChartEntryModel> _chartEntries;
		public List<StatsChartEntryModel> ChartEntries
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

		bool _isTests;
		public bool IsTests
		{
			get { return _isTests; }
			set { SetProperty(ref _isTests, value); }
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
				AppLogs.Log("StatsPage.Update skipped: update already in progress.");
				return;
			}

			_isUpdating = true;
			AppLogs.Log(
				$"StatsPage.Update started. showLoadingDialog={showLoadingDialog}, reloadSubjects={reloadSubjects}, " +
				$"currentSubjectId={CurrentSubject?.Id}, userType={AppUserData.UserType}");

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

				AppLogs.Log("StatsPage.Update step: checkStudent.");
				checkStudent();

				if (reloadSubjects)
				{
					AppLogs.Log("StatsPage.Update step: SetupSubjects start.");
					await SetupSubjects();
					AppLogs.Log($"StatsPage.Update step: SetupSubjects done. currentSubjectId={CurrentSubject?.Id}");
				}

				AppLogs.Log("StatsPage.Update step: setButtonsList start.");
				await setButtonsList();
				AppLogs.Log(
					$"StatsPage.Update step: setButtonsList done. isLabs={IsLabs}, isPract={IsPract}, " +
					$"isTests={IsTests}, isCourse={IsCourse}, hasModuleRules={_hasModuleVisibilityRules}");

				AppLogs.Log("StatsPage.Update step: getAndSetStatistics start.");
				await getAndSetStatistics();
				AppLogs.Log("StatsPage.Update step: getAndSetStatistics done.");
			}
			catch (Exception ex)
			{
				AppLogs.Log($"StatsPage.Update exception: {ex.Message}");
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
				AppLogs.Log("StatsPage.Update finished.");
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
			IsPract = IsLabs = IsTests = IsCourse = false;
			_hasModuleVisibilityRules = false;
			AppLogs.Log($"StatsPage.setButtonsList started. subjectId={CurrentSubject?.Id}");

			if (CurrentSubject == null)
			{
				AppLogs.Log("StatsPage.setButtonsList: current subject is null.");
				setPagesList();
				return;
			}

			var modules = await DataAccess.GetSubjectModules(CurrentSubject.Id);
			if (modules?.Count > 0)
			{
				var enabledModuleTypes = new HashSet<int>(
					modules.Where(m => m != null && m.Checked).Select(m => m.Type));

				IsPract = enabledModuleTypes.Contains(_practicalsModuleType);
				IsLabs = enabledModuleTypes.Contains(_labsModuleType);
				IsTests = enabledModuleTypes.Contains(_testsModuleType);
				IsCourse = enabledModuleTypes.Contains(_courseModuleType);
				_hasModuleVisibilityRules = true;
				AppLogs.Log(
					$"StatsPage.setButtonsList: modules applied. modulesCount={modules.Count}, " +
					$"isLabs={IsLabs}, isPract={IsPract}, isTests={IsTests}, isCourse={IsCourse}");
				setPagesList();
				return;
			}

			// Fallback for environments where modules are unavailable.
			AppLogs.Log(
				$"StatsPage.setButtonsList: modules unavailable. isError={DataAccess.IsError}, " +
				$"isConnectionError={DataAccess.IsConnectionError}, errorMessage={DataAccess.ErrorMessage}");
			IsTests = true;

			var dataPract = await DataAccess.GetPracticals(CurrentSubject.Id);
			if (dataPract?.Practicals?.Count != 0)
			{
				IsPract = true;
			}

			var dataLabs = await DataAccess.GetLabs(CurrentSubject.Id);
			if (dataLabs?.Labs?.Count != 0)
			{
				IsLabs = true;
			}

			AppLogs.Log(
				$"StatsPage.setButtonsList: fallback flags. isLabs={IsLabs}, isPract={IsPract}, " +
				$"isTests={IsTests}, isCourse={IsCourse}");
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
				AppLogs.Log(
					$"StatsPage.getStatistics started. subjectId={CurrentSubject?.Id}, groupId={groupId}, " +
					$"userType={AppUserData.UserType}");

				if (CurrentSubject == null || groupId == -1)
				{
					AppLogs.Log("StatsPage.getStatistics skipped: subject is null or groupId == -1.");
					return null;
				}

				var statisticsModel = await DataAccess.GetStudentsStatistics(
					CurrentSubject.Id, PlatformServices.Preferences.GroupId);

				if (DataAccess.IsError)
				{
					AppLogs.Log(
						$"StatsPage.getStatistics data error. isConnectionError={DataAccess.IsConnectionError}, " +
						$"message={DataAccess.ErrorMessage}");
				}

				if (DataAccess.IsError && !DataAccess.IsConnectionError)
				{
					PlatformServices.Dialogs.ShowError(DataAccess.ErrorMessage);
				}

				var result = statisticsModel?.Students?.ToList();
				AppLogs.Log($"StatsPage.getStatistics finished. studentsCount={result?.Count ?? 0}");
				return result;
			}
			catch (Exception ex)
			{
				AppLogs.Log($"StatsPage.getStatistics exception: {ex.Message}");
				AppLogs.Log(ex);
				return null;
			}
		}

		protected virtual async Task getAndSetStatistics()
		{
			try
			{
				AppLogs.Log($"StatsPage.getAndSetStatistics started. currentSubjectId={CurrentSubject?.Id}");
				if (CurrentSubject == null)
				{
					resetChartData();
					AppLogs.Log("StatsPage.getAndSetStatistics: reset chart due to null subject.");
					return;
				}

				var studentsStatistics = await getStatistics();
				_students = studentsStatistics;
				AppLogs.Log($"StatsPage.getAndSetStatistics: students loaded. count={_students?.Count ?? 0}");

				if (_isStudent)
				{
					AppLogs.Log("StatsPage.getAndSetStatistics: setting student chart data.");
					await setStudentChartData();
				}
				else
				{
					AppLogs.Log("StatsPage.getAndSetStatistics: setting teacher chart data.");
					await setTeacherChartData();
				}
			}
			catch (Exception ex)
			{
				AppLogs.Log($"StatsPage.getAndSetStatistics exception: {ex.Message}");
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
			if (DataAccess.IsError)
			{
				AppLogs.Log(
					$"StatsPage.setStudentChartData summary error. isConnectionError={DataAccess.IsConnectionError}, " +
					$"message={DataAccess.ErrorMessage}");
				setStudentChartDataFromMarks();
				return;
			}

			var studentSummary =
				summary?.Students?.FirstOrDefault(s => s.StudentId == PlatformServices.Preferences.UserId) ??
				summary?.Students?.FirstOrDefault();

			if (studentSummary == null)
			{
				setStudentChartDataFromMarks();
				return;
			}

			var subjectId = CurrentSubject.Id;
			var averageLabs = getSubjectMetricById(studentSummary.UserAvgLabMarks, subjectId);
			var averageTests = getSubjectMetricById(studentSummary.UserAvgTestMarks, subjectId);
			var averagePract = getSubjectMetricById(studentSummary.UserAvgPracticalMarks, subjectId);
			var averageCourse = getSubjectMetricById(studentSummary.UserAvgCourseMark, subjectId);
			var courseCount = getSubjectMetricById(studentSummary.UserCourseCount, subjectId);

			if (!_hasModuleVisibilityRules)
			{
				IsPract = IsPract || getSubjectMetricById(studentSummary.UserPracticalCount, subjectId) > 0 || averagePract > 0;
				IsCourse = courseCount > 0 ||
					(hasSubjectMetric(studentSummary.UserAvgCourseMark, subjectId) && averageCourse > 0);
			}

			setChartData(averageLabs, averageTests, averagePract, averageCourse, includeCourseInRating: true);
		}

		async Task setTeacherChartData()
		{
			var summary = await DataAccess.GetTeacherStatisticsSummary();
			if (DataAccess.IsError)
			{
				AppLogs.Log(
					$"StatsPage.setTeacherChartData summary error. isConnectionError={DataAccess.IsConnectionError}, " +
					$"message={DataAccess.ErrorMessage}");
				setTeacherChartDataFromMarks();
				return;
			}

			var subjectSummary = summary?.SubjectStatistics?.FirstOrDefault(s => s.SubjectId == CurrentSubject.Id);
			if (subjectSummary == null)
			{
				setTeacherChartDataFromMarks();
				return;
			}

			if (!_hasModuleVisibilityRules)
			{
				IsPract = IsPract || subjectSummary.AveragePracticalsMark > 0;
				IsCourse = subjectSummary.AverageCourseProjectMark > 0;
			}

			setChartData(
				subjectSummary.AverageLabsMark,
				subjectSummary.AverageTestsMark,
				subjectSummary.AveragePracticalsMark,
				subjectSummary.AverageCourseProjectMark,
				includeCourseInRating: true);
		}

		void setStudentChartDataFromMarks()
		{
			var student = _students?.FirstOrDefault(s => s.StudentId == PlatformServices.Preferences.UserId) ??
				_students?.FirstOrDefault();

			if (student == null)
			{
				resetChartData();
				return;
			}

			setChartData(
				parseChartMetric(student.AverageLabsMark),
				parseChartMetric(student.AverageTestMark),
				_defaultChartValue,
				_defaultChartValue,
				includeCourseInRating: false);
		}

		void setTeacherChartDataFromMarks()
		{
			var students = _students;
			if (students == null || students.Count == 0)
			{
				resetChartData();
				return;
			}

			setChartData(
				getAverageMetric(students.Select(s => s.AverageLabsMark)),
				getAverageMetric(students.Select(s => s.AverageTestMark)),
				_defaultChartValue,
				_defaultChartValue,
				includeCourseInRating: false);
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

				var chartEntries = new List<StatsChartEntryModel>();

				if (IsPract)
				{
					chartEntries.Add(new StatsChartEntryModel(StatsChartMetricType.Pract, averagePract));
				}

				if (IsLabs)
				{
					chartEntries.Add(new StatsChartEntryModel(StatsChartMetricType.Labs, averageLabs));
				}

				if (IsTests)
				{
					chartEntries.Add(new StatsChartEntryModel(StatsChartMetricType.Tests, averageTests));
				}

				if (IsCourse)
				{
					chartEntries.Add(new StatsChartEntryModel(StatsChartMetricType.Course, averageCourse));
				}

				chartEntries.Add(new StatsChartEntryModel(StatsChartMetricType.Rating, rating));
				ChartEntries = chartEntries;

				setNotEnoughDetails(ChartEntries == null || ChartEntries.Count == 0);
			}
			catch (Exception ex)
			{
				AppLogs.Log(ex);
			}
		}

		void resetChartData()
		{
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

			if (IsTests)
			{
				values.Add(averageTests);
			}

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

		static double parseChartMetric(string value) =>
			tryParseChartMetric(value) ?? _defaultChartValue;

		static double getAverageMetric(IEnumerable<string> values)
		{
			var parsedValues = values?
				.Select(tryParseChartMetric)
				.Where(v => v.HasValue)
				.Select(v => v.Value)
				.ToList();

			if (parsedValues == null || parsedValues.Count == 0)
			{
				return _defaultChartValue;
			}

			return parsedValues.Average();
		}

		static double? tryParseChartMetric(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return null;
			}

			if (double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsed))
			{
				return parsed;
			}

			// Some backend values use comma as decimal separator.
			if (double.TryParse(value.Replace(',', '.'),
				NumberStyles.Float, CultureInfo.InvariantCulture, out parsed))
			{
				return parsed;
			}

			return double.TryParse(value, NumberStyles.Float, CultureInfo.CurrentCulture, out parsed) ?
				parsed :
				(double?)null;
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
