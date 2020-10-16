using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduCATS.Data;
using EduCATS.Data.Models;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Logs;
using EduCATS.Pages.Statistics.Base.Models;
using Xamarin.Forms;
using EduCATS.Pages.Parental.FindGroup.Models;
using EduCATS.Pages.Statistics.Base.ViewModels;

namespace EduCATS.Pages.Parental.Statistics
{
	class ParentalsStatsPageViewModel : StatsPageViewModel
	{

		private List<StatsStudentModel> _students;

		private IPlatformServices _service;

		public GroupInfo Group { get; set; }
		
		Command _parentalCommand;
		
		public Command ParentalCommand
		{
			get
			{
				return _parentalCommand ?? (_parentalCommand = new Command(
					 () => openParental()));
			}
		}

		public ParentalsStatsPageViewModel(IPlatformServices services, GroupInfo group) : base(services)
		{
			_service = services;
			Group = group;
		}

		public new void Init()
		{
			setPagesList();
			setCollapsedDetails();

			_service.Device.MainThread(async () =>
			{
				IsLoading = true;
				SetupSubjects();
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

		protected new void SetupSubjects()
		{
			try
			{
				List<SubjectModel> subjects = new List<SubjectModel>();
				foreach (var subject in Group.Subjects)
				{
					subjects.Add(new SubjectModel() { Id = subject.Id, ShortName = subject.ShortName, Name = subject.Name, Color = Color.Blue.ToString(), Completing = 0 });
				}
				SetCurrentSubjectsList(subjects.ToList());
				SetupSubject();
			}
			catch (Exception ex)
			{
				AppLogs.Log(ex);
			}

		}

		protected override async Task executeRefreshCommand()
		{
			try
			{
				PlatformServices.Device.MainThread(() => IsLoading = true);
				SetupSubjects();
				await getAndSetStatistics();
				PlatformServices.Device.MainThread(() => IsLoading = false);
			}
			catch (Exception ex)
			{
				AppLogs.Log(ex);
			}
		}

		protected override async Task getAndSetStatistics()
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

		protected override async Task<List<StatsStudentModel>> getStatistics()
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

		protected override void openPage(object selectedObject)
		{
			try
			{
				if (selectedObject == null || selectedObject.GetType() != typeof(StatsPageModel))
				{
					return;
				}

				var page = selectedObject as StatsPageModel;
				var pageType = getPageToOpen(page.Title);

				PlatformServices.Navigation.OpenParentalStudentsListStats(_service,
					(int)pageType, CurrentSubject.Id, _students, page.Title);
				return;
			}
			catch (Exception ex)
			{
				AppLogs.Log(ex);
			}
		}

		/// <summary>
		/// Go to FindGroup Page
		/// </summary>
		protected void openParental()
		{
			_service.Navigation.OpenParental();
		}

	}
}
