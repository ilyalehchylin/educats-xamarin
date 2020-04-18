using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduCATS.Data;
using EduCATS.Data.Models;
using EduCATS.Data.User;
using EduCATS.Helpers.Extensions;
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

		List<StatsStudentModel> _students;

		public StatsPageViewModel(IPlatformServices services) : base(services)
		{
			setPagesList();
			setCollapsedDetails();

			IsStudent = AppUserData.UserType == UserTypeEnum.Student;

			Task.Run(async () => {
				IsLoading = true;
				await SetupSubjects();
				await getAndSetStatistics();
				IsLoading = false;
			});

			SubjectChanged += async (id, name) => {
				PlatformServices.Dialogs.ShowLoading();
				await getAndSetStatistics();
				PlatformServices.Dialogs.HideLoading();
			};
		}

		bool _isLoading;
		public bool IsLoading {
			get { return _isLoading; }
			set { SetProperty(ref _isLoading, value); }
		}

		bool _isExpandedStatistics;
		public bool IsExpandedStatistics {
			get { return _isExpandedStatistics; }
			set { SetProperty(ref _isExpandedStatistics, value); }
		}

		bool _isCollapsedStatistics;
		public bool IsCollapsedStatistics {
			get { return _isCollapsedStatistics; }
			set { SetProperty(ref _isCollapsedStatistics, value); }
		}

		bool _isNotEnoughDetails;
		public bool IsNotEnoughDetails {
			get { return _isNotEnoughDetails; }
			set { SetProperty(ref _isNotEnoughDetails, value); }
		}

		bool _isEnoughDetails;
		public bool IsEnoughDetails {
			get { return _isEnoughDetails; }
			set { SetProperty(ref _isEnoughDetails, value); }
		}

		bool _isStudent;
		public bool IsStudent {
			get { return _isStudent; }
			set { SetProperty(ref _isStudent, value); }
		}

		string _averageLabs;
		public string AverageLabs {
			get { return _averageLabs; }
			set { SetProperty(ref _averageLabs, value); }
		}

		string _averageTests;
		public string AverageTests {
			get { return _averageTests; }
			set { SetProperty(ref _averageTests, value); }
		}

		string _rating;
		public string Rating {
			get { return _rating; }
			set { SetProperty(ref _rating, value); }
		}

		List<double> _chartEntries;
		public List<double> ChartEntries {
			get { return _chartEntries; }
			set { SetProperty(ref _chartEntries, value); }
		}

		List<StatsPageModel> _pagesList;
		public List<StatsPageModel> PagesList {
			get { return _pagesList; }
			set { SetProperty(ref _pagesList, value); }
		}

		object _selectedItem;
		public object SelectedItem {
			get { return _selectedItem; }
			set {
				SetProperty(ref _selectedItem, value);

				if (_selectedItem != null) {
					openPage(_selectedItem);
				}
			}
		}

		Command _refreshCommand;
		public Command RefreshCommand {
			get {
				return _refreshCommand ?? (_refreshCommand = new Command(
					async () => await executeRefreshCommand()));
			}
		}

		Command _expandCommand;
		public Command ExpandCommand {
			get {
				return _expandCommand ?? (_expandCommand = new Command(executeExpandCommand));
			}
		}

		protected async Task executeRefreshCommand()
		{
			try {
				IsLoading = true;
				await SetupSubjects();
				await getAndSetStatistics();
				IsLoading = false;
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		protected void executeExpandCommand()
		{
			try {
				if (IsCollapsedStatistics) {
					setCollapsedDetails(false);
				} else {
					setCollapsedDetails();
				}
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		async Task getAndSetStatistics()
		{
			try {
				var studentsStatistics = await getStatistics();

				if (studentsStatistics == null) {
					setChartData(null);
				} else {
					var currentStudentStatistics = studentsStatistics.SingleOrDefault(
						s => s.StudentId == PlatformServices.Preferences.UserId);
					setChartData(currentStudentStatistics);
					_students = studentsStatistics;
				}
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		void setChartData(StatsStudentModel stats)
		{
			try {
				if (stats == null) {
					stats = new StatsStudentModel();
				}

				var avgLabs = stats.AverageLabsMark.StringToDouble();
				AverageLabs = avgLabs.ToString(_doubleStringFormat);

				var avgTests = stats.AverageTestMark.StringToDouble();
				AverageTests = avgTests.ToString(_doubleStringFormat);

				var rating = (avgLabs + avgTests) / 2;
				Rating = rating.ToString(_doubleStringFormat);

				setNotEnoughDetails(avgLabs == 0 && avgTests == 0 && rating == 0);

				ChartEntries = new List<double> {
					avgLabs, avgTests, rating
				};
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		async Task<List<StatsStudentModel>> getStatistics()
		{
			try {
				if (!AppUserData.IsProfileLoaded) {
					await getProfile();
				}

				var groupId = PlatformServices.Preferences.GroupId;

				if (CurrentSubject == null || groupId == -1) {
					return null;
				}

				var statisticsModel = await DataAccess.GetStatistics(
					CurrentSubject.Id, PlatformServices.Preferences.GroupId);

				if (DataAccess.IsError && !DataAccess.IsConnectionError) {
					PlatformServices.Dialogs.ShowError(DataAccess.ErrorMessage);
				}

				return statisticsModel.Students?.ToList();
			} catch (Exception ex) {
				AppLogs.Log(ex);
				return null;
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

		void setPagesList()
		{
			PagesList = new List<StatsPageModel> {
				getPage("stats_page_labs_rating"),
				getPage("stats_page_labs_visiting"),
				getPage("stats_page_lectures_visiting")
			};
		}

		StatsPageModel getPage(string text)
		{
			return new StatsPageModel {
				Title = CrossLocalization.Translate(text)
			};
		}

		void setCollapsedDetails(bool isCollapsed = true)
		{
			IsCollapsedStatistics = isCollapsed;
			IsExpandedStatistics = !isCollapsed;
		}

		void setNotEnoughDetails(bool isNotEnough = true)
		{
			IsEnoughDetails = !isNotEnough;
			IsNotEnoughDetails = isNotEnough;
		}

		void openPage(object selectedObject)
		{
			try {
				if (selectedObject == null || selectedObject.GetType() != typeof(StatsPageModel)) {
					return;
				}

				var page = selectedObject as StatsPageModel;
				var pageType = getPageToOpen(page.Title);

				if (AppUserData.UserType == UserTypeEnum.Professor) {
					PlatformServices.Navigation.OpenStudentsListStats(
						(int)pageType, CurrentSubject.Id, _students, page.Title);
					return;
				}

				var user = _students.SingleOrDefault(s => s.StudentId == PlatformServices.Preferences.UserId);

				if (user == null) {
					return;
				}

				PlatformServices.Navigation.OpenDetailedStatistics(
					user.Login, CurrentSubject.Id, PlatformServices.Preferences.GroupId, (int)pageType, page.Title);
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		StatsPageEnum getPageToOpen(string pageString)
		{
			var labsRatingString = CrossLocalization.Translate("stats_page_labs_rating");
			var labsVisitingString = CrossLocalization.Translate("stats_page_labs_visiting");

			if (pageString.Equals(labsRatingString)) {
				return StatsPageEnum.LabsRating;
			} else if (pageString.Equals(labsVisitingString)) {
				return StatsPageEnum.LabsVisiting;
			} else {
				return StatsPageEnum.LecturesVisiting;
			}
		}
	}
}
