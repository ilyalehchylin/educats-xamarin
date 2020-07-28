using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduCATS.Data;
using EduCATS.Data.Models;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Logs;
using EduCATS.Pages.Pickers;
using EduCATS.Pages.Statistics.Base.Models;
using EduCATS.Pages.Statistics.Enums;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;
using EduCATS.Pages.Parental.FindGroup.Models;

namespace EduCATS.Pages.Parental.Statistics
{
	class ParentalsStatsPageViewModel : SubjectsViewModel
	{

		public GroupInfo Group { get; set; }
		List<StatsStudentModel> _students;
		private IPlatformServices services;

		Command _parentalCommand;
		public Command ParentalCommand
		{
			get
			{
				return _parentalCommand ?? (_parentalCommand = new Command(
					async () => await openParental()));
			}
		}

		protected async Task openParental()
		{
			services.Navigation.OpenParental();
		}


		public ParentalsStatsPageViewModel(IPlatformServices services, GroupInfo group) : base(services)
		{
			this.services = services;
			Group = group;
			setPagesList();
			setCollapsedDetails();

			services.Device.MainThread(async () =>
			{
				IsLoading = true;
				await SetupSubjects();
				await getAndSetStatistics();
				IsLoading = false;
			});

			SubjectChanged += async (id, name) =>
			{
				PlatformServices.Dialogs.ShowLoading();
				await getAndSetStatistics();
				PlatformServices.Dialogs.HideLoading();
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

		bool _isStudent = false;
		public bool IsStudent
		{
			get { return _isStudent; }
		}

		string _averageLabs;
		public string AverageLabs
		{
			get { return _averageLabs; }
			set { SetProperty(ref _averageLabs, value); }
		}

		string _averageTests;
		public string AverageTests
		{
			get { return _averageTests; }
			set { SetProperty(ref _averageTests, value); }
		}

		string _rating;
		public string Rating
		{
			get { return _rating; }
			set { SetProperty(ref _rating, value); }
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

		Command _refreshCommand;
		public Command RefreshCommand
		{
			get
			{
				return _refreshCommand ?? (_refreshCommand = new Command(
					async () => await executeRefreshCommand()));
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

		protected async Task executeRefreshCommand()
		{
			try
			{
				PlatformServices.Device.MainThread(() => IsLoading = true);
				await SetupSubjects();
				await getAndSetStatistics();
				PlatformServices.Device.MainThread(() => IsLoading = false);
			}
			catch (Exception ex)
			{
				AppLogs.Log(ex);
			}
		}

		protected void executeExpandCommand()
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

		async Task getAndSetStatistics()
		{
			try
			{
				var studentsStatistics = await getStatistics();
				_students = studentsStatistics;
			}
			catch (Exception ex)
			{
				AppLogs.Log(ex);
			}
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
			return new StatsPageModel
			{
				Title = CrossLocalization.Translate(text)
			};
		}

		void setCollapsedDetails(bool isCollapsed = true)
		{
			IsCollapsedStatistics = isCollapsed;
			IsExpandedStatistics = !isCollapsed;
		}

		void openPage(object selectedObject)
		{
			try
			{
				if (selectedObject == null || selectedObject.GetType() != typeof(StatsPageModel))
				{
					return;
				}

				var page = selectedObject as StatsPageModel;
				var pageType = getPageToOpen(page.Title);

				PlatformServices.Navigation.OpenParentalStudentsListStats(services,
					(int)pageType, CurrentSubject.Id, _students, page.Title);
				return;
			}
			catch (Exception ex)
			{
				AppLogs.Log(ex);
			}
		}

		StatsPageEnum getPageToOpen(string pageString)
		{
			var labsRatingString = CrossLocalization.Translate("stats_page_labs_rating");
			var labsVisitingString = CrossLocalization.Translate("stats_page_labs_visiting");

			if (pageString.Equals(labsRatingString))
			{
				return StatsPageEnum.LabsRating;
			}
			else if (pageString.Equals(labsVisitingString))
			{
				return StatsPageEnum.LabsVisiting;
			}
			else
			{
				return StatsPageEnum.LecturesVisiting;
			}
		}

		public override async Task SetupSubjects()
		{
			try
			{
				List<SubjectModel> subjects = new List<SubjectModel>();
				foreach (var subject in Group.Subjects)
				{
					subjects.Add(new SubjectModel() { Id = subject.Id, ShortName = subject.ShortName, Name = subject.Name, Color = Color.Blue.ToString(), Completing = 0 });
				}
				setCurrentSubjectsList(subjects.ToList());
				setupSubject();
			}
			catch (Exception ex)
			{
				AppLogs.Log(ex);
			}

		}

		protected async Task<List<StatsStudentModel>> getStatistics()
		{
			try
			{
				var statisticsModel = await DataAccess.GetStatistics(
					CurrentSubject.Id, Group.GroupId);

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
	}
}
