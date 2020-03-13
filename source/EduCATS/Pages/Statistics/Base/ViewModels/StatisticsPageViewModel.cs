using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduCATS.Data;
using EduCATS.Data.Models.Statistics;
using EduCATS.Helpers.Dialogs.Interfaces;
using EduCATS.Helpers.Rating;
using EduCATS.Helpers.Settings;
using EduCATS.Pages.Statistics.Base.Models;
using EduCATS.Pages.Utils.ViewModels;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS.Pages.Statistics.Base.ViewModels
{
	public class StatisticsPageViewModel : SubjectsViewModel
	{
		public StatisticsPageViewModel(IDialogs dialogService) : base(dialogService)
		{
			setPagesList();
			setCollapsedDetails();

			Task.Run(async () => {
				await SetupSubjects();
				await getAndSetStatistics();
			});

			SubjectChanged += async (id, name) => {
				await getAndSetStatistics();
			};
		}

		bool isLoading;
		public bool IsLoading {
			get { return isLoading; }
			set { SetProperty(ref isLoading, value); }
		}

		bool isExpandedStatistics;
		public bool IsExpandedStatistics {
			get { return isExpandedStatistics; }
			set { SetProperty(ref isExpandedStatistics, value); }
		}

		bool isCollapsedStatistics;
		public bool IsCollapsedStatistics {
			get { return isCollapsedStatistics; }
			set { SetProperty(ref isCollapsedStatistics, value); }
		}

		bool isNotEnoughDetails;
		public bool IsNotEnoughDetails {
			get { return isNotEnoughDetails; }
			set { SetProperty(ref isNotEnoughDetails, value); }
		}

		bool isEnoughDetails;
		public bool IsEnoughDetails {
			get { return isEnoughDetails; }
			set { SetProperty(ref isEnoughDetails, value); }
		}

		List<double> chartEntries;
		public List<double> ChartEntries {
			get { return chartEntries; }
			set { SetProperty(ref chartEntries, value); }
		}

		List<StatisticsPageModel> pagesList;
		public List<StatisticsPageModel> PagesList {
			get { return pagesList; }
			set { SetProperty(ref pagesList, value); }
		}

		object selectedItem;
		public object SelectedItem {
			get { return selectedItem; }
			set {
				SetProperty(ref selectedItem, value);

				if (selectedItem != null) {
					// open page
				}
			}
		}

		Command refreshCommand;
		public Command RefreshCommand {
			get {
				return refreshCommand ?? (refreshCommand = new Command(
					async () => await executeRefreshCommand()));
			}
		}

		Command expandCommand;
		public Command ExpandCommand {
			get {
				return expandCommand ?? (expandCommand = new Command(executeExpandCommand));
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
			}
		}

		void setChartData(StatisticsStudentModel currentStatistics)
		{
			if (currentStatistics == null) {
				currentStatistics = new StatisticsStudentModel();
			}

			var averageLabsMark = RatingHelper.GetAverageMark(currentStatistics.AverageLabsMark);
			var averageTestsMark = RatingHelper.GetAverageMark(currentStatistics.AverageTestMark);
			var averageVisiting = RatingHelper.GetAverageVisiting(currentStatistics.VisitingList?.ToList());

			setNotEnoughDetails(averageLabsMark == 0 && averageTestsMark == 0 && averageVisiting == 0);

			ChartEntries = new List<double> {
				averageLabsMark,
				averageTestsMark,
				averageVisiting
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
			PagesList = new List<StatisticsPageModel> {
				getPage("statistics_page_labs_rating"),
				getPage("statistics_page_labs_visiting"),
				getPage("statistics_page_lectures_visiting")
			};
		}

		StatisticsPageModel getPage(string text)
		{
			return new StatisticsPageModel {
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
	}
}
