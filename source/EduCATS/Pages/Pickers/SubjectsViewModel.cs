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
					_chooseSubjectCommand = new Command(
						async () => await executeChooseSubjectCommand()));
			}
		}

		protected async Task executeChooseSubjectCommand()
		{
			try {
				if (CurrentSubjects == null) {
					return;
				}

				var buttons = CurrentSubjects.Select(s => s.Name).ToList();
				var name = await PlatformServices.Dialogs.ShowSheet(
					CrossLocalization.Translate("subjects_choose"), buttons);

				if (string.IsNullOrEmpty(name) ||
					string.Compare(name, CrossLocalization.Translate("base_cancel")) == 0) {
					return;
				}

				var isChosen = setChosenSubject(name);

				if (isChosen) {
					SubjectChanged?.Invoke(PlatformServices.Preferences.ChosenSubjectId, name);
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

				setCurrentSubjectsList(subjects.ToList());
				setupSubject();
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
		/// <param name="subjectName">Subject name.</param>
		void setupSubject(string subjectName = null)
		{
			if (!checkSubjectsList()) {
				return;
			}

			if (string.IsNullOrEmpty(subjectName)) {
				var savedSubjectId = PlatformServices.Preferences.ChosenSubjectId;
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
			try {
				var subject = CurrentSubjects.SingleOrDefault(
							s => string.Compare(s.Name, subjectName) == 0);
				return setChosenSubject(subject);
			} catch (InvalidOperationException) {
				PlatformServices.Dialogs.ShowError(CrossLocalization.Translate("subjects_identical_error"));
				return false;
			} catch (Exception) {
				return false;
			}
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

		void setCurrentSubjectsList(List<SubjectModel> subjects)
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
