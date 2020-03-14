using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduCATS.Constants;
using EduCATS.Data;
using EduCATS.Data.Models.Statistics;
using EduCATS.Data.User;
using EduCATS.Helpers.Converters;
using EduCATS.Helpers.Dialogs.Interfaces;
using EduCATS.Helpers.Pages.Interfaces;
using EduCATS.Helpers.Settings;
using EduCATS.Pages.Statistics.Base.Models;
using EduCATS.Pages.Statistics.Enums;
using EduCATS.Pages.Utils.ViewModels;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS.Pages.Statistics.Base.ViewModels
{
	public class StatsPageViewModel : SubjectsViewModel
	{
		readonly IPages _navigationService;

		List<StatisticsStudentModel> _students;

		public StatsPageViewModel(
			IDialogs dialogService, IPages navigationService) : base(dialogService)
		{
			_navigationService = navigationService;
			setPagesList();
			setCollapsedDetails();
			IsStudent = AppUserData.UserType == UserTypeEnum.Student;

			Task.Run(async () => {
				await SetupSubjects();
				await getAndSetStatistics();
			});

			SubjectChanged += async (id, name) => {
				await getAndSetStatistics();
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
			IsLoading = true;
			await getAndSetStatistics();
			IsLoading = false;
		}

		protected void executeExpandCommand()
		{
			if (IsCollapsedStatistics) {
				setCollapsedDetails(false);
			} else {
				setCollapsedDetails();
			}
		}

		async Task getAndSetStatistics()
		{
			var studentsStatistics = await getStatistics();

			if (studentsStatistics == null) {
				setChartData(null);
			} else {
				var currentStudentStatistics = studentsStatistics.SingleOrDefault(
					s => s.StudentId == AppPrefs.UserId);
				setChartData(currentStudentStatistics);
				_students = studentsStatistics;
			}
		}

		void setChartData(StatisticsStudentModel stats)
		{
			if (stats == null) {
				stats = new StatisticsStudentModel();
			}

			var avgLabs = StringConverter.StringToDouble(stats.AverageLabsMark);
			AverageLabs = avgLabs.ToString(GlobalConsts.DoubleStringFormat);

			var avgTests = StringConverter.StringToDouble(stats.AverageTestMark);
			AverageTests = avgTests.ToString(GlobalConsts.DoubleStringFormat);

			var rating = (avgLabs + avgTests) / 2;
			Rating = rating.ToString(GlobalConsts.DoubleStringFormat);

			setNotEnoughDetails(avgLabs == 0 && avgTests == 0 && rating == 0);

			ChartEntries = new List<double> {
				avgLabs, avgTests, rating
			};
		}

		async Task<List<StatisticsStudentModel>> getStatistics()
		{
			var groupId = AppPrefs.GroupId;

			if (CurrentSubject == null || groupId == -1) {
				return null;
			}

			var statisticsModel = await DataAccess.GetStatistics(CurrentSubject.Id, AppPrefs.GroupId);

			if (statisticsModel.IsError) {
				await DialogService.ShowError(statisticsModel.ErrorMessage);
				return null;
			}

			return statisticsModel.Students?.ToList();
		}

		void setPagesList()
		{
			PagesList = new List<StatsPageModel> {
				getPage("statistics_page_labs_rating"),
				getPage("statistics_page_labs_visiting"),
				getPage("statistics_page_lectures_visiting")
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
			if (selectedObject == null || selectedObject.GetType() != typeof(StatsPageModel)) {
				return;
			}

			var page = selectedObject as StatsPageModel;
			var pageType = getPageToOpen(page.Title);

			if (AppUserData.UserType == UserTypeEnum.Professor) {
				_navigationService.OpenStudentsListStats(
					(int)pageType, CurrentSubject.Id, _students, page.Title);
				return;
			}

			var user = _students.SingleOrDefault(s => s.StudentId == AppPrefs.UserId);

			if (user == null) {
				return;
			}

			_navigationService.OpenDetailedStatistics(
				user.Login, CurrentSubject.Id, AppPrefs.GroupId, (int)pageType, page.Title);
		}

		StatsPageEnum getPageToOpen(string pageString)
		{
			var labsRatingString = CrossLocalization.Translate("statistics_page_labs_rating");
			var labsVisitingString = CrossLocalization.Translate("statistics_page_labs_visiting");

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
