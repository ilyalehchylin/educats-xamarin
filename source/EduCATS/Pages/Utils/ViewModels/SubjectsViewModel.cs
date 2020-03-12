using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduCATS.Data;
using EduCATS.Data.Models.Subjects;
using EduCATS.Helpers.Dialogs.Interfaces;
using EduCATS.Helpers.Settings;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS.Pages.Utils.ViewModels
{
	public class SubjectsViewModel : ViewModel
	{
		public readonly IDialogs DialogService;

		public List<SubjectItemModel> CurrentSubjects { get; set; }
		public SubjectItemModel CurrentSubject { get; set; }

		public delegate void SubjectEventHandler(int id, string name);
		public event SubjectEventHandler SubjectChanged;

		public SubjectsViewModel(IDialogs dialogService)
		{
			DialogService = dialogService;
		}

		string chosenSubject;
		public string ChosenSubject {
			get { return chosenSubject; }
			set { SetProperty(ref chosenSubject, value); }
		}

		string chosenSubjectColor;
		public string ChosenSubjectColor {
			get { return chosenSubjectColor; }
			set { SetProperty(ref chosenSubjectColor, value); }
		}

		Command chooseSubjectCommand;
		public Command ChooseSubjectCommand {
			get {
				return chooseSubjectCommand ?? (
					chooseSubjectCommand = new Command(
						async () => await executeChooseSubjectCommand()));
			}
		}

		protected async Task executeChooseSubjectCommand()
		{
			if (CurrentSubjects == null) {
				return;
			}

			var buttons = CurrentSubjects.Select(s => s.Name).ToList();
			var name = await DialogService.ShowSheet(CrossLocalization.Translate("subjects_choose"), buttons);

			if (string.IsNullOrEmpty(name) ||
				string.Compare(name, CrossLocalization.Translate("common_cancel")) == 0) {
				return;
			}

			var isChosen = setChosenSubject(name);

			if (isChosen) {
				SubjectChanged?.Invoke(AppPrefs.ChosenSubjectId, name);
			}
		}

		public async Task SetupSubjects()
		{
			var subjects = await getSubjects();

			if (subjects == null) {
				return;
			}

			setCurrentSubjectsList(subjects.ToList());
			setupSubject();
		}

		async Task<IList<SubjectItemModel>> getSubjects()
		{
			var subjects = await DataAccess.GetProfileInfoSubjects(AppPrefs.UserLogin);

			if (subjects.IsError) {
				await DialogService.ShowError(subjects.ErrorMessage);
				return null;
			}

			return subjects.SubjectsList;
		}

		void setupSubject(string subjectName = null)
		{
			if (!checkSubjectsList()) {
				return;
			}

			if (string.IsNullOrEmpty(subjectName)) {
				var savedSubjectId = AppPrefs.ChosenSubjectId;
				var success = setChosenSubject(savedSubjectId);

				if (!success) {
					setChosenSubject(CurrentSubjects[0]);
				}

				return;
			}

			setChosenSubject(subjectName);
		}

		bool setChosenSubject(int subjectId)
		{
			var subject = CurrentSubjects.SingleOrDefault(
						s => s.Id == subjectId);

			return setChosenSubject(subject);
		}

		bool setChosenSubject(string subjectName)
		{
			var subject = CurrentSubjects.SingleOrDefault(
						s => string.Compare(s.Name, subjectName) == 0);

			return setChosenSubject(subject);
		}

		bool setChosenSubject(SubjectItemModel subject)
		{
			if (subject != null) {
				CurrentSubject = subject;
				ChosenSubject = subject.Name;
				ChosenSubjectColor = subject.Color;
				AppPrefs.ChosenSubjectId = subject.Id;
				return true;
			}

			return false;
		}

		void setCurrentSubjectsList(List<SubjectItemModel> subjects)
		{
			CurrentSubjects = subjects;
		}

		bool checkSubjectsList()
		{
			if (CurrentSubjects != null && CurrentSubjects.Count > 0) {
				return true;
			}

			return false;
		}
	}
}