using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduCATS.Data;
using EduCATS.Data.Models;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Logs;
using EduCATS.Pages.Pickers;
using EduCATS.Pages.Statistics.Enums;
using EduCATS.Pages.Statistics.Students.Models;
using EduCATS.Pages.Statistics.Students.ViewModels;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;


namespace EduCATS.Pages.Parental.Statistics.ViewsModels
{
	class ParentalStudentsPageViewModel : StudentsPageViewModel
	{

		public ParentalStudentsPageViewModel(IPlatformServices services, int subjectId,List<StatsStudentModel> studentsList, int pageIndex) : base(services,subjectId, studentsList, pageIndex)
		{
		}

		public override void Init()
		{
			CurrentGroup = new GroupItemModel() { GroupId = _service.Preferences.GroupId, GroupName = _service.Preferences.GroupName };

			setStudents(_studentsList);

			Task.Run(async () =>
			{
				if (_studentsList == null || _studentsList.Count == 0)
				{
					await update();
				}
			});

			GroupChanged += async (id, name) => await update();

		}
	
		protected override async Task update(bool groupsOnly = false)
		{
			try
			{
				await getAndSetStudents();
			}
			catch (Exception ex)
			{
				AppLogs.Log(ex);
			}
		}

		protected override async Task getAndSetStudents()
		{
			PlatformServices.Device.MainThread(() => PlatformServices.Dialogs.ShowLoading());
			IsLoading = false;
			var studentsStatistics = await getStatistics();
			setStudents(studentsStatistics);
			PlatformServices.Device.MainThread(() => PlatformServices.Dialogs.HideLoading());
		}

		protected override async Task<List<StatsStudentModel>> getStatistics()
		{
			if (CurrentGroup == null)
			{
				return null;
			}

			var statisticsModel = await DataAccess.GetStatistics(SubjectId, CurrentGroup.GroupId);

			if (DataAccess.IsError && !DataAccess.IsConnectionError)
			{
				_service.Dialogs.ShowError(DataAccess.ErrorMessage);
			}

			return statisticsModel?.Students?.ToList();
		}
	}
}

