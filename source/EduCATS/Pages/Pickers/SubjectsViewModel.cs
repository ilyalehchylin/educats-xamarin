using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduCATS.Data;
using EduCATS.Data.Models;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Logs;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS.Pages.Pickers
{
	/// <summary>
	/// Subjects view model.
	/// </summary>
	/// <remarks>Used for Subjects picker.</remarks>
	public class SubjectsViewModel : ViewModel
	{
		/// <summary>
		/// Platform services.
		/// </summary>
		public readonly IPlatformServices PlatformServices;

		public List<SubjectModel> CurrentSubjects { get; set; }
		public SubjectModel CurrentSubject { get; set; }

		public delegate void SubjectEventHandler(int id, string name);
		public event SubjectEventHandler SubjectChanged;

		public SubjectsViewModel(IPlatformServices platformServices)
		{
			PlatformServices = platformServices;
		}

		string _chosenSubject;
		public string ChosenSubject {
			get { return _chosenSubject; }
			set { SetProperty(ref _chosenSubject, value); }
		}

		string _chosenSubjectColor;
		public string ChosenSubjectColor {
			get { return _chosenSubjectColor; }
			set { SetProperty(ref _chosenSubjectColor, value); }
		}

		Command _chooseSubjectCommand;
		public Command ChooseSubjectCommand {
			get {
				return _chooseSubjectCommand ?? (
					_chooseSubjectCommand = new Command(chooseSubject));
			}
		}

		Command _subjectSelectedCommand;
		public Command SubjectSelectedCommand {
			get {
				return _subjectSelectedCommand ?? (_subjectSelectedCommand = new Command(subjectChosen));
			}
		}

		protected void chooseSubject()
		{
			try {
				if (CurrentSubjects == null) {
					return;
				}

				var buttons = new Dictionary<int, string>();
				foreach (var subject in CurrentSubjects) {
					buttons.Add(subject.Id, subject.Name);
				}

				PlatformServices.Dialogs.ShowSheet(
					CrossLocalization.Translate("subjects_choose"), buttons, SubjectSelectedCommand);
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		protected void subjectChosen(object chosenObject)
		{
			try {
				var id = Convert.ToInt32(chosenObject);

				if (id == -1) {
					return;
				}

				var subject = CurrentSubjects.SingleOrDefault(s => s.Id == id);
				var isChosen = setChosenSubject(subject);

				if (isChosen) {
					SubjectChanged?.Invoke(PlatformServices.Preferences.ChosenSubjectId, subject.Name);
				}
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		/// <summary>
		/// Setup subjects.
		/// </summary>
		/// <returns>Task.</returns>
		public async Task SetupSubjects()
		{
			try {
				var subjects = await getSubjects();

				if (subjects == null) {
					return;
				}

				SetCurrentSubjectsList(subjects.Distinct().ToList());
				SetupSubject();
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		/// <summary>
		/// Fetch subjects.
		/// </summary>
		/// <returns>List of subjects.</returns>
		async Task<IList<SubjectModel>> getSubjects()
		{
			var subjects = await DataAccess.GetProfileInfoSubjects(PlatformServices.Preferences.UserLogin);

			if (DataAccess.IsError) {
				PlatformServices.Device.MainThread(
					() => PlatformServices.Dialogs.ShowError(DataAccess.ErrorMessage));
			}

			return subjects;
		}

		/// <summary>
		/// Setup subject by subject name.
		/// </summary>
		public void SetupSubject()
		{
			if (!checkSubjectsList()) {
				return;
			}

			var savedSubjectId = PlatformServices.Preferences.ChosenSubjectId;
			var success = setChosenSubject(savedSubjectId);

			if (!success) {
				setChosenSubject(CurrentSubjects[0]);
			}
		}

		bool setChosenSubject(int subjectId)
		{
			var subject = CurrentSubjects.SingleOrDefault(
						s => s.Id == subjectId);

			return setChosenSubject(subject);
		}

		bool setChosenSubject(SubjectModel subject)
		{
			if (subject != null) {
				CurrentSubject = subject;
				ChosenSubject = subject.Name;
				ChosenSubjectColor = subject.Color;
				PlatformServices.Preferences.ChosenSubjectId = subject.Id;
				return true;
			}

			return false;
		}

		public void SetCurrentSubjectsList(List<SubjectModel> subjects)
		{
			CurrentSubjects = subjects.OrderBy(x => x.Name).ToList();
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
