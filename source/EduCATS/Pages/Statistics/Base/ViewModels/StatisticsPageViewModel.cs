using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduCATS.Data;
using EduCATS.Data.Models.Statistics;
using EduCATS.Helpers.Dialogs.Interfaces;
using EduCATS.Helpers.Settings;
using EduCATS.Pages.Utils.ViewModels;
using Xamarin.Forms;

namespace EduCATS.Pages.Statistics.Base.ViewModels
{
	public class StatisticsPageViewModel : SubjectsViewModel
	{
		public StatisticsPageViewModel(IDialogs dialogService) : base(dialogService)
		{
			Task.Run(async () => {
				await SetupSubjects();
				await getAndSetStatistics();
			});
		}

		bool isLoading;
		public bool IsLoading {
			get { return isLoading; }
			set { SetProperty(ref isLoading, value); }
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

		protected async Task executeRefreshCommand()
		{
			IsLoading = true;
			await getAndSetStatistics();
		}

		async Task getAndSetStatistics()
		{
			var studentsStatistics = await getStatistics();
			var currentStudentStatistics = studentsStatistics.SingleOrDefault(
				s => s.StudentId == AppPrefs.UserId);
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
	}
}