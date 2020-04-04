using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduCATS.Data;
using EduCATS.Data.Models;
using EduCATS.Helpers.Devices.Interfaces;
using EduCATS.Helpers.Dialogs.Interfaces;
using EduCATS.Helpers.Settings;
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
		public readonly IDialogs DialogService;
		public readonly IDevice DeviceService;

		public List<SubjectModel> CurrentSubjects { get; set; }
		public SubjectModel CurrentSubject { get; set; }

		public delegate void SubjectEventHandler(int id, string name);
		public event SubjectEventHandler SubjectChanged;

		public SubjectsViewModel(IDialogs dialogService, IDevice deviceService)
		{
			DialogService = dialogService;
			DeviceService = deviceService;
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
			if (CurrentSubjects == null) {
				return;
			}

			var buttons = CurrentSubjects.Select(s => s.Name).ToList();
			var name = await DialogService.ShowSheet(CrossLocalization.Translate("subjects_choose"), buttons);

			if (string.IsNullOrEmpty(name) ||
				string.Compare(name, CrossLocalization.Translate("base_cancel")) == 0) {
				return;
			}

			var isChosen = setChosenSubject(name);

			if (isChosen) {
				SubjectChanged?.Invoke(AppPrefs.ChosenSubjectId, name);
			}
		}

		/// <summary>
		/// Setup subjects.
		/// </summary>
		/// <returns>Task.</returns>
		public async Task SetupSubjects()
		{
			var subjects = await getSubjects();

			if (subjects == null) {
				return;
			}

			setCurrentSubjectsList(subjects.ToList());
			setupSubject();
		}

		/// <summary>
		/// Fetch subjects.
		/// </summary>
		/// <returns>List of subjects.</returns>
		async Task<IList<SubjectModel>> getSubjects()
		{
			var subjects = await DataAccess.GetProfileInfoSubjects(AppPrefs.UserLogin);

			if (DataAccess.IsError) {
				DeviceService.MainThread(
					() => DialogService.ShowError(DataAccess.ErrorMessage));
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
			try {
				var subject = CurrentSubjects.SingleOrDefault(
							s => string.Compare(s.Name, subjectName) == 0);
				return setChosenSubject(subject);
			} catch (InvalidOperationException) {
				DialogService.ShowError(CrossLocalization.Translate("subjects_identical_error"));
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
				AppPrefs.ChosenSubjectId = subject.Id;
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
