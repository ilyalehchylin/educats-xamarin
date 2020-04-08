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
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS.Pages.Statistics.Students.ViewModels
{
	public class StudentsPageViewModel : GroupsViewModel
	{
		readonly int _pageIndex;

		List<StudentsPageModel> _backupStudents;

		public StudentsPageViewModel(
			IPlatformServices services, int subjectId,
			List<StatsStudentModel> studentsList, int pageIndex) : base(services, subjectId)
		{
			_pageIndex = pageIndex;
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

		bool _isLoading;
		public bool IsLoading {
			get { return _isLoading; }
			set { SetProperty(ref _isLoading, value); }
		}

		string _searchText;
		public string SearchText {
			get { return _searchText; }
			set {
				SetProperty(ref _searchText, value);
				search(_searchText);
			}
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

		List<StudentsPageModel> _students;
		public List<StudentsPageModel> Students {
			get { return _students; }
			set { SetProperty(ref _students, value); }
		}

		Command _refreshCommand;
		public Command RefreshCommand {
			get {
				return _refreshCommand ?? (
					_refreshCommand = new Command(async () => await getAndSetStudents()));
			}
		}

		async Task update(bool groupsOnly = false)
		{
			try {
				await SetupGroups();

				if (groupsOnly) {
					return;
				}

				await getAndSetStudents();
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		async Task getAndSetStudents()
		{
			IsLoading = true;
			var studentsStatistics = await getStatistics();
			setStudents(studentsStatistics);
			IsLoading = false;
		}

		void setStudents(List<StatsStudentModel> studentsStatistics)
		{
			try {
				var students = studentsStatistics?.Select(s => new StudentsPageModel(s.Login, s.Name));

				if (students == null) {
					return;
				}

				Students = new List<StudentsPageModel>(students);
				_backupStudents = new List<StudentsPageModel>(students);
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		async Task<List<StatsStudentModel>> getStatistics()
		{
			if (CurrentGroup == null) {
				return null;
			}

			var statisticsModel = await DataAccess.GetStatistics(SubjectId, CurrentGroup.GroupId);

			if (DataAccess.IsError && !DataAccess.IsConnectionError) {
				PlatformServices.Dialogs.ShowError(DataAccess.ErrorMessage);
			}

			return statisticsModel?.Students?.ToList();
		}

		void search(string text)
		{
			try {
				if (string.IsNullOrEmpty(text)) {
					Students = new List<StudentsPageModel>(_backupStudents);
					return;
				}

				text = text.ToLower();

				var foundStudents = _backupStudents.Where(
					s => string.IsNullOrEmpty(s.Name) ? false : s.Name.ToLower().Contains(text));

				Students = new List<StudentsPageModel>(foundStudents);
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		void openPage(object selectedObject)
		{
			try {
				if (selectedObject == null || selectedObject.GetType() != typeof(StudentsPageModel)) {
					return;
				}

				var student = selectedObject as StudentsPageModel;
				var title = getTitle((StatsPageEnum)_pageIndex);

				if (CurrentGroup == null) {
					return;
				}

				PlatformServices.Navigation.OpenDetailedStatistics(
					student.Username, SubjectId, CurrentGroup.GroupId, _pageIndex, title, student.Name);
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		string getTitle(StatsPageEnum pageType)
		{
			return pageType switch
			{
				StatsPageEnum.LabsRating => CrossLocalization.Translate("stats_page_labs_rating"),
				StatsPageEnum.LabsVisiting => CrossLocalization.Translate("stats_page_labs_visiting"),
				_ => CrossLocalization.Translate("stats_page_lectures_visiting"),
			};
		}
	}
}
