using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduCATS.Data;
using EduCATS.Data.Models.Statistics;
using EduCATS.Helpers.Devices.Interfaces;
using EduCATS.Helpers.Dialogs.Interfaces;
using EduCATS.Helpers.Pages.Interfaces;
using EduCATS.Pages.Statistics.Enums;
using EduCATS.Pages.Statistics.Students.Models;
using EduCATS.Pages.Utils.ViewModels;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS.Pages.Statistics.Students.ViewModels
{
	public class StudentsPageViewModel : GroupsViewModel
	{
		readonly IPages navigationService;
		readonly int pageIndex;

		List<StudentsPageModel> backupStudentsList;

		public StudentsPageViewModel(
			IPages navigationService, IDialogs dialogService, IAppDevice device, int subjectId,
			List<StatisticsStudentModel> studentsList, int pageIndex)
			: base(dialogService, device, subjectId)
		{
			this.navigationService = navigationService;
			this.pageIndex = pageIndex;

			setStudents(studentsList);

			Task.Run(async () => {
				if (studentsList == null || studentsList.Count == 0) {
					await update();
				} else {
					await update(true);
				}
			});

			GroupChanged += async (id, name) => await update();
		}

		bool isLoading;
		public bool IsLoading {
			get { return isLoading; }
			set { SetProperty(ref isLoading, value); }
		}

		string searchText;
		public string SearchText {
			get { return searchText; }
			set {
				SetProperty(ref searchText, value);
				search(searchText);
			}
		}

		object selectedItem;
		public object SelectedItem {
			get { return selectedItem; }
			set {
				SetProperty(ref selectedItem, value);

				if (selectedItem != null) {
					openPage(selectedItem);
				}
			}
		}

		List<StudentsPageModel> students;
		public List<StudentsPageModel> Students {
			get { return students; }
			set { SetProperty(ref students, value); }
		}

		Command refreshCommand;
		public Command RefreshCommand {
			get {
				return refreshCommand ?? (
					refreshCommand = new Command(async () => await executeRefreshCommand()));
			}
		}

		async Task update(bool groupsOnly = false)
		{
			await SetupGroups();

			if (groupsOnly) {
				return;
			}

			await getAndSetStudents();
		}

		async Task getAndSetStudents()
		{
			IsLoading = true;
			var studentsStatistics = await getStatistics();
			setStudents(studentsStatistics);
			IsLoading = false;
		}

		void setStudents(List<StatisticsStudentModel> studentsStatistics)
		{
			var students = studentsStatistics?.Select(s => new StudentsPageModel(s.Login, s.Name));

			if (students == null) {
				return;
			}

			Students = new List<StudentsPageModel>(students);
			backupStudentsList = new List<StudentsPageModel>(students);
		}

		async Task<List<StatisticsStudentModel>> getStatistics()
		{
			if (CurrentGroup == null) {
				return null;
			}

			var statisticsModel = await DataAccess.GetStatistics(SubjectId, CurrentGroup.GroupId);

			if (DataAccess.IsError && !DataAccess.IsConnectionError) {
				DialogService.ShowError(DataAccess.ErrorMessage);
			}

			return statisticsModel?.Students?.ToList();
		}

		void search(string text)
		{
			if (string.IsNullOrEmpty(text)) {
				Students = new List<StudentsPageModel>(backupStudentsList);
				return;
			}

			text = text.ToLower();

			var foundStudents = backupStudentsList.Where(
				s => string.IsNullOrEmpty(s.Name) ? false : s.Name.ToLower().Contains(text));

			Students = new List<StudentsPageModel>(foundStudents);
		}

		protected async Task executeRefreshCommand()
		{
			await getAndSetStudents();
		}

		void openPage(object selectedObject)
		{
			if (selectedObject == null || selectedObject.GetType() != typeof(StudentsPageModel)) {
				return;
			}

			var student = selectedObject as StudentsPageModel;
			var title = getTitle((StatsPageEnum)pageIndex);

			if (CurrentGroup == null) {
				return;
			}

			navigationService.OpenDetailedStatistics(
				student.Username, SubjectId, CurrentGroup.GroupId, pageIndex, title, student.Name);
		}

		string getTitle(StatsPageEnum pageType)
		{
			switch (pageType) {
				case StatsPageEnum.LabsRating:
					return CrossLocalization.Translate("statistics_page_labs_rating");
				case StatsPageEnum.LabsVisiting:
					return CrossLocalization.Translate("statistics_page_labs_visiting");
				default:
					return CrossLocalization.Translate("statistics_page_lectures_visiting");
			}
		}
	}
}
