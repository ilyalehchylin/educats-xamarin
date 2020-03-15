using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduCATS.Data;
using EduCATS.Data.Models.Statistics;
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
			IPages navigationService, IDialogs dialogService, int subjectId,
			List<StatisticsStudentModel> studentsList, int pageIndex) : base(dialogService, subjectId)
		{
			this.navigationService = navigationService;
			this.pageIndex = pageIndex;

			setStudents(studentsList);

			Task.Run(async () => {
				await SetupGroups();

				if (studentsList == null || studentsList.Count == 0) {
					await getAndSetStudents();
				}
			});

			GroupChanged += async (id, name) => {
				await getAndSetStudents();
			};
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

		async Task getAndSetStudents()
		{
			IsLoading = true;
			var studentsStatistics = await getStatistics();
			setStudents(studentsStatistics);
			IsLoading = false;
		}

		void setStudents(List<StatisticsStudentModel> studentsStatistics)
		{
			// TODO: error handling
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

			if (statisticsModel.IsError) {
				await DialogService.ShowError(statisticsModel.ErrorMessage);
				return null;
			}

			return statisticsModel.Students?.ToList();
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
