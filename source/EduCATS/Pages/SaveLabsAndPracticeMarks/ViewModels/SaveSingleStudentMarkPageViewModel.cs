using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Json;
using EduCATS.Networking;
using EduCATS.Networking.AppServices;
using EduCATS.Networking.Models.SaveMarks;
using EduCATS.Networking.Models.SaveMarks.LabSchedule;
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
		public TakedLabs _takedLabs { get; set; }
		public string _title { get; set; }
		public string studentName { get; set; }
		public int _subGruop { get; set; }

		public LabsVisitingList fullMarksLabs = new LabsVisitingList();
		public LabsVisitingList fullPractice = new LabsVisitingList();

		public SaveSingleStudentMarkPageViewModel(IPlatformServices services,
			string nameofLabOrPr, LabsVisitingList marks, TakedLabs prOrLabStat, string title, string _studName, int subGruop)
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
			SelectedShortName = nameofLabOrPr;
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

		private async Task<KeyValuePair<string, HttpStatusCode>> saveMarks()
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
					foreach (var practic in fullPractice.Students.Where(v => v.FullName == studentName))
					{
						save.studentId = practic.StudentId;
						save.showForStudent = ShowForStud;
						save.mark = Mark;
						save.comment = Comment;
						save.date = DateTime.Today.ToString("dd.MM.yyyy");
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
						save.mark = Mark;
						save.comment = Comment;
						save.date = DateTime.Today.ToString("dd.MM.yyyy");
						foreach (var labMark in labs.LabsMarks.Where(v => v.LabId == save.labId))
						{
							save.id = labMark.StudentLabMarkId;
						}
					} 
				}
				body = JsonController.ConvertObjectToJson(save);
			}
			await _services.Navigation.ClosePage(false);
			return await AppServicesController.Request(link, body);
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
	}
}
