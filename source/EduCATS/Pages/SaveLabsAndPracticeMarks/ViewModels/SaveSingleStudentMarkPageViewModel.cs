using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Json;
using EduCATS.Networking;
using EduCATS.Networking.AppServices;
using EduCATS.Networking.Models.SaveMarks;
using EduCATS.Networking.Models.SaveMarks.LabSchedule;
using EduCATS.Networking.Models.SaveMarks.Practicals;
using EduCATS.Pages.SaveMarks;
using EduCATS.Pages.Statistics.Marks.Models;
using Nyxbull.Plugins.CrossLocalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EduCATS.Pages.SaveLabsAndPracticeMarks.ViewModels
{
	public class SaveSingleStudentMarkPageViewModel : ViewModel
	{
		public readonly IPlatformServices _services;
		public int selSubGroup { get; set; }
		public List<VisitingPageModel> _currentLabsVisitingMarksSubGroup1;
		public List<VisitingPageModel> _currentLabsVisitingMarksSubGroup2;
		public List<string> FullNames { get; set; }
		public List<string> NameOfLabOrPr { get; set; }
		public TakedLabs _takedLabs { get; set; }
		public string _title { get; set; }
		public string studentName { get; set; }
		public int _subGruop { get; set; }

		public LabsVisitingList fullMarksLabs = new LabsVisitingList();
		public LabsVisitingList fullPractice = new LabsVisitingList();

		public SaveSingleStudentMarkPageViewModel(IPlatformServices services,
			List<string> nameofLabOrPr, LabsVisitingList marks, TakedLabs prOrLabStat, string title, string _studName, int subGruop)
		{
			_subGruop = subGruop;
			studentName = _studName;
			_title = title;
			if (title == CrossLocalization.Translate("practice_mark"))
			{
				fullPractice = marks;
			}
			else if (title == CrossLocalization.Translate("stats_page_labs_rating"))
			{
				fullMarksLabs = marks;
			}
			_services = services;
			_takedLabs = prOrLabStat;
			SelectedShortName = nameofLabOrPr[0];
			NameOfLabOrPr = nameofLabOrPr;
			setMarks(SelectedShortName);
		}

		Command _saveMarksButton;
		public Command SaveMarksButton
		{
			get
			{
				return _saveMarksButton ?? (_saveMarksButton = new Command(
					async () => await saveMarks()));
			}
		}

		public void setMarks(object selectedLab)
		{
			if (selectedLab == null)
			{
				resetSelectedMarkDetails();
				return;
			}

			var shortName = selectedLab.ToString();
			if (string.IsNullOrWhiteSpace(shortName))
			{
				resetSelectedMarkDetails();
				return;
			}

			if (trySetLabMarkDetails(shortName) || trySetPracticalMarkDetails(shortName))
			{
				return;
			}

			resetSelectedMarkDetails();
		}

		private async Task saveMarks()
		{
			string body = ""; 
			string link = "";
			if (_title == CrossLocalization.Translate("practice_mark"))
			{
				link = Links.SaveSinglePract;
				SavePractSingle save = new SavePractSingle();
				foreach (var pract in _takedLabs.Practicals.Where(v => v.ShortName == SelectedShortName))
				{
					save.practicalId = pract.PracticalId;
					save.subjectId = _services.Preferences.ChosenSubjectId;
					foreach (var practic in fullPractice.Students.Where(v => v.FullName == studentName))
					{
						save.studentId = practic.StudentId;
						save.showForStudent = ShowForStud;
						save.mark = MarkStudent;
						save.Comment = Comment;
						save.date = string.IsNullOrWhiteSpace(SelectedDate)
							? DateTime.Today.ToString("dd.MM.yyyy")
							: SelectedDate;
						foreach (var practMark in practic.PracticalsMarks.Where(v => v.PracticalId == save.practicalId))
						{
							save.id = practMark.StudentPracticalMarkId;
						}
					}
				}
				body = JsonController.ConvertObjectToJson(save);
			}
			else if (_title == CrossLocalization.Translate("stats_page_labs_rating"))
			{
				link = Links.SaveSingleLab;
				SaveLabsSingle save = new SaveLabsSingle();
				foreach (var lab in _takedLabs.Labs.Where(v => v.ShortName == SelectedShortName && v.SubGroup == _subGruop))
				{
					save.labId = lab.LabId;
					foreach (var labs in fullMarksLabs.Students.Where(v => v.FullName == studentName))
					{
						save.studentId = labs.StudentId;
						save.showForStudent = ShowForStud;
						save.mark = MarkStudent;
						save.Comment = Comment;
						save.date = string.IsNullOrWhiteSpace(SelectedDate)
							? DateTime.Today.ToString("dd.MM.yyyy")
							: SelectedDate;
						foreach (var labMark in labs.LabsMarks.Where(v => v.LabId == save.labId))
						{
							save.id = labMark.StudentLabMarkId;
						}
					} 
				}
				body = JsonController.ConvertObjectToJson(save);
			}
			
			var response = await AppServicesController.Request(link, body);
			if (response.Value == HttpStatusCode.OK ||
				response.Value == HttpStatusCode.NoContent ||
				response.Value == HttpStatusCode.Created)
			{
				applySavedMarkToLocalCache();
			}
			await _services.Navigation.ClosePage(false);

			return;
		}

		void applySavedMarkToLocalCache()
		{
			if (_title == CrossLocalization.Translate("practice_mark"))
			{
				applyPracticalMarkToLocalCache();
				return;
			}

			if (_title == CrossLocalization.Translate("stats_page_labs_rating"))
			{
				applyLabMarkToLocalCache();
			}
		}

		void applyLabMarkToLocalCache()
		{
			var selectedLab = _takedLabs?.Labs?
				.FirstOrDefault(v => v.ShortName == SelectedShortName && v.SubGroup == _subGruop);
			var student = fullMarksLabs?.Students?.FirstOrDefault(v => v.FullName == studentName);
			if (selectedLab == null || student == null)
			{
				return;
			}

			var mark = student.LabsMarks?.FirstOrDefault(v => v.LabId == selectedLab.LabId);
			if (mark == null)
			{
				mark = new LabMarks {
					LabId = selectedLab.LabId,
					StudentId = student.StudentId
				};
				if (student.LabsMarks == null)
				{
					student.LabsMarks = new List<LabMarks>();
				}
				student.LabsMarks.Add(mark);
			}

			mark.Mark = MarkStudent.ToString();
			mark.Comment = Comment ?? string.Empty;
			mark.ShowForStudent = ShowForStud;
			mark.Date = SelectedDate;
		}

		void applyPracticalMarkToLocalCache()
		{
			var selectedPractical = _takedLabs?.Practicals?
				.FirstOrDefault(v => v.ShortName == SelectedShortName && v.SubGroup == _subGruop);
			var student = fullPractice?.Students?.FirstOrDefault(v => v.FullName == studentName);
			if (selectedPractical == null || student == null)
			{
				return;
			}

			var mark = student.PracticalsMarks?.FirstOrDefault(v => v.PracticalId == selectedPractical.PracticalId);
			if (mark == null)
			{
				mark = new PracticialMarks {
					PracticalId = selectedPractical.PracticalId,
					StudentId = student.StudentId
				};
				if (student.PracticalsMarks == null)
				{
					student.PracticalsMarks = new List<PracticialMarks>();
				}
				student.PracticalsMarks.Add(mark);
			}

			mark.Mark = MarkStudent.ToString();
			mark.Comment = Comment ?? string.Empty;
			mark.ShowForStudent = ShowForStud;
			mark.Date = SelectedDate;
		}

		string _selectedShortName;
		public string SelectedShortName
		{
			get { return _selectedShortName; }
			set { SetProperty(ref _selectedShortName, value); }
		}

		bool _showForStud;
		public bool ShowForStud
		{
			get { return _showForStud; }
			set { SetProperty(ref _showForStud, value); }
		}

		int _mark;
		public int Mark
		{
			get { return _mark; }
			set { SetProperty(ref _mark, value); }
		}

		int _markStudent;
		public int MarkStudent
		{
			get { return _markStudent; }
			set { SetProperty(ref _markStudent, value); }
		}

		string _comment;
		public string Comment
		{
			get { return _comment; }
			set { SetProperty(ref _comment, value); }
		}

		string _selectedDate;
		public string SelectedDate
		{
			get { return _selectedDate; }
			set { SetProperty(ref _selectedDate, value); }
		}

		bool trySetLabMarkDetails(string shortName)
		{
			var selectedLab = _takedLabs?.Labs?
				.FirstOrDefault(v => v.ShortName == shortName && v.SubGroup == _subGruop);
			if (selectedLab == null)
			{
				return false;
			}

			var student = fullMarksLabs?.Students?.FirstOrDefault(v => v.FullName == studentName);
			if (student == null)
			{
				return false;
			}

			var mark = student.LabsMarks?.FirstOrDefault(v => v.LabId == selectedLab.LabId);
			setSelectedMarkDetails(mark?.Mark, mark?.Comment, mark?.ShowForStudent ?? false, mark?.Date);
			return true;
		}

		bool trySetPracticalMarkDetails(string shortName)
		{
			var selectedPractical = _takedLabs?.Practicals?
				.FirstOrDefault(v => v.ShortName == shortName && v.SubGroup == _subGruop);
			if (selectedPractical == null)
			{
				return false;
			}

			var student = fullPractice?.Students?.FirstOrDefault(v => v.FullName == studentName);
			if (student == null)
			{
				return false;
			}

			var mark = student.PracticalsMarks?
				.FirstOrDefault(v => v.PracticalId == selectedPractical.PracticalId);
			setSelectedMarkDetails(mark?.Mark, mark?.Comment, mark?.ShowForStudent ?? false, mark?.Date);
			return true;
		}

		void setSelectedMarkDetails(string mark, string comment, bool showForStud, string date)
		{
			MarkStudent = int.TryParse(mark, out var parsedMark) ? parsedMark : 0;
			Comment = comment ?? string.Empty;
			ShowForStud = showForStud;
			SelectedDate = string.IsNullOrWhiteSpace(date)
				? DateTime.Today.ToString("dd.MM.yyyy")
				: date;
		}

		void resetSelectedMarkDetails()
		{
			MarkStudent = 0;
			Comment = string.Empty;
			ShowForStud = false;
			SelectedDate = DateTime.Today.ToString("dd.MM.yyyy");
		}
	}
}
